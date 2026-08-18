using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
  public class DungeonGenerator : MonoBehaviour
  {
    [Header("Dungeon parameters")] [SerializeField]
    private int maxRooms = 12;

    [SerializeField] private int maxIterations = 100;

    [SerializeField] private int minDistance = 4;
    [SerializeField] private int maxDistance = 6;
    [SerializeField] private int minX = 0;

    [SerializeField] private float initialProbability = 0.3f;
    [SerializeField] private float minProbability = 0.0f;
    [SerializeField] private float probabilityDecrement = 0.0f;

    [SerializeField] private int gridScale = 20;

    /*
    [Header("Room Prefabs")]
    [SerializeField] private GameObject[] roomPrefabs;
    [SerializeField] private GameObject startRoom;
    [SerializeField] private GameObject endRoom;
    [SerializeField] private GameObject verticalCorridor;
    [SerializeField] private GameObject horizontalCorridor;
     */

    [Header("Placeholders")] [SerializeField]
    private GameObject roomPref;

    [SerializeField] private GameObject startRoomPref;
    [SerializeField] private GameObject endRoomPref;
    [SerializeField] private GameObject verticalCorridorPref;
    [SerializeField] private GameObject horizontalCorridorPref;

    private Dictionary<Vector2Int, GameObject> _rooms = new();
    private List<GameObject> _corridors = new();

    public void Awake()
    {
      // Generate dungeon data
      (List<Vector2Int>, List<(Vector2Int, Vector2Int)>, Vector2Int) dungeon = DungeonGeneration.GenerateDungeon(
        maxRooms, maxIterations, minDistance, maxDistance, minX, initialProbability, minProbability,
        probabilityDecrement);

      // Instantiate rooms
      foreach (Vector2Int room in dungeon.Item1)
      {
        if (!room.Equals(Vector2Int.zero) && !room.Equals(dungeon.Item3))
        {
          var roomObject = Instantiate(roomPref, new Vector3(room.x, room.y, 0) * gridScale, Quaternion.identity);
          _rooms.Add(room, roomObject);
        }
      }

      // Start and end rooms
      var startRoomObject = Instantiate(startRoomPref, Vector3.zero, Quaternion.identity);
      var endRoomObject = Instantiate(endRoomPref, new Vector3(dungeon.Item3.x, dungeon.Item3.y, 0) * gridScale,
        Quaternion.identity);
      _rooms.Add(Vector2Int.zero, startRoomObject);
      _rooms.Add(dungeon.Item3, endRoomObject);

      // Create corridors
      foreach ((Vector2Int, Vector2Int) corridor in dungeon.Item2)
      {
        var room1 = corridor.Item1;
        var room2 = corridor.Item2;
        var x = (float)(room1.x + room2.x) / 2;
        var y = (float)(room1.y + room2.y) / 2;
        var position = new Vector3(x, y, 0) * gridScale;

        var corridorPref = room1.x == room2.x ? verticalCorridorPref : horizontalCorridorPref;
        var corridorObject = Instantiate(corridorPref, position, Quaternion.identity); 
        _corridors.Add(corridorObject);
        
        // Update doors
        // Case: vertical corridor
        if (room1.x == room2.x)
        {
          // Case: room1 below room2
          if (room1.y < room2.y)
          {
            _rooms[room1].GetComponent<Room>().SetDoor("north", true);
            _rooms[room2].GetComponent<Room>().SetDoor("south", true);
          }
          // Case: room1 above room2
          else
          {
            _rooms[room1].GetComponent<Room>().SetDoor("south", true);
            _rooms[room2].GetComponent<Room>().SetDoor("north", true);
          }
        }
        // Case: horizontal corridor
        else
        {
          // Case: room1 --- room2
          if (room1.x < room2.x)
          {
            _rooms[room1].GetComponent<Room>().SetDoor("east", true);
            _rooms[room2].GetComponent<Room>().SetDoor("west", true);
          }
          // Case: room2 --- room1
          else
          {
            _rooms[room1].GetComponent<Room>().SetDoor("west", true);
            _rooms[room2].GetComponent<Room>().SetDoor("east", true);
          }
        }
        
      }
    }

    public void regenerateDungeon()
    {
      foreach (Vector2Int room in _rooms.Keys)
      {
        Destroy(_rooms[room]);
      }

      _rooms.Clear();
      foreach (GameObject corridor in _corridors)
      {
        Destroy(corridor);
      }

      _corridors.Clear();
      Awake();

      var player = GameObject.FindGameObjectWithTag("Player");
      player.transform.position = Vector3.zero;
    }

    public void ToggleAllDoors()
    {
      foreach (Vector2Int room in _rooms.Keys)
      {
        _rooms[room].GetComponent<Room>().ToggleOpen();
      }
    }

  }
}