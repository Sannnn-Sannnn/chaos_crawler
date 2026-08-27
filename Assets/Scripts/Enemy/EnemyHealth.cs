using Dungeon;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    
    void Awake()
    {
        health = maxHealth;
    }
    
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
    }
    
}
