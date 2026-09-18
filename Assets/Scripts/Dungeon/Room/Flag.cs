using UnityEngine;

namespace Dungeon
{
  public class Flag : MonoBehaviour
  {

    private GameManager _gameManager;

    private void Awake()
    {
      _gameManager = GameObject.FindAnyObjectByType<GameManager>();
    }

    public void Reset()
    {
      _gameManager.NextFloor();
    }
  }
}