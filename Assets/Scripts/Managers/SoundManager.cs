using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource soundeffectSource;


    [Header("Audio Clips")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameplayMusic;
    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip gameOverMusic;


    [SerializeField] private AudioClip playerDamagedSoundEffect;
    [SerializeField] private AudioClip playerHealSoundEffect;


    [Header("Intro Musics")]
    [SerializeField] private AudioClip introTVDramaMusic;
    [SerializeField] private AudioClip introGraveyardMusic;
    [SerializeField] private AudioClip introTVLossMusic;
    [SerializeField] private AudioClip introFinaleMusic;
    [SerializeField] private AudioClip explosionSoundEffect;
    [SerializeField] private AudioClip zombieGrowlSoundEffect;


    [Header("Test Sounds")]
    [SerializeField] private AudioClip testSoundEffect;
    protected override void Awake()
    {
        base.Awake();
    }


    public void PlaySoundEffect(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("SoundManager: Attempted to play a null sound effect.");
            return;
        }
        soundeffectSource.PlayOneShot(clip);
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        if (backgroundMusicSource.clip == clip) return;
        backgroundMusicSource.clip = clip;
        backgroundMusicSource.Play();
    }


    public void StopBackgroundMusic()
    {
        backgroundMusicSource.Stop();
        backgroundMusicSource.clip = null;
    }

    public void SetBackgroundMusicVolume(float volume)
    {
        backgroundMusicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSoundEffectVolume(float volume)
    {
        soundeffectSource.volume = Mathf.Clamp01(volume);
        PlaySoundEffect(testSoundEffect);
    }

    public float GetBackgroundMusicVolume()
    {
        return backgroundMusicSource.volume;
    }
    public float GetSoundEffectVolume()
    {
        return soundeffectSource.volume;
    }

    public void MuteBackgroundMusic(bool mute)
    {
        backgroundMusicSource.mute = mute;
    }
    public void MuteSoundEffects(bool mute)
    {
        soundeffectSource.mute = mute;
    }

    public void PauseBackgroundMusic()
    {
        backgroundMusicSource.Pause();
    }
    public void UnPauseBackgroundMusic()
    {
        backgroundMusicSource.UnPause();
    }

    public void StopAllSounds()
    {
        backgroundMusicSource.Stop();
        soundeffectSource.Stop();
    }

    public void PlayMainMenuMusic()
    {
        PlayBackgroundMusic(mainMenuMusic);
    }

    public void PlayGameplayMusic()
    {
        PlayBackgroundMusic(gameplayMusic);
    }

    public void PlayWinMusic()
    {
        PlayBackgroundMusic(winMusic);
    }

    public void PlayGameOverMusic()
    {
        PlayBackgroundMusic(gameOverMusic);
    }

    public void PlayPlayerDamagedSoundEffect()
    {
        PlaySoundEffect(playerDamagedSoundEffect);
    }

    public void PlayPlayerHealSoundEffect()
    {
        PlaySoundEffect(playerHealSoundEffect);
    }

    public void PlayIntroMusic(int index)
    {
        switch (index)
        {
            case 0:
                StopAllSounds();
                break;
            case 1:
                PlayBackgroundMusic(introTVDramaMusic);
                break;
            case 2:
                PlayBackgroundMusic(introGraveyardMusic);
                break;
            case 3:
                PlayBackgroundMusic(introTVLossMusic);
                break;
            case 4:
                PlayBackgroundMusic(introFinaleMusic);
                break;
            case 5:
                PlaySoundEffect(explosionSoundEffect);
                break;
            case 6:
                PlaySoundEffect(zombieGrowlSoundEffect);
                break;
            default:
                Debug.LogWarning("SoundManager: Invalid intro music index: " + index);
                break;
        }
    }
    
}
