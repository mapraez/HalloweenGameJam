using UnityEngine;

public class IntroController : MonoBehaviour
{

    static int introStopMusicIndex = 0;
    static int introTVDramaIndex = 1;
    static int introGraveyardIndex = 2;
    static int introTVLossIndex = 3;
    static int introFinaleIndex = 4;
    static int explosionSoundIndex = 5;
    static int zombieGrowlIndex = 6;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayIntroDramaMusic()
    {
        SoundManager.Instance.PlayIntroMusic(introTVDramaIndex);
    }

    public void PlayIntroGraveyardMusic()
    {
        SoundManager.Instance.PlayIntroMusic(introGraveyardIndex);
    }
    public void PlayIntroTVLossMusic()
    {
        SoundManager.Instance.PlayIntroMusic(introTVLossIndex);
    }
    public void PlayIntroFinaleMusic()
    {
        SoundManager.Instance.PlayIntroMusic(introFinaleIndex);
    }

    public void PlayExplosionSound()
    {
        SoundManager.Instance.PlayIntroMusic(explosionSoundIndex);
    }

    public void StopMusicSignal()
    {
        SoundManager.Instance.PlayIntroMusic(introStopMusicIndex);
    }

    public void PlayZombieGrowlSound()
    {
        SoundManager.Instance.PlayIntroMusic(zombieGrowlIndex);
    }

    public void EndIntroScene()
    {
        GameManager.Instance.StartGame();
    }
}
