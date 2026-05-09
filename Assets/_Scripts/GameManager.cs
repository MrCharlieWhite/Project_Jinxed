using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public LevelManager LevelManager { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(gameObject);
        GetSubManagers();
    }

    private void GetSubManagers()
    {
        LevelManager = GetComponentInChildren<LevelManager>();
    }
}
