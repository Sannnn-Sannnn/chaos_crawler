using JetBrains.Annotations;
using UnityEngine;
using Weapon;

public class WeaponRenderer : MonoBehaviour
{
  [Header("Weapon Slots (UI)")] [SerializeField]
  private WeaponSlot[] uiWeaponSlots;

  [SerializeField] private Color defaultColour = new Color32(255, 255, 255, 90);
  [SerializeField] private Color selectedColour = new Color32(0, 255, 255, 90);

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

  public void SetSelected(int index, bool selected)
  {
    uiWeaponSlots[index].SetColour(selected ? selectedColour : defaultColour);
  }
}