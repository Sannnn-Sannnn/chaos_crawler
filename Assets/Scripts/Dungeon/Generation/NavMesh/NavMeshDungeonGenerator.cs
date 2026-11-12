using Dungeon;
using UnityEngine;

public class NavMeshDungeonGenerator : DungeonGenerator
{

  [SerializeField] private NavMeshDungeon dungeon;
  
  private NavMeshDungeon _currentDungeon = null;

  public override void RegenerateDungeon()
  {
    Destroy(_currentDungeon);
    _currentDungeon = Instantiate(dungeon, Vector2.zero, Quaternion.identity);
  }
}