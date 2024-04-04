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
    /// <param name="playerCharacter">Character to boost</param>
    private void GivePlayerBoost(PlayerCharacter playerCharacter)
    {
        playerCharacter.AddForce(transform.forward,speedBoost);
    }
}
