using UnityEngine;

namespace Dungeon
{
  public class Flag : MonoBehaviour
  {

    private DungeonGenerator _generator;

    private void Awake()
    {
      _generator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
    }

    public void Reset()
    {
      _generator.RegenerateDungeon();
    }
  }
}