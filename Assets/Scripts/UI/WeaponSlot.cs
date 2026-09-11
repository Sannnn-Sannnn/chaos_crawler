using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
  [SerializeField] private SpriteRenderer weaponSprite;

  private void Awake()
  {
    if (weaponSprite == null) return;
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

}