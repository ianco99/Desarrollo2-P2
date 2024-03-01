using UnityEngine;
using kuznickiAttackables;

public class PlayerCharacter : MonoBehaviour, ICharacter
{
    [SerializeField] private PlayerSettings playerSettings;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LineRenderer lineRenderer;

    private PlayerController controller;

    private SpringJoint hookJoint;

    private bool characterGrounded;
    private bool characterJumping;
    private bool characterAttacking;

    private int jumpCount = 1;

    private float currentTimeJumping;
    private float coyoteCurrentTime;

    private void Awake()
    {
        if (TryGetComponent(out controller))
        {
            controller.OnStartAttack += LaunchAttack;
            controller.OnStartHook += StartHook;
        }
    }

    private void OnDestroy()
    {
        if (controller)
        {
            controller.OnStartAttack -= LaunchAttack;
            controller.OnStartHook -= StartHook;
        }
    }

    private void Update()
    {
        CheckGrounded();
    }

    /// <summary>
    /// Checks if player is grounded
    /// </summary>
    private void CheckGrounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 0.8f))
        {
            characterGrounded = true;

            if (currentTimeJumping > playerSettings.targetLowJumpTimer)
            {
                characterJumping = false;
                currentTimeJumping = 0;
                jumpCount = 1;
            }

            coyoteCurrentTime = 0;
        }
        else
        {
            characterGrounded = false;
        }

        if (!characterGrounded)
            coyoteCurrentTime += Time.deltaTime;
    }

    /// <summary>
    /// Upwards force when jumping
    /// </summary>
    public void Jump()
    {
        if (coyoteCurrentTime <= playerSettings.coyoteTargetTime)
        {
            if (jumpCount > 0)
            {
                jumpCount--;

                characterJumping = true;
                rb.AddForce(Vector3.up * playerSettings.jumpForce, ForceMode.VelocityChange);

            }
        }

    }

    /// <summary>
    /// Controls movement via rigidbody and a given direction
    /// </summary>
    /// <param name="relativeMovement"></param>
    public void Move(Vector3 relativeMovement)
    {
        if (Mathf.Abs(rb.velocity.z) > playerSettings.maxHorVelocity)
        {
            relativeMovement = Vector3.zero;
        }

        if (Mathf.Abs(rb.velocity.x) > playerSettings.maxHorVelocity)
        {
            relativeMovement = Vector3.zero;
        }

        if (Mathf.Abs(rb.velocity.y) > playerSettings.maxVertVelocity)
        {
            relativeMovement = Vector3.zero;
        }

        if (rb.useGravity)
        {
            rb.AddForce(new Vector3(relativeMovement.x * playerSettings.speed, 0.0f, relativeMovement.z * playerSettings.speed), ForceMode.VelocityChange);

            if (rb.velocity.y < 0.0f)
            {
                rb.velocity += Vector3.up * playerSettings.fallingMultiplier * Physics.gravity.y * Time.deltaTime;
            }
            else if (rb.velocity.y > 0f && !characterGrounded)
                rb.velocity += Vector3.up * Physics.gravity.y * (playerSettings.lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (characterJumping)
            currentTimeJumping += Time.deltaTime;
    }

    /// <summary>
    /// Launches player towards attackTarget
    /// </summary>
    /// <param name="attackTarget"></param>
    public void LaunchAttack(IAttackable target)
    {
        if (characterAttacking)
            return;

        rb.velocity = Vector3.zero;
        rb.useGravity = false;

        Vector3 destination = target.GetTransform().position - transform.position;
        destination = destination.normalized;
        rb.AddForce(destination * playerSettings.launchAttackForce, ForceMode.Impulse);
        rb.AddTorque(destination, ForceMode.Impulse);
    }

    /// <summary>
    /// Called on rebound from collision with enemy
    /// </summary>
    /// <param name="other"></param>
    public void CheckRebound(Collision other)
    {
        if (other == null)
        {
            if (!characterAttacking)
            {
                return;
            }
        }

        rb.useGravity = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (other != null)
        {
            other.gameObject.GetComponentInParent<ITargetable>()?.SetTargettedState(false);
            other.gameObject.GetComponentInParent<IAttackable>()?.ReceiveAttack();
        }

        rb.AddForce(Vector3.up * 20, ForceMode.Impulse);

        characterAttacking = false;
    }

    /// <summary>
    /// Adds joint component
    /// </summary>
    public void StartHook(IHookable target)
    {
        hookJoint = rb.gameObject.AddComponent<SpringJoint>();
        hookJoint.connectedBody = target.GetRigidbody();
        hookJoint.anchor = Vector3.zero;
        hookJoint.autoConfigureConnectedAnchor = false;
        hookJoint.spring = 3.0f;
        hookJoint.maxDistance = 2.0f;

        lineRenderer.enabled = true;
    }

    public void DrawHook(Vector3 endHookPos)
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endHookPos);
    }

    /// <summary>
    /// Deletes joint component
    /// </summary>
    public void StopHook()
    {
        rb.AddRelativeTorque(controller.detectionPoint.right * 99999.0f, ForceMode.VelocityChange);

        rb.AddForce(Vector3.up * 20, ForceMode.Impulse);

        Destroy(hookJoint);
        hookJoint = null;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);

        lineRenderer.enabled = false;
    }

    /// <summary>
    /// Speed adder for player rigidbody
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="speedBoost"></param>
    public void AddSpeed(Vector3 direction, float speedBoost)
    {
        rb.AddForce(direction * speedBoost, ForceMode.Impulse);
    }

    /// <summary>
    /// Toggles 'characterGrounded' to boolean 'value'
    /// </summary>
    /// <param name="value"></param>
    public void ToggleGrounded(bool value)
    {
        characterGrounded = value;
    }

    /// <summary>
    /// Toggles 'characterJumping' to boolean 'value'
    /// </summary>
    /// <param name="value"></param>
    public void ToggleJumping(bool value)
    {
        characterJumping = value;
    }

    /// <summary>
    /// Rigidbody getter
    /// </summary>
    /// <returns></returns>
    public Rigidbody GetRigidbody()
    {
        return rb;
    }
}
