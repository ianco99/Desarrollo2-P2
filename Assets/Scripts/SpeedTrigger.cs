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

    //TODO: Fix - It would be better to just have an OnTriggerEnter in this component, the event will be triggered anyways
    //I prefer using my player trigger class, since it automates the verification process - this can be discussed
    /// <summary>
    /// Adds currentSpeed boost to player when entering its trigger
    /// </summary>
    /// <param name="controller"></param>
    private void GivePlayerBoost(PlayerController controller)
    {
        controller.PlayerCharacter.AddSpeed(transform.forward,speedBoost);
    }
}
