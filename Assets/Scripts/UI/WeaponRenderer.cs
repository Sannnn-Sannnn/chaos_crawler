using UnityEngine;
using UnityEngine.UI;

public class WeaponRenderer : MonoBehaviour
{
  [Header("Weapon Slots (UI)")] [SerializeField]
  private WeaponSlot[] uiWeaponSlots;
  
  [SerializeField] private Color defaultColour = new Color32(255, 255, 255, 90);
  [SerializeField] private Color selectedColour = new Color32(0, 255, 255, 90);
  
  public void RenderWeapon(int index, Sprite sprite)
  {
    var weaponSlot = uiWeaponSlots[index];
    weaponSlot.SetWeaponSprite(sprite);
  }

  public void SetSelected(int index, bool selected)
  {
    if (selected)
    {
      uiWeaponSlots[index].SetColour(selectedColour);      
    }
    else
    {
      uiWeaponSlots[index].SetColour(defaultColour);
    }
  }
}