using System;
using UnityEngine;

namespace Dungeon
{
  public class Room : MonoBehaviour
  {
    [SerializeField] private bool hasDoors = true;
    [SerializeField] private bool isOpen = true;
    private Door _northDoor;
    private Door _eastDoor;
    private Door _southDoor;
    private Door _westDoor;

    public void Awake()
    {
      var doors = transform.Find("Doors");
      if (doors != null)
      {
        _northDoor = doors.transform.Find("North door").gameObject.GetComponent<Door>();
        _eastDoor = doors.transform.Find("East door").gameObject.GetComponent<Door>();
        _southDoor = doors.transform.Find("South door").gameObject.GetComponent<Door>();
        _westDoor = doors.transform.Find("West door").gameObject.GetComponent<Door>();
      }
      else
      {
        Debug.Log($"No doors found at ({transform.position.x}, {transform.position.y})");
      }
    }

    public void SetDoor(string direction, bool isDoor)
    {
      var state = isDoor ? "door" : "wall";
      switch (direction)
      {
        case "north":
          if (_northDoor == null)
          {
            Debug.Log("North null");
            break;
          }
          _northDoor.SetAs(state);
          break;
        case "east":
          if (_eastDoor == null)
          {
            Debug.Log("East null");
            break;
          }
          _eastDoor.SetAs(state);
          break;
        case "south":
          if (_southDoor == null)
          {
            Debug.Log("South null");
            break;
          }
          _southDoor.SetAs(state);
          break;
        case "west":
          if (_westDoor == null)
          {
            Debug.Log("West null");
            break;
          }
          _westDoor.SetAs(state);
          break;
      }
    }

    public void ToggleOpen()
    {
      if (!hasDoors) return;
      isOpen = !isOpen;
      _northDoor.Toggle();
      _eastDoor.Toggle();
      _southDoor.Toggle();
      _westDoor.Toggle();
    }
  }
}