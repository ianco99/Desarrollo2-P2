using kuznickiAttackables;
using kuznickiEventChannel;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HealthController), typeof(Rigidbody))]
public class CrabEnemy : MonoBehaviour
{
    [SerializeField] private float timeToMove;
    [SerializeField] private float timeToRotateWeapons;
    [SerializeField] AnimationCurve speedCurve;
    [SerializeField] AnimationCurve rotateSpeedCurve;
    [SerializeField] private float moveSpeedMultiplier;
    [SerializeField] private float rotateSpeedMultiplier;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private PlayerControllerEventChannel respawnChannel;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private Rigidbody rb;
    private HealthController healthController;

    private bool movingRight = true;
    private float currentMoveTime = 0;

    private bool rotatingUp = true;
    private float currentWeaponRotateTime = 0;

    private float defaultHealth = 1.0f;

    private float upDeathMultiplier = 5.0f;
    private float timeToDie = 3.0f;

    private void Start()
    {
        if (!TryGetComponent(out healthController) || !TryGetComponent(out rb))
        {
            Debug.LogError(gameObject.name + " needs a " + nameof(HealthController) + " and a Rigidbody to work properly!");
            gameObject.SetActive(false);
        }

        respawnChannel?.Subscribe(Respawn);

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void OnDestroy()
    {
        respawnChannel?.Unsubscribe(Respawn);
    }

    private void Update()
    {
        BodyMovement();
        WeaponMovement();
    }

    /// <summary>
    /// Called when player is respawned
    /// </summary>
    /// <param name="controller"></param>
    private void Respawn(PlayerController controller)
    {
        StopAllCoroutines();

        healthController.SetHealth(defaultHealth);

        rb.velocity = Vector3.zero;
        transform.SetPositionAndRotation(startPosition, startRotation);

        for (int i = 0; i < weaponManager.GetWeaponParents().Count; i++)
        {
            weaponManager.GetWeaponParents()[i].gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            healthController.RecieveDamage(defaultHealth);
        }
    }

    /// <summary>
    /// Called on update. Moves entity
    /// </summary>
    private void BodyMovement()
    {
        float normTime = currentMoveTime / timeToMove;
        float moveSpeed = speedCurve.Evaluate(normTime);

        if (movingRight)
        {
            transform.Translate(transform.right * moveSpeed * moveSpeedMultiplier * Time.deltaTime, Space.Self);
        }
        else
        {
            transform.Translate(-transform.right * moveSpeed * moveSpeedMultiplier * Time.deltaTime, Space.Self);
        }

        currentMoveTime += Time.deltaTime;

        if (currentMoveTime > timeToMove)
        {
            movingRight = !movingRight;
            currentMoveTime = 0;
        }
    }

    /// <summary>
    /// Called on update. Moves weapons
    /// </summary>
    private void WeaponMovement()
    {
        float normTime = currentWeaponRotateTime / timeToRotateWeapons;
        float rotateSpeed = rotateSpeedCurve.Evaluate(normTime);

        for (int i = 0; i < weaponManager.GetWeaponParents().Count; i++)
        {
            Vector3 newRotation = Vector3.zero;

            if (rotatingUp)
            {
                newRotation -= new Vector3(rotateSpeed, 0, 0) * rotateSpeedMultiplier * Time.deltaTime;
                weaponManager.GetWeaponParents()[i].Rotate(newRotation, Space.Self);
            }
            else
            {
                newRotation += new Vector3(rotateSpeed, 0, 0) * rotateSpeedMultiplier * Time.deltaTime;
                weaponManager.GetWeaponParents()[i].Rotate(newRotation, Space.Self);
            }
        }

        currentWeaponRotateTime += Time.deltaTime;

        if (currentWeaponRotateTime > timeToRotateWeapons)
        {
            rotatingUp = !rotatingUp;
            currentWeaponRotateTime = 0;
        }
    }

    /// <summary>
    /// Called on entities death
    /// </summary>
    public void Die()
    {
        rb.AddForce(Vector3.up * upDeathMultiplier, ForceMode.Impulse);

        for (int i = 0; i < weaponManager.GetWeaponParents().Count; i++)
        {
            weaponManager.GetWeaponParents()[i].gameObject.SetActive(false);
        }

        StartCoroutine(DeathCoroutine());
    }

    /// <summary>
    /// Death sequence
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(timeToDie);
        gameObject.SetActive(false);
    }
}