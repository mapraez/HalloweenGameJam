using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource soundeffectSource;
   
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
}
