using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private PlayerTriggerDetector endGoalTrigger;
    [SerializeField] private UnityEvent OnEndLevel;
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform startingPoint;
    [SerializeField] private Cinemachine.CinemachineFreeLook camera;
    [SerializeField] private GameObject[] respawneableObjects;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private MenuInput menuInput;

    private bool isPaused = false;

    private void Awake()
    {
        instance = this;
        Cursor.lockState = CursorLockMode.Locked;
        endGoalTrigger.OnPlayerTrigger += OnEndLevelHandler;

        SetCameraValues();
    }

    private void SetCameraValues()
    {
        string controlScheme = PlayerPrefs.GetString("ControlScheme", " ");

        if (controlScheme == null)
            return;

        if (controlScheme == "Keyboard")
        {
            camera.m_YAxis.m_MaxSpeed = 0.01f;
            camera.m_XAxis.m_MaxSpeed = 0.2f;
        }
        else
        {
            camera.m_YAxis.m_MaxSpeed = 0.1f;
            camera.m_XAxis.m_MaxSpeed = 3f;
        }
    }

    public void RespawnPlayer()
    {
        player.transform.position = startingPoint.position;
        player.GetPlayerModel().GetRigidbody().rotation = Quaternion.Euler(Vector3.zero);
        player.GetPlayerModel().GetRigidbody().velocity = Vector3.zero;

        foreach (var item in respawneableObjects)
        {
            item.SetActive(true);
            item.GetComponent<Outline>().enabled = false;
        }
    }

    public void OnPause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            menuInput.TogglePause();
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            menuInput.TogglePause();
        }
    }


    public void LoadNextLevel()
    {
        GameManager.Instance.LoadGameLevel(true);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        GameManager.Instance.LoadMenu();
    }
    private void OnEndLevelHandler(PlayerController controller)
    {
        OnEndLevel.Invoke();
        Cursor.lockState = CursorLockMode.None;
    }
}
