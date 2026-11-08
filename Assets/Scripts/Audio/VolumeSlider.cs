using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Audio
{
  public class VolumeSlider : MonoBehaviour
  {
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioManager.VolumeChannel channel;
    [SerializeField] private Slider slider;

    private void Start()
    {
      slider.SetValueWithoutNotify(audioManager.GetVolume(channel));
      slider.onValueChanged.AddListener(v => audioManager.SetVolume(channel, v));
    }
  }
}