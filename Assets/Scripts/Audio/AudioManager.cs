using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SoundId
    {
        PlayerAttack,
        PlayerDamage,
        PlayerDeath,
        PlayerSwapWeapon,
        PlayerDash,
        EnemyAttack,
        EnemyDamage,
        EnemyDeath,
        PortalWarp,
        Door,
        ButtonClick,
        Consumable
    }

    public enum MusicId
    {
        MainMenu,
        Dungeon
    }

    public enum AmbienceId
    {
        PortalAmbience
    }

    public enum VolumeChannel
    {
        Music,
        Ambience,
        Sfx
    }

    [Serializable]
    private struct SfxEntry
    {
        public SoundId id;
        public AudioClip[] clips;
    }

    [Serializable]
    private struct MusicEntry
    {
        public MusicId id;
        public AudioClip clip;
    }

    [SerializeField] private SfxEntry[] sfxEntries;
    [SerializeField] private MusicEntry[] musicEntries;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambienceSource;
    [SerializeField] private AudioSource sfxSource;
    
    [SerializeField, Range(0f, 1f)] private float defaultMusicVolume = 0.7f;
    [SerializeField, Range(0f, 1f)] private float defaultAmbienceVolume = 0.7f;
    [SerializeField, Range(0f, 1f)] private float defaultSfxVolume = 1f;
    
    private const string MusicKey = "Volume_Music";
    private const string AmbienceKey = "Volume_Ambience";
    private const string SfxKey = "Volume_Sfx";
    
    private Dictionary<SoundId, SfxEntry> _sfx;
    private Dictionary<MusicId, AudioClip> _music;
    
    private GameObject _player;
    private GameObject _flag;

    private void Awake()
    {
        _sfx = new Dictionary<SoundId, SfxEntry>();
        foreach (var e in sfxEntries) _sfx[e.id] = e;

        _music = new Dictionary<MusicId, AudioClip>();
        foreach (var e in musicEntries) _music[e.id] = e.clip;
        
        _player = GameObject.FindGameObjectWithTag("Player");
        _flag = GameObject.FindGameObjectWithTag("Flag");

        LoadVolumes();
    }
    
    private void LoadVolumes()
    {
        musicSource.volume = PlayerPrefs.GetFloat(MusicKey, defaultMusicVolume);
        ambienceSource.volume = PlayerPrefs.GetFloat(AmbienceKey, defaultAmbienceVolume);
        sfxSource.volume = PlayerPrefs.GetFloat(SfxKey, defaultSfxVolume);
    }
    
    public void SetVolume(VolumeChannel channel, float value)
    {
        value = Mathf.Clamp01(value);
        GetSource(channel).volume = value;
        PlayerPrefs.SetFloat(GetKey(channel), value);
    }

    public void SetMusicVolume(float v) => SetVolume(VolumeChannel.Music, v);
    public void SetAmbienceVolume(float v) => SetVolume(VolumeChannel.Ambience, v);
    public void SetSfxVolume(float v) => SetVolume(VolumeChannel.Sfx, v);

    public void SaveVolumes()
    {
        PlayerPrefs.Save();
    }

    private AudioSource GetSource(VolumeChannel channel) => channel switch
    {
        VolumeChannel.Music => musicSource,
        VolumeChannel.Ambience => ambienceSource,
        _ => sfxSource
    };

    private string GetKey(VolumeChannel channel) => channel switch
    {
        VolumeChannel.Music => MusicKey,
        VolumeChannel.Ambience => AmbienceKey,
        _ => SfxKey
    };
    
    public float GetVolume(VolumeChannel channel)
    {
        return PlayerPrefs.GetFloat(GetKey(channel), GetDefault(channel));
    }

    private float GetDefault(VolumeChannel channel) => channel switch
    {
        VolumeChannel.Music => defaultMusicVolume,
        VolumeChannel.Ambience => defaultAmbienceVolume,
        _ => defaultSfxVolume
    };

    private void OnApplicationPause(bool paused) { if (paused) SaveVolumes(); }
    private void OnApplicationQuit() => SaveVolumes();


    public void PlaySfx(SoundId id)
    {
        if (!_sfx.TryGetValue(id, out var e) || e.clips.Length == 0)
        {
            Debug.LogWarning($"SFX no configurado: {id}");
            return;
        }

        var clip = e.clips[UnityEngine.Random.Range(0, e.clips.Length)];
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(MusicId id)
    {
        if (!_music.TryGetValue(id, out var clip)) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayAmbience(AmbienceId id)
    {
        return;
    }
}