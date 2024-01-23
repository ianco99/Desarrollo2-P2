using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettingsData", menuName = "ScriptableObjects/PlayerSettings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    [Header("MovementSettings")]
    public float speed = 10.0f;
    public float coyoteTargetTime;
    public float jumpForce = 10.0f;
    public float maxHorVelocity = 5.0f;
    public float maxVertVelocity = 5.0f;
    public float fallingMultiplier = 5.0f;
    public float lowJumpMultiplier = 5.0f;
    public float targetLowJumpTimer = 1.0f;

    [Header("DetectionSettings")]
    public float insideDetectionRadius;
    public float OutsideDetectionRadius;

    [Header("AttackSettings")]
    public float launchAttackForce = 200.0f;

}
