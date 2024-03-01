using kuznickiAttackables;
using UnityEngine;

[RequireComponent(typeof(HealthController), typeof(Rigidbody))]
public class CrabEnemy : MonoBehaviour
{
    [SerializeField] private float timeToMove;
    [SerializeField] private float speed;

    private Rigidbody rb;
    private HealthController healthController;
    private bool movingRight = true;
    private float currentMoveTime = 0;

    private void Start()
    {
        if (!TryGetComponent(out healthController) || !TryGetComponent(out rb))
        {
            Debug.LogError(gameObject.name + " needs a " + nameof(HealthController) + " and a Rigidbody " + " to work properly!");
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (movingRight)
        {
            transform.Translate(transform.right * speed * Time.deltaTime, Space.Self);
        }
        else
        {
            transform.Translate(-transform.right * speed * Time.deltaTime, Space.Self);
        }

        currentMoveTime += Time.deltaTime;

        if (currentMoveTime > timeToMove)
        {
            movingRight = !movingRight;
            currentMoveTime = 0;
        }
    }
}
