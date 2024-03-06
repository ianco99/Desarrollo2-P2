using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettingsData", menuName = "ScriptableObjects/PlayerSettings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    [Header("MovementSettings")]
    public float defaultSpeed = 10.0f;
    public float currentSpeed = 10.0f;
    public float coyoteTargetTime;
    public float jumpForce = 10.0f;
    public float maxHorVelocity = 5.0f;
    public float maxVertVelocity = 5.0f;
    public float currentFallingMultiplier = 5.0f;
    public float currentLowJumpMultiplier = 5.0f;
    public float targetLowJumpTimer = 1.0f;

    [Header("DetectionSettings")]
    public float insideDetectionRadius;
    public float OutsideDetectionRadius;

    [Header("AttackSettings")]
    public float launchAttackForce = 200.0f;

    [Header("Cheats")]
    public float FeatherFallMass = 0.2f;
    public float FlashSpeed = 15.0f;
}
