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

    private void OnNextLevelCheat()
    {
        if(SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex+1)
        GameManager.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex+1);
    }

    private void OnGodModeCheat()
    {
        godMode = !godMode;
        godModeChannel?.RaiseEvent(godMode);
    }

    private void OnFlashCheat()
    {
        flashMode = !flashMode;
        flashModeChannel?.RaiseEvent(flashMode);
    }

    private void OnFeatherFallCheat()
    {
        featherFall = !featherFall;
        featherFallChannel?.RaiseEvent(featherFall);
    }
}
