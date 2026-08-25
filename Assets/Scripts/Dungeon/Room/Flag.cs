using UnityEngine;

namespace Dungeon
{
  public class Flag : MonoBehaviour
  {

    private DungeonGenerator generator;

    private void Awake()
    {
      generator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
    }

    public void Reset()
    {
      generator.regenerateDungeon();
    }
  }
}