using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource soundeffectSource;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
}
