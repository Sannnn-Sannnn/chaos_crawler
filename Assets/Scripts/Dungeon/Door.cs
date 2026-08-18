using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dungeon
{
  public class Door : MonoBehaviour
  {
    [SerializeField] private bool isOpen = true;
    [SerializeField] private bool isWall = true;

    private GameObject _door;
    private GameObject _wall;

    public void Awake()
    {
      _door = transform.Find("Door").gameObject;
      _wall = transform.Find("Wall").gameObject;
      _door.SetActive(false);
    }

    public void Toggle()
    {
      if (isWall) return;
      isOpen = !isOpen;
      _door.SetActive(!isOpen);
    }

    public void SetAs(string option)
    {
      if (option == "wall")
      {
        isWall = true;
        _wall.SetActive(true);
        _door.SetActive(false);
      }
      else if (option == "door")
      {
        isWall = false;
        _wall.SetActive(false);
      }
    }
  }
}