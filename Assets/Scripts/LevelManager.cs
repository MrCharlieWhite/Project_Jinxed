using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    public int _currentLevel = 0;
    private int _lastLoadFrame = int.MinValue;

    private bool LoadedRecently => Time.frameCount <= _lastLoadFrame + 1;
    

    public void LoadNextScene()
    {
        LoadScene(_currentLevel + 1);
    }

    public void LoadPreviousScene()
    {
        LoadScene(_currentLevel - 1);
    }

    public void ReloadLevel()
    {
        LoadScene(_currentLevel);
    }

    private void LoadScene(int levelIndex)
    {
        if (LoadedRecently) return; 
        
        _lastLoadFrame = Time.frameCount;
        SceneManager.LoadScene(levelIndex);
        _currentLevel = levelIndex;
    }

    public void LoadFirstLevel()
    {
        LoadScene(1);
    }

    public void LoadMainMenu()
    {
        LoadScene(0);
    }


    public void PlayMenuMusic()
    {
        if (_currentLevel == 0)
        {
            
        }
    }
}