using JetBrains.Annotations;
using UnityEngine;
using Weapon;

public class WeaponRenderer : MonoBehaviour
{
  [Header("Weapon Slots (UI)")] [SerializeField]
  private WeaponSlot[] uiWeaponSlots;

  [SerializeField] private int fixedSlot = 0;

  private void Awake()
  {
    uiWeaponSlots[fixedSlot].SetFixed(true);
  }

  public void RenderWeapon(int index, [CanBeNull] WeaponBase weapon)
  {
    var weaponSlot = uiWeaponSlots[index];
    if (weapon == null)
    {
      weaponSlot.Empty();
    }
    else
    {
      weaponSlot.SetWeapon(weapon);
    }
  }

  public void SetSelected(int selectedIndex)
  {
    for (int i = 0; i < uiWeaponSlots.Length; i++)
    {
      var slot = uiWeaponSlots[i];
      slot.SetSelected(selectedIndex == i);
    }
  }
}