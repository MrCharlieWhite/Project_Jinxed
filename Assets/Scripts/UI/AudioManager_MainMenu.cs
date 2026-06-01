using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    
    [Header("----------Audio Source----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("----------Audio Clip----------")]
    public AudioClip backgroundMenu;
    
    public AudioClip startButton;
    public AudioClip quitButton;
    public AudioClip optionsButton;
    public AudioClip quitOptions;

    public void Start()
    {
        musicSource.clip = backgroundMenu;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
    
    public void PlayStartButton()
    {
        SFXSource.PlayOneShot(startButton);
    }
    
    public void PlayQuitButton()
    {
        SFXSource.PlayOneShot(quitButton);
    }
    
    public void PlayOptionsButton()
    {
        SFXSource.PlayOneShot(optionsButton);
    }
    
    public void PlayQuitOptions()
    {
        SFXSource.PlayOneShot(quitOptions);
    }
}
