using kuznickiEventChannel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatManager : MonoBehaviourSingleton<CheatManager>
{
    [SerializeField] private BoolEventChannel godModeChannel;
    [SerializeField] private bool godMode = false;
    private void OnNextLevelCheat()
    {
        GameManager.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex+1);
    }

    private void OnGodMode()
    {
        godMode = !godMode;
        godModeChannel.RaiseEvent(godMode);
    }
}
