using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Point from which sphere cast starts in order to detect enemies")]
    [SerializeField] private Transform launchAttackPoint;
    [Tooltip("Point for the camera to focus on")]
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private PlayerSettings settings;

    [SerializeField] private PlayerCharacter playerCharacter;

    private Vector2 moveInput;

    public PlayerCharacter PlayerCharacter
    {
        get => playerCharacter;
    }

    private void Update()
    {
        playerCharacter.Move(GetRelativeMovement());

        CheckNearbyEnemies();
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
    private void CheckNearbyEnemies()
    {
        List<Transform> enemies = new List<Transform>();
        foreach (Collider coll in Physics.OverlapSphere(launchAttackPoint.position, settings.ExternLaunchAttackDetectRadius))
        {
            if (coll.GetComponentInParent<IAttackable>() != null)
            {

                if (Vector3.Distance(coll.transform.position, launchAttackPoint.position) <= settings.launchAttackDetectRadius)
                    enemies.Add(coll.transform);
                else
                    coll.GetComponentInParent<ITargetable>()?.SetTargettedState(false);
            }
        }

        if (enemies.Count > 0)
        {
            Transform newTarget = GetClosest(enemies);

            newTarget.gameObject.GetComponentInParent<ITargetable>()?.SetTargettedState(true);

            playerCharacter.SetAttackTarget(newTarget);
        }
        else
            playerCharacter.SetAttackTarget(null);
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
        CheckNearbyEnemies();

        playerCharacter.LaunchAttack();
    }

    private void OnHook(InputValue inputValue)
    {
        playerCharacter.TryHook();
    }

    /// <summary>
    /// Gets closest transform from a list
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
            Gizmos.DrawWireSphere(launchAttackPoint.position, settings.launchAttackDetectRadius);
    }
}