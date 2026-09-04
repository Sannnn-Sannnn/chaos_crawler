using UnityEngine;

namespace Weapon
{
  public class Handgun : WeaponBase
  {
    [Header("Bullet Setup")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("Stats")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float cooldown = 0.2f;
    [SerializeField] private float speed = 15f;

    public override void Shoot(Vector2 aimDirection)
    {
      // Validar si está equipada y si ya pasó el cooldown
      if (!IsEquipped || Time.time - LastShotTime < cooldown) return;
      LastShotTime = Time.time;

      // Determinar posición de spawn (firePoint o la posición del arma)
      Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

      // Instanciar el proyectil con la rotación actual del arma
      var bulletInstance = Instantiate(bulletPrefab, spawnPos, transform.rotation);

      // Configurar los parámetros del proyectil
      if (bulletInstance.TryGetComponent<IBullet>(out var bulletComponent))
      {
        bulletComponent.SetDamage(damage);
        bulletComponent.SetDirection(aimDirection);
        bulletComponent.SetSpeed(speed);
      }
    }
  }
}