using UnityEngine;

namespace Dungeon
{
  public class Flag : MonoBehaviour
  {

    private GameManager _gameManager;
    private AudioManager _audioManager;

    private void Awake()
    {
      _gameManager = FindAnyObjectByType<GameManager>();
      _audioManager = GameObject.FindWithTag("GameManager").GetComponent<AudioManager>();
    }

    public void Reset()
    {
      _gameManager.NextFloor();
      _audioManager.PlaySfx(AudioManager.SoundId.PortalWarp);
    }
  }
}