using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using kuznickiAttackables;
using kuznickiEventChannel;
/// <summary>
/// Player class in charge of processing input and controlling player's behaviour
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Tooltip("Point from which sphere cast starts in order to detect enemies")]
    [SerializeField] public Transform detectionPoint;
    [Tooltip("Point for the camera to focus on")]
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private PlayerSettings settings;
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private HealthController healthController;
    [SerializeField] private BoolEventChannel godModeChannel;
    [SerializeField] private BoolEventChannel flashModeChannel;
    [SerializeField] private BoolEventChannel featherFallChannel;

    private Vector2 moveInput;

    private List<Transform> currentTargets;
    private IAttackable currentAttackTarget;
    private IHookable currentHookTarget;

    private bool hooking = false;
    private bool godMode;

    public Action<IAttackable> OnStartAttack;
    public Action<IHookable> OnStartHook;

    public PlayerCharacter PlayerCharacter
    {
        get => playerCharacter;
    }

    private void Awake()
    {
        godModeChannel.Subscribe(GodModeToggle);
        flashModeChannel.Subscribe(FlashModeToggle);
        featherFallChannel.Subscribe(FeatherFallToggle);
    }

    private void OnDestroy()
    {
        godModeChannel.Unsubscribe(GodModeToggle);
    }

    private void GodModeToggle(bool value)
    {
        healthController.SetGodMode(value);
    }

    private void FlashModeToggle(bool value)
    {
        if (value)
            settings.currentSpeed = settings.FlashSpeed;
        else
            settings.currentSpeed = settings.defaultSpeed;
    }

    private void FeatherFallToggle(bool value)
    {
        if (value)
            settings.currentFallingMultiplier = settings.FeatherFallMultiplier;
        else
            settings.currentFallingMultiplier = settings.defaultFallMultiplier;
    }

    private void Update()
    {
        playerCharacter.Move(GetRelativeMovement());

        CheckNearbyTargets();

        UpdateAttackTarget();

        if (!hooking)
            UpdateHookTarget();

        if (hooking && currentHookTarget != null)
        {
            playerCharacter.DrawHook(currentHookTarget.GetTransform().position);
        }
    }

    /// <summary>
    /// Gets movement relative to camera position
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRelativeMovement()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        cameraForward = cameraForward.normalized;

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f;
        cameraRight = cameraRight.normalized;

        return (cameraForward * moveInput.y + cameraRight * moveInput.x) * Time.deltaTime;
    }

    /// <summary>
    /// Casts sphere to detect nearby enemies and target them
    /// </summary>
    private void CheckNearbyTargets()
    {
        List<Transform> objects = new List<Transform>();
        foreach (Collider coll in Physics.OverlapSphere(detectionPoint.position, settings.OutsideDetectionRadius))
        {
            if (coll.GetComponentInParent<ITargetable>() != null)
            {

                if (Vector3.Distance(coll.transform.position, detectionPoint.position) <= settings.insideDetectionRadius)
                {
                    objects.Add(coll.transform);
                    coll.GetComponentInParent<ITargetable>()?.SetTargettedState(true);
                }
                else
                    coll.GetComponentInParent<ITargetable>()?.SetTargettedState(false);
            }
        }
        currentTargets = objects;
    }

    /// <summary>
    /// Update attack target in player character
    /// </summary>
    private void UpdateAttackTarget()
    {
        if (currentTargets.Count > 0)
        {
            List<Transform> attackables = new List<Transform>();

            for (int i = 0; i < currentTargets.Count; i++)
            {
                if (currentTargets[i].TryGetComponent<IAttackable>(out _))
                    attackables.Add(currentTargets[i]);
            }

            if (attackables.Count > 0)
            {
                Transform newTarget = GetClosest(attackables);

                currentAttackTarget = newTarget.GetComponent<IAttackable>();
            }
            else
                currentAttackTarget = null;
        }
        else
            currentAttackTarget = null;
    }

    /// <summary>
    /// Update hookable target on player character
    /// </summary>
    private void UpdateHookTarget()
    {

        if (currentTargets.Count > 0)
        {
            List<Transform> hookables = new List<Transform>();

            for (int i = 0; i < currentTargets.Count; i++)
            {
                if (currentTargets[i].TryGetComponent<IHookable>(out _))
                    hookables.Add(currentTargets[i]);
            }

            if (hookables.Count > 0)
            {
                Transform newTarget = GetClosest(hookables);

                currentHookTarget = newTarget.GetComponent<IHookable>();
            }
            else
                currentHookTarget = null;
        }
        else
            currentHookTarget = null;
    }

    /// <summary>
    /// On move input
    /// </summary>
    /// <param name="value"></param>
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    /// <summary>
    /// On jump input
    /// </summary>
    /// <param name="value"></param>
    private void OnJump(InputValue value)
    {
        playerCharacter.Jump();
    }

    /// <summary>
    /// On attack input
    /// </summary>
    /// <param name="inputValue"></param>
    private void OnAttack(InputValue inputValue)
    {
        if (currentAttackTarget != null)
        {
            OnStartAttack?.Invoke(currentAttackTarget);
        }
    }

    /// <summary>
    /// On hook input
    /// </summary>
    /// <param name="inputValue"></param>
    private void OnHook(InputValue inputValue)
    {
        bool value = inputValue.isPressed;

        if (hooking != value)
        {
            if (hooking)
            {
                playerCharacter.StopHook();
                hooking = value;
            }
            else
            {

                if (currentHookTarget != null)
                {
                    OnStartHook?.Invoke(currentHookTarget);
                    hooking = value;
                }
            }
        }

    }

    /// <summary>
    /// Gets closest model from a list
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    private Transform GetClosest(List<Transform> points)
    {
        Transform closest = points[0];

        for (int i = 0; i < points.Count; i++)
        {
            if (Vector3.Distance(transform.position, points[i].position) < Vector3.Distance(transform.position, closest.position))
            {
                closest = points[i];
            }
        }

        return closest;
    }


    public void OnRecievedDamageHandler()
    {
        playerCharacter.DamagedReaction();
    }

    public void OnDeathHandler()
    {
        playerCharacter.Die();
    }

    private void OnCollisionEnter(Collision other)
    {
        IAttackable attackable = other.gameObject.GetComponentInParent<IAttackable>();
        if (attackable != null)
        {
            playerCharacter.CheckRebound(other);

        }

        else
        {
            playerCharacter.CheckRebound(null);
        }
    }

    private void OnDrawGizmos()
    {
        if (settings)
            Gizmos.DrawWireSphere(detectionPoint.position, settings.insideDetectionRadius);
    }
}