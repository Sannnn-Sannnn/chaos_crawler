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
  
  [SerializeField] private Image weaponOuterRing;
  [SerializeField] private Image counterOuterRing;

  private void Update()
  {
    if (weapon == null) return;
    if (weapon.GetUsagesLeft() < 0)
    {
      text.text = "∞";
    }
    else
    {
      text.text = weapon.GetUsagesLeft().ToString(); 
    }
  }

  public void SetWeapon(WeaponBase newWeapon)
  {
    weapon = newWeapon;
    weaponSprite.sprite = newWeapon.GetSprite();
    weaponSprite.enabled = true;
  }
  
  public void SetColour(Color newColour)
  {
    weaponOuterRing.color = newColour;
    counterOuterRing.color = newColour;
  }
  
  public void Empty()
  {
    weapon = null;
    text.text = "";
    weaponSprite.enabled = false;
  }
  
}