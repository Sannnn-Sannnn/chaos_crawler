using UnityEngine;

  public class EnemyDropBehaviour : MonoBehaviour
  {
   
    [Tooltip("Both arrays must have the same length or else the object destroys itself upon awakening.\nChances go from 0 to 100.")]
    [SerializeField] private GameObject[] drops;
    [SerializeField] private float[] dropRates;

    private void Awake()
    {
      if (drops.Length != dropRates.Length)
      {
        Destroy(gameObject);
      }
    }

    public void Drop()
    {
      for (int i = 0; i < drops.Length; i++)
      {
        var dropChance = Random.Range(0, 100f);
        if (dropChance <= dropRates[i])
        {
          Instantiate(drops[i], transform.position, Quaternion.identity);
        }
      }
    }
    
  }