using UnityEngine;

  public class GeneralAudioInterface : MonoBehaviour
  {
    
    /*
     * This class is meant for starting the music in each scene, as well as allowing objects to call the Button SFX 
     */
    
    [SerializeField] private AudioManager.MusicId musicId;
    
    private AudioManager _audioManager;

    private void Start()
    {
      _audioManager = GameObject.FindWithTag("GameManager").GetComponent<AudioManager>();
      
      _audioManager.PlayMusic(musicId);
    }

    public void PlayButton()
    {
      _audioManager.PlaySfx(AudioManager.SoundId.ButtonClick);
    }
  }