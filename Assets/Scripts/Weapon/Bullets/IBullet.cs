using UnityEngine;

namespace Weapon
{
  public interface IBullet
  {
    public void SetDirection(Vector3 direction);
    public void SetDamage(int damage);
    public void SetSpeed(float newSpeed);
    public void SetLifetime(float newLifetime);
  }
}