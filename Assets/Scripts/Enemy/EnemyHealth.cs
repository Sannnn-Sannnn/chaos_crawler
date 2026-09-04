using Dungeon;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    private EnemyDropBehaviour _dropper;
    private EnemySpawner _spawner;
    
    void Awake()
    {
        health = maxHealth;
        _dropper = GetComponent<EnemyDropBehaviour>();
    }
    
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    public void SetSpawner(EnemySpawner spawner)
    {
        _spawner = spawner;
    }

    public void Damage(int damage)
    {
        health -= damage;
    }

    private void Die()
    {
        _spawner.Down();
        _dropper.Drop();
        Destroy(gameObject);
    }
    
}
