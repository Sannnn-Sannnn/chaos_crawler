using System;
using Player;
using UnityEngine;

namespace Dungeon
{
  public class DoorTrigger : MonoBehaviour
  {

    [SerializeField] private bool activated = false;

    private DungeonGenerator _generator;
    private EnemySpawner _spawner;
    
    private void Awake()
    {
      _generator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
      _spawner = GetComponent<EnemySpawner>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      var player = other.gameObject.GetComponent<PlayerHealth>();
      if (player == null) return;

      if (!activated)
      {
        activated = true;
        _generator.ToggleAllDoors();
        _spawner.SpawnEnemies();
      }
    }

    public void Complete()
    {
      _generator.ToggleAllDoors();
    }
  }
}