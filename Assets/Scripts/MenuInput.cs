using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

enum MenuScreen { MainMenu, Options, Credits, PauseMenu}
public class MenuInput : MonoBehaviour
{
    [SerializeField] Button[] mainMenuButtons;
    [SerializeField] Button[] optionsMenuButtons;
    [SerializeField] Button[] pauseMenuButtons;
    [SerializeField] Button backButtonCredits;
    [SerializeField] int index = 0;

    private MenuScreen currentScreen;

    private bool isInCredits = false;
    private bool isInOptions = false;
    private bool isInPause = false;
    private bool usingGamepad = false;

    private void OnSelection(InputValue value)
    {
        switch (currentScreen)
        {
            case MenuScreen.MainMenu:
                if (value.Get<float>() < 0)
                {
                    if (index > 0)
                    {
                        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                        index--;
                        mainMenuButtons[index].Select();
                    }
                }
                else if (index + value.Get<float>() < mainMenuButtons.Length)
                {
                    index++;
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                    mainMenuButtons[index].Select();
                }
                break;
            case MenuScreen.Options:
                if (value.Get<float>() < 0)
                {
                    if (index > 0)
                    {

                        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                        index--;
                        optionsMenuButtons[index].Select();
                    }
                }
                else if (index + value.Get<float>() < optionsMenuButtons.Length)
                {
                    index++;
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                    optionsMenuButtons[index].Select();
                }
                break;
            case MenuScreen.PauseMenu:
                if (value.Get<float>() < 0)
                {
                    if (index > 0)
                    {

                        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                        index--;
                        pauseMenuButtons[index].Select();
                    }
                }
                else if (index + value.Get<float>() < pauseMenuButtons.Length)
                {
                    index++;
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                    pauseMenuButtons[index].Select();
                }
                break;
            case MenuScreen.Credits:
                backButtonCredits.Select();
                break;
            default:
                break;
        }
    }

    public void ToggleCredits()
    {
        index = 0;
        isInCredits = !isInCredits;

        if (isInCredits)
            currentScreen = MenuScreen.Credits;
        else
            currentScreen = MenuScreen.MainMenu;
    }
    public void ToggleOptions()
    {
        index = 0;
        isInOptions = !isInOptions;

        if (isInOptions)
            currentScreen = MenuScreen.Options;
        else
            currentScreen = MenuScreen.MainMenu;
    }

    public void TogglePause()
    {
        index = 0;
        isInPause = !isInPause;

        if (isInPause)
            currentScreen = MenuScreen.PauseMenu;
        else
            currentScreen = MenuScreen.MainMenu;
    }

    public void ToggleControlScheme(GameObject go)
    {
        usingGamepad = !usingGamepad;

        if (usingGamepad)
        {
            PlayerPrefs.SetString("ControlScheme", "Gamepad");
            go.GetComponentInChildren<TextMeshProUGUI>().text = "Gamepad";
        }
        else
        {
            PlayerPrefs.SetString("ControlScheme", "Keyboard");
            go.GetComponentInChildren<TextMeshProUGUI>().text = "Keyboard";
        }
    }
}
