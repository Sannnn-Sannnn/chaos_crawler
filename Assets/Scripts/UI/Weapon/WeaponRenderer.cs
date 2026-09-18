using JetBrains.Annotations;
using UnityEngine;
using Weapon;

public class WeaponRenderer : MonoBehaviour
{
  [Header("Weapon Slots (UI)")] [SerializeField]
  private WeaponSlot[] uiWeaponSlots;

  [SerializeField] private int fixedSlot = 0;
  [SerializeField] private Color defaultColour = new Color32(255, 255, 255, 90);
  [SerializeField] private Color selectedColour = new Color32(0, 255, 255, 90);
  [SerializeField] private Color fixedColour = new Color32(0, 255, 255, 90);

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
      slot.SetColour(i == selectedIndex ? selectedColour : i == fixedSlot ? fixedColour : defaultColour);
    }
  }
}