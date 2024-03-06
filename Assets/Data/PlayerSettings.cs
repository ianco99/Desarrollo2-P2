using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettingsData", menuName = "ScriptableObjects/PlayerSettings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    [Header("Default Settings")]
    public float defaultSpeed = 10.0f;
    public float defaultFallMultiplier = 2.5f;

    [Header("MovementSettings")]
    public float currentSpeed = 10.0f;
    public float coyoteTargetTime;
    public float jumpForce = 10.0f;
    public float maxHorVelocity = 5.0f;
    public float maxVertVelocity = 5.0f;
    public float currentFallingMultiplier = 2.5f;
    public float lowJumpMultiplier = 5.0f;
    public float targetLowJumpTimer = 1.0f;

    [Header("DetectionSettings")]
    public float insideDetectionRadius;
    public float OutsideDetectionRadius;

    [Header("AttackSettings")]
    public float launchAttackForce = 200.0f;

    [Header("Cheats")]
    public float FeatherFallMultiplier = 0.2f;
    public float FlashSpeed = 15.0f;
}
