using UnityEngine;

namespace Weapon
{
    public class Spark : MonoBehaviour, IBullet
    {
        private Vector2 _direction = Vector2.zero;
        [Header("Lifetime")]
        [SerializeField] private float lifetime = 5f;
        [Range(0f, 1f)] [SerializeField] private float slowdownLifetimePercentage = 0.5f;

        [Header("Projectile")]
        [SerializeField] private int pierceCount = 1;

        private float _initialSpeed = 1f;
        private float _speed = 1f;
        private float _birthTime;
        private int _damage = 0;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _birthTime = Time.time;
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            float age = Time.time - _birthTime;
            if (age >= lifetime)
            {
                Destroy(gameObject);
                return;
            }
            UpdateSpeed(age);
            Vector2 newPosition =
                _rb.position +
                _direction * _speed * Time.fixedDeltaTime;
            _rb.MovePosition(newPosition);
        }

        private void UpdateSpeed(float age)
        {
            // Momento en el que comienza la desaceleración
            float slowdownStartTime =
                lifetime * (1f - slowdownLifetimePercentage);

            // Todavía no comenzó la desaceleración
            if (age < slowdownStartTime)
            {
                _speed = _initialSpeed;
                return;
            }

            // Progreso de la desaceleración entre 0 y 1
            float slowdownProgress = Mathf.InverseLerp(
                slowdownStartTime,
                lifetime,
                age
            );

            // Reduce gradualmente la velocidad hasta 0
            _speed = Mathf.Lerp(
                _initialSpeed,
                0f,
                slowdownProgress
            );
        }

        public void SetDirection(Vector3 direction)
        {
            _direction = direction;
        }

        public void SetDamage(int damage)
        {
            _damage = damage;
        }

        public void SetSpeed(float newSpeed)
        {
            _initialSpeed = newSpeed;
            _speed = newSpeed;
        }

        public void SetLifetime(float newLifetime)
        {
            lifetime = newLifetime;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();

            // Golpeó a un enemigo
            if (enemy != null)
            {
                enemy.Damage(_damage);
                pierceCount--;
                if (pierceCount <= 0)
                {
                    Destroy(gameObject);
                }

                return;
            }

            // Golpeó algo que no es un enemigo:
            // rebota usando la normal de la superficie
            ContactPoint2D contact = collision.GetContact(0);

            _direction = Vector2.Reflect(
                _direction,
                contact.normal
            ).normalized;

            // Actualizar la rotación visual
            transform.right = _direction;
        }
    }
}
