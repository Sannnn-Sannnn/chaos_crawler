using System.Collections.Generic;
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
    private int _enemyAmount; // Amount to spawn
    private int _enemyCount;  // Amount spawned
    private List<Vector2> _spawnPositions = new();
    private Transform _spawnAnchor; // (0, 0) from room
    private Vector3 _roomPositionZero;

    private DoorTrigger _trigger;

    private void Awake()
    {
      _trigger = GetComponent<DoorTrigger>();
      if (_trigger == null) Debug.Log("TRIGGER IS NULL");

      if (minEnemies < 0) minEnemies = 0;
      if (maxEnemies < 0) maxEnemies = 0;
      if (minEnemies > maxEnemies) maxEnemies = minEnemies;
      _enemyAmount = Random.Range(minEnemies, maxEnemies + 1);


      _spawnAnchor = transform.parent.Find("EnemySpawnAnchor");
      _roomPositionZero = _spawnAnchor.position;
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
      const int maxAttempts = 100;
      int halfWidth = roomWidth / 2;
      int halfHeight = roomHeight / 2;

      for (int i = 0; i < maxAttempts; i++)
      {
        int posX = Random.Range(-halfWidth + 1, halfWidth);
        int posY = Random.Range(-halfHeight + 1, halfHeight);

        Vector3 spawnPosition = _roomPositionZero + new Vector3(posX, posY, 0);

        if (!_spawnPositions.Contains(spawnPosition) && !IsCloseToDoor(spawnPosition))
        {
          _spawnPositions.Add(spawnPosition);
          return spawnPosition;
        }
      }
      return _roomPositionZero;
    }

    private bool IsCloseToDoor(Vector3 pos)
    {
      Vector3 northDoor = _roomPositionZero + new Vector3(0, roomHeight / 2f, 0);
      Vector3 southDoor = _roomPositionZero + new Vector3(0, -roomHeight / 2f, 0);
      Vector3 eastDoor  = _roomPositionZero + new Vector3(roomWidth / 2f, 0, 0);
      Vector3 westDoor  = _roomPositionZero + new Vector3(-roomWidth / 2f, 0, 0);
      
      return Vector3.Distance(pos, northDoor) < minDoorDistance ||
             Vector3.Distance(pos, southDoor) < minDoorDistance ||
             Vector3.Distance(pos, eastDoor)  < minDoorDistance ||
             Vector3.Distance(pos, westDoor)  < minDoorDistance;
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