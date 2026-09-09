using System.Collections.Generic;
using Enemy;
using UnityEditorInternal;
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
    [SerializeField] private int minDoorDistance = 0;
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
        var enemyVariant = Random.Range(0, enemyPrefabs.Length);
        var enemyPrefab = enemyPrefabs[enemyVariant];
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
        if (!_spawnPositions.Contains(spawnPosition) || IsCloseToDoor(spawnPosition))
        {
          _spawnPositions.Add(spawnPosition);
          return spawnPosition;
        }
      }
    }

    private bool IsCloseToDoor(Vector3 pos)
    {
      var northDoor = new Vector2(0, roomHeight / 2f);
      var eastDoor = new Vector2(roomWidth / 2f, 0);
      var southDoor = new Vector2(0, -roomHeight / 2f);
      var westDoor = new Vector2(-roomWidth / 2f, 0);

      float[] distances =
      {
        Vector3.Distance(pos, northDoor), Vector3.Distance(pos, eastDoor), Vector3.Distance(pos, southDoor),
        Vector3.Distance(pos, westDoor)
      };

      foreach (var distance in distances)
      {
        if (distance > minDoorDistance)
        {
          return true;
        }
      }
      return false;
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