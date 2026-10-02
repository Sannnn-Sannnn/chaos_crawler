using UnityEngine;

namespace Dungeon
{
  [CreateAssetMenu(fileName = "EnemySpawnerData", menuName = "SpawnerData", order = 0)]
  public class EnemySpawnerData : ScriptableObject
  {
    [SerializeField] private int minEnemiesPerFloor;
    [SerializeField] private int maxEnemiesPerFloor;

    public int GetEnemies(int floor)
    {
      
      return 1;
    }
  }
}