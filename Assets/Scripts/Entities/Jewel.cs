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

    private void Respawn(PlayerController controller)
    {
        gameObject.SetActive(true);
    }
}
