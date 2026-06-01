using UnityEngine;

public class AudioManager_Levels : MonoBehaviour
{
 
    [Header("----------Audio Source----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("----------Audio Clip----------")]
    public AudioClip levelMusic;
    
    public AudioClip jump;
    public AudioClip death;
    public AudioClip teleport;
    public AudioClip fireball;

    public void Start()
    {
        musicSource.clip = levelMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
    
    public void PlayJump()
    {
        SFXSource.PlayOneShot(jump);
    }
    
    public void PlayDeath()
    {
        SFXSource.PlayOneShot(death);
    }
    

}
