using kuznickiEventChannel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatManager : MonoBehaviourSingleton<CheatManager>
{
    [SerializeField] private BoolEventChannel godModeChannel;
    [SerializeField] private BoolEventChannel flashModeChannel;
    [SerializeField] private BoolEventChannel featherFallChannel;
    [SerializeField] private bool godMode = false;
    [SerializeField] private bool flashMode = false;
    [SerializeField] private bool featherFall = false;

    private void Awake()
    {
        godMode = false;
        flashMode = false;
        featherFall = false;

        godModeChannel?.RaiseEvent(godMode);
        flashModeChannel?.RaiseEvent(flashMode);
        featherFallChannel?.RaiseEvent(featherFall);
    }

    /// <summary>
    /// Loads next level on manual command from user (Developer)
    /// </summary>
    private void OnNextLevelCheat()
    {
        if(SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex+1)
        GameManager.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex+1);
    }

    /// <summary>
    /// Toggles god mode cheat for user
    /// </summary>
    private void OnGodModeCheat()
    {
        godMode = !godMode;
        godModeChannel?.RaiseEvent(godMode);
    }

    /// <summary>
    /// Toggles flash cheat which increases considerably player's speed
    /// </summary>
    private void OnFlashCheat()
    {
        flashMode = !flashMode;
        flashModeChannel?.RaiseEvent(flashMode);
    }

    /// <summary>
    /// Toggles feather fall cheat which decreases considerably player's gravity
    /// </summary>
    private void OnFeatherFallCheat()
    {
        featherFall = !featherFall;
        featherFallChannel?.RaiseEvent(featherFall);
    }
}
