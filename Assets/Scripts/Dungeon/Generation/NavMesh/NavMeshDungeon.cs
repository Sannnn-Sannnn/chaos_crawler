using System.Collections.Generic;
using Dungeon;
using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshDungeon : MonoBehaviour
{
  [SerializeField] private NavMeshSurface navMesh;
  
  [Header("Dungeon parameters")] [SerializeField]
  private int maxRooms = 12;

  [SerializeField] private int maxIterations = 100;

  [SerializeField] private int minDistance = 4;
  [SerializeField] private int maxDistance = 6;
  [SerializeField] private int minX = 0;

  [SerializeField] private float initialProbability = 0.3f;
  [SerializeField] private float minProbability = 0.0f;
  [SerializeField] private float probabilityDecrement = 0.0f;

  [SerializeField] private int roomSizeX;
  [SerializeField] private int roomSizeY;

  [SerializeField] private bool branchFromStart;
  [SerializeField] private bool branchFromEnd;

  [Header("Room Prefabs")] [SerializeField]
  private GameObject[] roomPrefabs;

  [SerializeField] private GameObject startRoom;
  [SerializeField] private GameObject endRoom;

  private Dictionary<Vector2Int, GameObject> _rooms = new();
  private Dictionary<Vector2, GameObject> _corridors = new();

  [Header("Corridors")] [SerializeField] private GameObject horizontalCorridorPrefab;
  [SerializeField] private GameObject verticalCorridorPrefab;

  [Header("Transition")] [SerializeField]
  private TransitionPanel transitionPanel;

  [SerializeField] private bool hideTransition = true;
  
  void Start()
  {
    navMesh = GameObject.FindGameObjectWithTag("NavMesh").GetComponent<NavMeshSurface>();
    GenerateDungeon();    
    navMesh.BuildNavMeshAsync();
  }

  private void GenerateDungeon()
  {
    // Generate dungeon data
    (List<Vector2Int>, List<(Vector2Int, Vector2Int)>, Vector2Int) dungeon = DungeonGeneration.GenerateDungeon(
      maxRooms, maxIterations, minDistance, maxDistance, minX, initialProbability, minProbability,
      probabilityDecrement, branchFromStart, branchFromEnd);

    // Instantiate rooms
    foreach (Vector2Int room in dungeon.Item1)
    {
      if (!room.Equals(Vector2Int.zero) && !room.Equals(dungeon.Item3))
      {
        var roomPref = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
        var roomObject = Instantiate(roomPref, transform);
        roomObject.transform.position = new Vector3(room.x * roomSizeX, room.y * roomSizeY, 0);
        roomObject.transform.rotation = Quaternion.identity;
        _rooms.Add(room, roomObject);
      }
    }

    // Start and end rooms
    var startRoomObject = Instantiate(startRoom, Vector3.zero, Quaternion.identity);
    startRoomObject.transform.position = Vector3.zero;
    startRoomObject.transform.rotation = Quaternion.identity;
    var endRoomObject = Instantiate(endRoom, transform);
    endRoomObject.transform.position = new Vector3(dungeon.Item3.x * roomSizeX, dungeon.Item3.y * roomSizeY, 0);
    endRoomObject.transform.rotation = Quaternion.identity;
    _rooms.Add(Vector2Int.zero, startRoomObject);
    _rooms.Add(dungeon.Item3, endRoomObject);

    // Create doors
    foreach ((Vector2Int, Vector2Int) corridor in dungeon.Item2)
    {
      var room1 = corridor.Item1;
      var room2 = corridor.Item2;
      var x = (float)(room1.x + room2.x) / 2 * roomSizeX;
      var y = (float)(room1.y + room2.y) / 2 * roomSizeY;

      if (_corridors.ContainsKey(new Vector2(x, y)))
      {
        continue;
      }

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

        var corridorInstance = Instantiate(verticalCorridorPrefab, new Vector3(x, y, 0), Quaternion.identity);
        corridorInstance.transform.position = new Vector3(x, y, 0);
        corridorInstance.transform.rotation = Quaternion.identity;
        _corridors.TryAdd(new Vector2(x, y), corridorInstance);
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

        var corridorInstance = Instantiate(horizontalCorridorPrefab, transform);
        corridorInstance.transform.position = new Vector3(x, y, 0);
        corridorInstance.transform.rotation = Quaternion.identity;
      _corridors.Add(new Vector2(x, y), corridorInstance);
      }
    }
  }
}