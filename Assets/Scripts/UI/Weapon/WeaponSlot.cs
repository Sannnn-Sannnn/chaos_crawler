using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Weapon;

public class WeaponSlot : MonoBehaviour
{

  [SerializeField] [CanBeNull] private WeaponBase weapon;

  [SerializeField] private Image weaponSprite;
  [SerializeField] private TextMeshProUGUI text;
  
  [SerializeField] private Color defaultColour = new Color32(255, 255, 255, 90);
  [SerializeField] private Color selectedColour = new Color32(0, 255, 255, 255);
  [SerializeField] private Color fixedColour = new Color32(255, 255, 0, 255);
  
  [SerializeField] private Image[] statusIndicator;
  
  [SerializeField] private bool isFixed = false;

  private void Update()
  {
    if (weapon == null) Empty();
    else text.text = weapon.GetUsagesLeft() > 0 ? weapon.GetUsagesLeft().ToString() : "∞";
  }

  private void SetColour(Color newColour)
  {
    foreach (Image indicator in statusIndicator)
    {
      indicator.color = newColour; 
    }
  }

  public void SetSelected(bool selected)
  {
    if (selected)
    {
      SetColour(selectedColour);
    }
    else
    {
        SetColour(isFixed ? fixedColour : defaultColour);
    }
  }
  
  public void SetFixed(bool isFixed)
  {
    this.isFixed = isFixed;
  }

  public void SetWeapon(WeaponBase newWeapon)
  {
    weapon = newWeapon;
    weaponSprite.sprite = newWeapon.GetSprite();
    weaponSprite.enabled = true;
  }
  
  public void Empty()
  {
    weapon = null;
    text.text = "";
    weaponSprite.enabled = false;
  }
  
}