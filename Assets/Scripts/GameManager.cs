using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [Tooltip("Scene index for the menu scene")]
    [SerializeField] private int MenuBuildIndex = 0;

    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    /// <summary>
    /// Loads scene by given index param
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// loads pre-set menu scene
    /// </summary>
    public void LoadMenu()
    {
        SceneManager.LoadScene(MenuBuildIndex);
    }
}
