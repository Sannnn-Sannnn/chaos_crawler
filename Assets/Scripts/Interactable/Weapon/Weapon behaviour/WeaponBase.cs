using UnityEngine;

namespace Weapon
{
  [RequireComponent(typeof(Collider2D))]
  public abstract class WeaponBase : MonoBehaviour, IInteractable
  {
    [Header("Weapon Base Setup")]
    [SerializeField] protected Transform firePoint;

    [SerializeField] protected int usagesLeft;

    protected Collider2D PickupCollider;
    protected SpriteRenderer WeaponSprite;
    protected float LastShotTime = -1000f;

    public bool IsEquipped { get; protected set; }
    
    public Sprite GetSprite() => WeaponSprite.sprite;
    public int GetUsagesLeft() => usagesLeft;

    protected virtual void Awake()
    {
      PickupCollider = GetComponent<Collider2D>();
      WeaponSprite = transform.Find("Sprite")?.GetComponent<SpriteRenderer>();
    }

    // --- IINTERACTABLE ---
    public virtual void Interact(WeaponController controller)
    {
      if (!IsEquipped)
      {
        controller.EquipWeapon(this);
      }
    }

    // Cada arma implementa su propia forma de disparar
    public abstract void Shoot(Vector2 aimDirection);

    public void Use()
    {
      usagesLeft--;
      if (usagesLeft == 0) Destroy(gameObject);
    }

    // --- ESTADOS: EQUIPADA / EN SUELO ---
    public virtual void SetEquipped(Transform parentHoldPoint)
    {
      IsEquipped = true;
      PickupCollider.enabled = false;

      transform.SetParent(parentHoldPoint);
      transform.localPosition = Vector3.zero;
      transform.localRotation = Quaternion.identity;
    }

    public virtual void SetDropped()
    {
      IsEquipped = false;
      transform.SetParent(null);
      PickupCollider.enabled = true;
    }

    public virtual void FlipSpriteY(bool flip)
    {
      if (WeaponSprite != null) WeaponSprite.flipY = flip;
    }
  }
}