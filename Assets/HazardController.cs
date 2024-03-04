using kuznickiEventChannel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardController : MonoBehaviour
{
    [SerializeField] private PlayerTriggerDetector hazard;
    [SerializeField] private PlayerControllerEventChannel respawnChannel;

    private void Start()
    {
        hazard.OnPlayerTrigger += InstaKillPlayer;
    }

    private void InstaKillPlayer(PlayerController playerController)
    {
        respawnChannel.RaiseEvent(playerController);
    }
}
