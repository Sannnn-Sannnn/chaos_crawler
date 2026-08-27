using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace Dungeon
{
  public class EnemySpawner : MonoBehaviour
  {

    [Header("EnemyPrefabs")] [SerializeField]
    private GameObject[] enemyPrefabs;

    [Header("Stats")] [SerializeField] private int minEnemies;
    [SerializeField] private int maxEnemies;
    [SerializeField] private int roomHeight;
    [SerializeField] private int roomWidth;
    private int _enemyAmount;
    private int _enemyCount;
    private List<Vector2> _spawnPositions = new();

    private DoorTrigger _trigger;

    private void Awake()
    {
      _trigger = GetComponent<DoorTrigger>();
      if (_trigger == null) Debug.Log("TRIGGER IS NULL");

      if (minEnemies < 0) minEnemies = 0;
      if (maxEnemies < 0) maxEnemies = 0;
      if (minEnemies > maxEnemies) maxEnemies = minEnemies;
      _enemyAmount = Random.Range(minEnemies, maxEnemies + 1);
    }

    public void SpawnEnemies()
    {
      for (int i = 0; i < _enemyAmount; i++)
      {
        var spawnPosition = GetRandomPosition();
        var enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        var enemyInstance = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        var enemyHealth = enemyInstance.GetComponent<EnemyHealth>();
        enemyHealth.SetSpawner(this);
        _enemyCount++;
      }
    }

    private Vector3 GetRandomPosition()
    {
      while (true)
      {
        var posX = Random.Range(-roomHeight / 2, roomHeight / 2 + 1);
        var posY = Random.Range(-roomWidth / 2, roomWidth / 2 + 1);
        var spawnPosition = transform.position + new Vector3(posX, posY, transform.position.z);
        if (!_spawnPositions.Contains(spawnPosition))
        {
          _spawnPositions.Add(spawnPosition);
          return spawnPosition;
        }
      }
    }

    public void Down()
    {
      _enemyCount--;
      if (_enemyCount == 0)
      {
        _trigger.Complete();
      }
    }

  }
}