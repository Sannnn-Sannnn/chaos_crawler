using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Weapon;

public class WeaponController : MonoBehaviour
{

    [SerializeField] private WeaponRenderer weaponRenderer;
    
    [Header("Inputs & Listeners")] [SerializeField]
    private InputActionReference mouseClickListener;

    [SerializeField] private MouseListener mouseListener;
    [SerializeField] private WeaponChangeTracker weaponChangeListener;

    [Header("Hold Points")] [SerializeField]
    private Transform[] weaponSlots;

    [Header("Weapon Slots")] [SerializeField]
    private WeaponBase[] equippedWeapons;
    [SerializeField] private int currentWeaponSlot = 0;
    [SerializeField] private int fixedWeaponSlot = 0;

    private WeaponBase _storedWeapon = null;
    
    private WeaponBase _currentWeaponBase;
    private bool _isHoldingClick = false;
    
    private AudioManager _audioManager;
    
    private readonly Color _transparentHighlight = new  Color(0f, 0f, 0f, 0f);
    private readonly Color _visibleHighlight = new  Color(255, 255, 255, 255);

    private void Awake()
    {
        if (mouseListener == null)
        {
            var listenerObject = GameObject.FindGameObjectWithTag("MouseListener");
            if (listenerObject != null)
            {
                mouseListener = listenerObject.GetComponent<MouseListener>();
            }
        }
        _audioManager = GameObject.FindWithTag("GameManager").GetComponent<AudioManager>();
    }

    private void Start()
    {
        if (equippedWeapons == null || weaponSlots == null) return;

        if (equippedWeapons.Length < 2 && fixedWeaponSlot >= 0)
        {
            Destroy(gameObject);
        }
        
        int count = Mathf.Min(equippedWeapons.Length, weaponSlots.Length);
        for (int i = 0; i < count; i++)
        {
            var weapon = equippedWeapons[i];
            if (weapon != null && weaponSlots[i] != null)
            {
                equippedWeapons[i].SetEquipped(weaponSlots[i]);
                weaponRenderer.RenderWeapon(i, weapon);
            }
            else
            {
                weaponRenderer.RenderWeapon(i, null);
            }
        }
    }

    private void OnEnable()
    {
        if (mouseClickListener != null)
        {
            mouseClickListener.action.Enable();
            mouseClickListener.action.started += OnActionStarted;
            mouseClickListener.action.canceled += OnActionCanceled;
        }

        if (weaponChangeListener != null)
        {
            weaponChangeListener.OnWheelScroll += HandleWheelScroll;
            weaponChangeListener.OnNumberPressed += HandleNumberPressed;
        }
    }

    private void OnDisable()
    {
        if (mouseClickListener != null)
        {
            mouseClickListener.action.Disable();
            mouseClickListener.action.started -= OnActionStarted;
            mouseClickListener.action.canceled -= OnActionCanceled;
        }

        if (mouseListener != null)
        {
            weaponChangeListener.OnWheelScroll -= HandleWheelScroll;
            weaponChangeListener.OnNumberPressed -= HandleNumberPressed;

        }
    }

    private void FixedUpdate()
    {
        RotateAndAim();

        if (_isHoldingClick)
        {
            ShootAllWeapons();
        }

        if (equippedWeapons[fixedWeaponSlot] == null && _storedWeapon != null)
        {
            EquipWeapon(_storedWeapon);
            _storedWeapon = null;
        }
    }
    
    // Attack

    private void RotateAndAim()
    {
        if (mouseListener == null) return;

        if (weaponSlots != null)
        {
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if (weaponSlots[i] == null) continue;

                Vector2 aimDir = mouseListener.GetAimDirection(weaponSlots[i].position);
                float slotAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
                weaponSlots[i].rotation = Quaternion.Euler(0f, 0f, slotAngle);

                if (equippedWeapons != null && i < equippedWeapons.Length && equippedWeapons[i] != null)
                {
                    equippedWeapons[i].FlipSpriteY(aimDir.x < 0);
                }
            }
        }

        Vector2 selfDir = mouseListener.GetAimDirection(transform.position);
        float selfAngle = Mathf.Atan2(selfDir.y, selfDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, selfAngle);
    }

    private void ShootAllWeapons()
    {
        if (equippedWeapons == null || mouseListener == null) return;

        for (int i = 0; i < equippedWeapons.Length; i++)
        {
            if (equippedWeapons[i] == null) continue;

            Transform slot = (weaponSlots != null && i < weaponSlots.Length) ? weaponSlots[i] : transform;
            Vector2 aimDir = slot != null ? mouseListener.GetAimDirection(slot.position) : (Vector2)transform.right;

            equippedWeapons[i].Shoot(aimDir);
        }
    }

    private void OnActionStarted(InputAction.CallbackContext context)
    {
        if (_currentWeaponBase != null)
        {
            _currentWeaponBase.Interact(this);
            return;
        }

        _isHoldingClick = true;
        ShootAllWeapons();
    }

    private void OnActionCanceled(InputAction.CallbackContext context)
    {
        _isHoldingClick = false;
    }

    // Equipped weapons
    
    private void HandleWheelScroll(int movement)
    {
        if (equippedWeapons == null || equippedWeapons.Length == 0) return;
        
        int totalSlots = equippedWeapons.Length;
        currentWeaponSlot = (currentWeaponSlot + movement) % totalSlots;
        if (currentWeaponSlot < 0) currentWeaponSlot += totalSlots;
        
        weaponRenderer.SetSelected(currentWeaponSlot);
    }

    private void HandleNumberPressed(int number)
    {
        if (equippedWeapons == null || equippedWeapons.Length == 0) return;
        currentWeaponSlot = number;
        weaponRenderer.SetSelected(currentWeaponSlot);
    }

    public void EquipWeapon(WeaponBase newWeapon)
    {
        CheckNextSlot();
        EquipWeapon(newWeapon, currentWeaponSlot);
    }
    
    private void EquipWeapon(WeaponBase newWeapon, int weaponIndex) {
    if (newWeapon == null || equippedWeapons == null || equippedWeapons.Length == 0) return;
    
        weaponRenderer.SetSelected(weaponIndex);

        Transform targetSlot = (weaponSlots != null && weaponIndex < weaponSlots.Length)
            ? weaponSlots[weaponIndex]
            : transform;

        if (equippedWeapons[weaponIndex] != null)
        {
            equippedWeapons[weaponIndex].SetDropped();
        }

        equippedWeapons[weaponIndex] = newWeapon;
        equippedWeapons[weaponIndex].SetEquipped(targetSlot);
        _currentWeaponBase = null;

        weaponRenderer.RenderWeapon(weaponIndex, newWeapon);
        
        _audioManager.PlaySfx(AudioManager.SoundId.PlayerSwapWeapon);
    }

    private void CheckNextSlot()
    {
        if (currentWeaponSlot < 0 || currentWeaponSlot >= equippedWeapons.Length)
        {
            currentWeaponSlot = 0;
        }

        // Case 1: empty slot 
        if (equippedWeapons[currentWeaponSlot] == null)
        {
            return;
        }
        // Case 2: on common occupied slot with empty slots
        for (int i = 0; i < equippedWeapons.Length; i++)
        {
            var weapon = equippedWeapons[i];
            if (weapon == null)
            {
                currentWeaponSlot = i;
                return;
            }
        }
        // Case 3: on fixed slot, with no empty slots
        if (currentWeaponSlot == fixedWeaponSlot)
        {
            if (_storedWeapon == null)
            {
                _storedWeapon = equippedWeapons[currentWeaponSlot];
                equippedWeapons[currentWeaponSlot] = null;
            }
        }
        // Case 4: on common occupied slot with no empty slots
        return;
    }

    // Triggers

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<WeaponBase>(out var weapon))
        {
            if (_currentWeaponBase != null && _currentWeaponBase != weapon)
            {
                _currentWeaponBase.highlight.color = _transparentHighlight;
            }
            _currentWeaponBase = weapon;
            _currentWeaponBase.highlight.color = _visibleHighlight;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<WeaponBase>(out var weapon))
        {
            if (_currentWeaponBase == weapon)
            {
                _currentWeaponBase.highlight.color = _transparentHighlight;
                _currentWeaponBase = null;
            }
        }
    }
}