using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTrigger : MonoBehaviour
{
    [SerializeField] private PlayerTriggerDetector playerTrigger;
    [SerializeField] private float speedBoost;

    private void Awake()
    {
        playerTrigger.OnPlayerTrigger += GivePlayerBoost;
    }

    /// <summary>
    /// Adds currentSpeed boost to player when entering its trigger
    /// </summary>
    /// <param name="controller"></param>
    private void GivePlayerBoost(PlayerController controller)
    {
        controller.PlayerCharacter.AddSpeed(transform.forward,speedBoost);
    }
}
