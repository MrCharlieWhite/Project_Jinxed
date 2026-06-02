using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    BoxCollider2D levelFinish;
    
    public int _currentLevel;
    private int _lastLoadFrame = int.MinValue;
    
    private bool LoadedRecently => Time.frameCount <= _lastLoadFrame + 1;


    public void Start()
    {
        levelFinish = GetComponent<BoxCollider2D>();

    }

    public void LoadNextScene()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        LoadScene(currentLevel + 1);
        currentLevel = currentLevel + 1;
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
    

    void LoadWhenLevelOver()
    {
        if (levelFinish.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            LoadNextScene();
        }
    }



    void Update()
    {
        LoadWhenLevelOver();
    }
    
}