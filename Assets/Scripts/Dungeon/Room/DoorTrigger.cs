using System;
using UnityEngine;

namespace Dungeon
{
  public class DoorTrigger : MonoBehaviour
  {

    [SerializeField] private bool wasCompleted = false;

    private DungeonGenerator _generator;
    private EnemySpawner _spawner;
    
    private void Awake()
    {
      _generator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
      _spawner = GetComponent<EnemySpawner>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      var player = other.gameObject.GetComponent<PlayerMovement>();
      if (player == null) return;

      if (!wasCompleted)
      {
        _generator.ToggleAllDoors();
        _spawner.SpawnEnemies();
      }
    }

    public void Complete()
    {
      wasCompleted = true;
      _generator.ToggleAllDoors();
    }
  }
}