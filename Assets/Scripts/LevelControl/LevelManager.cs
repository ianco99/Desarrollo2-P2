using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using kuznickiEventChannel;
using static UnityEditor.Progress;


public class LevelManager : MonoBehaviour
{
    [Tooltip("Event channel to hear when player reaches end goal")]
    [SerializeField] private PlayerControllerEventChannel endGoalChannel;
    [Tooltip("Actions done on ending level")]
    [SerializeField] private UnityEvent OnEndLevel;
    [SerializeField] private PlayerControllerEventChannel playerRespawnChannel;
    [SerializeField] private Transform startingPoint;
    [Tooltip("All game objects that need re-enabling when restarting level")]
    [SerializeField] private GameObject[] respawneableObjects;
    [Tooltip("GameObject containing the pause menu")]
    [SerializeField] private GameObject pauseMenu;

    private bool isPaused = false;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        endGoalChannel.Subscribe(OnEndLevelHandler);
        playerRespawnChannel.Subscribe(RespawnPlayer);
    }

    private void OnDestroy()
    {
        endGoalChannel.Unsubscribe(OnEndLevelHandler);
        playerRespawnChannel.Unsubscribe(RespawnPlayer);
    }

    /// <summary>
    /// Respawns player in level
    /// </summary>
    public void RespawnPlayer(PlayerController playerController)
    {
        playerController.transform.position = startingPoint.position;
        playerController.PlayerCharacter.Respawn();


        for (int i = 0; i < respawneableObjects.Length; i++)
        {
            respawneableObjects[i].SetActive(true);
            respawneableObjects[i].GetComponent<ToggleOutline>()?.SetOutlines(false);
        }
    }

    /// <summary>
    /// On pause input recieved
    /// </summary>
    public void OnPause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    /// <summary>
    /// Loads main menu scene
    /// </summary>
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        GameManager.Instance.LoadMenu();
    }

    /// <summary>
    /// Loads next level scene
    /// </summary>
    public void LoadNextLevel()
    {
        GameManager.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex+1);
    }

    /// <summary>
    /// Called on reached end level trigger
    /// </summary>
    /// <param name="controller"></param>
    private void OnEndLevelHandler(PlayerController controller)
    {
        OnEndLevel.Invoke();
        Cursor.lockState = CursorLockMode.None;
    }
}