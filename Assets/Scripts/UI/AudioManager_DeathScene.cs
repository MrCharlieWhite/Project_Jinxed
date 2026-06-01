using UnityEngine;

public class AudioManager_DeathScene : MonoBehaviour
{
    [Header("----------Audio Source----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    
    [Header("----------Audio Clip----------")]
    public AudioClip deathMenu;
    public AudioClip restart;
    public AudioClip returnToMenu;
    
    public void Start()
    {
        musicSource.clip = deathMenu;
        musicSource.Play();
    }

    void Restart()
    {
        SFXSource.PlayOneShot(restart);
    }

    void ReturnToMenu()
    {
        SFXSource.PlayOneShot(returnToMenu);
    }
}
