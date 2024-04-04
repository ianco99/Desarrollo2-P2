using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BouncePlatform : MonoBehaviour
{
    [SerializeField] private float bounceForce = 100;
    [SerializeField] private PlayerTriggerDetector detector;
    private void Awake()
    {
        detector.OnPlayerTrigger += PlayerDetectedHandler;
    }

    private void PlayerDetectedHandler(PlayerCharacter character)
    {
        character.AddForce(transform.up, bounceForce, ForceMode.VelocityChange);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position, transform.up * bounceForce);
    }
}
