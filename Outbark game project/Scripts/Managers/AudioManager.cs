using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    private AudioMixer mixer;
    public static AudioManager instance;

    public SoundObjects soundObjects;
    public SoundTrackObjects soundTrackObjects;
    public AudioSource NoOverLapSFXSource;
    private Dictionary<AudioType,AudioClip> audioTable = new Dictionary<AudioType, AudioClip>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (var sound in soundObjects.soundObjects)
            {
                audioTable.Add(sound.audioType, sound.clip);
            }
            foreach (var track in soundTrackObjects.soundTrackObjects)
            {
                audioTable.Add(track.audioType, track.clip);
            }
        }
        else Destroy(this);
    }
    public void PlayClipAtPosition(AudioClip clip, float volume, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }

    public void PlaySound(AudioType type)
    {
        if (audioTable.ContainsKey(type))
        {
            soundObjects.source.PlayOneShot(audioTable[type]);
        }
    }

    /// <summary>
    /// used for laser currently
    /// </summary>
    /// <param name="type"></param>
    public void PlaySoundNoOverLap(AudioType type)
    {
        if(!NoOverLapSFXSource.isPlaying)
        {
            NoOverLapSFXSource.clip = audioTable[type];
            NoOverLapSFXSource.Play();
        }

    }
    public void StopSoundNoOverLap()
    {
        if (NoOverLapSFXSource.isPlaying)
        {
            NoOverLapSFXSource.Stop();
        }
    }

    public void PlayTrack(AudioType type)
    {
        if (audioTable.ContainsKey(type))
        {
            soundTrackObjects.source.clip = audioTable[type];
            soundTrackObjects.source.Play();
        }
    }

    public void PlayTrackWithFadeIn(AudioType type,float duration, bool fadeIn)
    {
        if (audioTable.ContainsKey(type))
        {
            soundTrackObjects.source.clip = audioTable[type];
            soundTrackObjects.source.Play();
            FadeMusic(type, 1, fadeIn);
        }
    }

    public void PlayTrackWithTransition(AudioType track, AudioType transition)
    {
        if (audioTable.ContainsKey(transition))
        {
            soundTrackObjects.source.clip = audioTable[transition];
            soundTrackObjects.source.Play();
            StartCoroutine(WaitRoutine(audioTable[transition].length, delegate { PlayTrack(track); } ));
        }
    }
    

    public void FadeMusic(AudioType type, float duration, bool fadeIn)
    {
        StartCoroutine(FadeMusicRoutine(type, duration, fadeIn));
    }

    public IEnumerator FadeMusicRoutine(AudioType type, float duration, bool fadeout)
    {
        float initialVolume;
        float target = 0;
        float timer = 0;
        if (fadeout)
        {
            target = 0;
            initialVolume = 1;
        }
        else
        {
            initialVolume = 0;
            target = 1;
        }
        while(timer < duration)
        {
            soundTrackObjects.source.volume = Mathf.Lerp(initialVolume,target, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        soundTrackObjects.source.volume = target;
       
    }

    public IEnumerator WaitRoutine(float wait,Action a )
    {
        yield return new WaitForSeconds(wait);
        a.Invoke();
    }
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            instance.PlaySound(AudioType.SFXBulletShot);
        }
        if (Input.GetKeyDown(KeyCode.H)){
            instance.PlaySound(AudioType.SFXEnemyDeath);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            instance.PlayTrack(AudioType.STBattle);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            instance.PlayTrackWithTransition(AudioType.STExploring, AudioType.STBattleToExploring);
        }
    }*/


    public void PlayClipAtSource(AudioSource source, AudioClip clip, float volume, float pitch)
    {
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
    }

    public void ChangeMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
    }

    public void ChangeSoundVolume(float volume)
    {
        mixer.SetFloat("AudioVol", Mathf.Log10(volume) * 20);
    }

    public enum SoundType
    {
        Sound,
        Music
    }

    public void StopAudio()
    {
        soundObjects.source.Stop();
        soundTrackObjects.source.Stop();
    }


    [System.Serializable]
    public class SoundObjects
    {
        public SoundType type;
        public AudioSource source;
        public List<AudioObject> soundObjects;
    }

    [System.Serializable]
    public class SoundTrackObjects
    {
        public SoundType type;
        public AudioSource source;
        public List<AudioObject> soundTrackObjects;
    }

    [System.Serializable]
    public class AudioObject
    {
        public AudioType audioType;
        public AudioClip clip;
    }

}
public enum AudioType
{
    STBattle,
    STBoss,
    STExploring,
    STBattleToBoss,
    STBattleToExploring,
    STBossToExploring,
    StExploringToBattle,
    SFXBasicShot,
    SFXBasicBounce,
    SFXShotgunShot,
    SFXGunReload,
    SFXLasergunShot,
    SFXRifleShot,
    SFXEnemyShot,
    SFXPlayerInteract,
    SFXPlayerDeath,
    SFXPlayerDamaged,
    SFXEnemyDeath,
    SFXBulletShot,
    SFXLaserShot,
    SFXPauseMenuOpen,
    SFXPauseMenuClose,
    SFXButtonPress,
    SFXCoin,
    SFXRoomClear,
    SFXChestOpen,
    SFXPurchaseItem,
    SFXFailPurchase,
    SFXStartGame
}
