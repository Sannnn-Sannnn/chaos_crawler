using UnityEngine;

namespace Dungeon
{
  [CreateAssetMenu(fileName = "EnemySpawnerData", menuName = "SpawnerData", order = 0)]
  public class EnemySpawnerData : ScriptableObject
  {
    
    /*
     * Enemy types:
     *   * Common
     *   * Ranged
     *   * Bulky
     *   * Sprinter
     */

    
    
    public int GetRandomEnemyHp(string enemyType, int floor)
    {
      switch (enemyType)
      {
       case "common":
         return 10;
       case "ranged":
         return 10;
       case "bulky":
         return 10;
       case "sprinter":
         return 2;
       default:
         return 1;
      }
    }
  }
}