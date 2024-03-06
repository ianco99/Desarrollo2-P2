using kuznickiAttackables;
using kuznickiEventChannel;
using UnityEngine;
public class Jewel : BaseAttackable
{
    [SerializeField] private PlayerControllerEventChannel respawnChannel;

    private void Awake()
    {
        respawnChannel.Subscribe(Respawn);
    }

    private void OnDestroy()
    {
        respawnChannel.Unsubscribe(Respawn);
    }

    /// <summary>
    /// Called when respawning player
    /// </summary>
    /// <param name="controller"></param>
    private void Respawn(PlayerController controller)
    {
        gameObject.SetActive(true);
    }
}
