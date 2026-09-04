using UnityEngine;

namespace Weapon
{
  public class Shotgun : WeaponBase
  {
    [Header("Bullet Setup")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("Stats")]
    [SerializeField] private int damage = 5;
    [SerializeField] private float cooldown = 0.8f;
    [SerializeField] private int bulletCount = 6;
    [SerializeField] private float shotAngleDeviation = 15f;
    [SerializeField] private float speed = 12f;
    [SerializeField] private float bulletSpeedDeviation = 2f;

    public override void Shoot(Vector2 aimDirection)
    {
      if (!IsEquipped || Time.time - LastShotTime < cooldown) return;
      LastShotTime = Time.time;

      Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

      for (int i = 0; i < bulletCount; i++)
      {
        // Variación angular aleatoria por proyectil
        float randomAngle = Random.Range(-shotAngleDeviation, shotAngleDeviation);
        Quaternion deviation = Quaternion.Euler(0f, 0f, randomAngle);
        Vector2 pelletDirection = deviation * aimDirection;

        // Instanciar proyectil
        var bulletInstance = Instantiate(bulletPrefab, spawnPos, transform.rotation * deviation);

        if (bulletInstance.TryGetComponent<IBullet>(out var bulletComponent))
        {
          bulletComponent.SetDamage(damage);
          bulletComponent.SetDirection(pelletDirection);
                    
          float randomizedSpeed = speed + Random.Range(-bulletSpeedDeviation, bulletSpeedDeviation);
          bulletComponent.SetSpeed(randomizedSpeed);
        }
      }
    }
  }
}