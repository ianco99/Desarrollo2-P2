using kuznickiAttackables;
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
    [SerializeField] private Transform[] weaponsTransforms;

    private Rigidbody rb;
    private HealthController healthController;

    private bool movingRight = true;
    private float currentMoveTime = 0;

    private bool rotatingUp = true;
    private float currentWeaponRotateTime = 0;

    private void Start()
    {
        if (!TryGetComponent(out healthController) || !TryGetComponent(out rb))
        {
            Debug.LogError(gameObject.name + " needs a " + nameof(HealthController) + " and a Rigidbody to work properly!");
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        BodyMovement();
        WeaponMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            healthController.RecieveDamage(1);
        }
    }

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

    private void WeaponMovement()
    {
        float normTime = currentWeaponRotateTime / timeToRotateWeapons;
        float rotateSpeed = rotateSpeedCurve.Evaluate(normTime);

        for (int i = 0; i < weaponsTransforms.Length; i++)
        {
            Vector3 newRotation = Vector3.zero;

            if (rotatingUp)
            {
                newRotation -= new Vector3(rotateSpeed,0, 0 ) * rotateSpeedMultiplier * Time.deltaTime;
                //transform.localEulerAngles = newRotation;
                weaponsTransforms[i].Rotate(newRotation, Space.Self);
            }
            else
            {
                newRotation += new Vector3(rotateSpeed, 0, 0) * rotateSpeedMultiplier * Time.deltaTime;
                //transform.localEulerAngles = newRotation;
                weaponsTransforms[i].Rotate(newRotation, Space.Self);
            }
        }



        currentWeaponRotateTime += Time.deltaTime;

        if (currentWeaponRotateTime > timeToRotateWeapons)
        {
            rotatingUp = !rotatingUp;
            currentWeaponRotateTime = 0;
        }
    }

    public void Die()
    {
        rb.AddForce(Vector3.up * 5.0f, ForceMode.Impulse);

        for (int i = 0; i < weaponsTransforms.Length; i++)
        {
            weaponsTransforms[i].gameObject.SetActive(false);
        }
        
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
