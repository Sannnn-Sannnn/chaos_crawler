using Dungeon;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("PlayerAttack"))
        {
            Debug.Log("Enemy damaged");
        }
        else if (other.collider.CompareTag("Player"))
        {
            Debug.Log("Gotcha!");
        }
    }
}
