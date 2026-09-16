using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
  [SerializeField] private Image weaponSprite;
  [SerializeField] private Image outerRing;
  
  private void Awake()
  {
  }

  public void SetWeaponSprite(Sprite sprite)
  {
    if (sprite == null)
    {
      weaponSprite.enabled = false;
    }
    else
    {
      weaponSprite.sprite = sprite;
      weaponSprite.enabled = true;
    }
  }
  
  public void SetColour(Color newColour)
  {
    outerRing.color = newColour;
  }

}