using UnityEngine;
using kuznickiAttackables;
using System.Collections;
using kuznickiEventChannel;

public class PlayerCharacter : MonoBehaviour, ICharacter
{
    [SerializeField] private PlayerSettings playerSettings;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Material material;
    [SerializeField] private HealthController healthController;
    [SerializeField] private PlayerControllerEventChannel respawnChannel;
    [SerializeField] private Animator anim;

    private PlayerController controller;

    private SpringJoint hookJoint;
    private bool characterGrounded;
    private bool characterJumping;
    private bool characterAttacking;

    private int jumpCount = 1;

    private float currentAccelerationTime = 0;
    private float currentTimeJumping;
    private float coyoteCurrentTime;
    private float groundedDistance = 0.8f;

    private float defaultHealth = 2.0f;
    private float timeToDie = 3.0f;

    private float reboundMultiplier = 20.0f;

    private float damagedImpulseMultiplier = 2.5f;

    private float hookSpring = 9.0f;
    private float hookMaxDistance = 2.0f;


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
            material.color = Color.cyan;
        }
    }

    private void Update()
    {
        CheckGrounded();
        CheckHealth();
    }

    /// <summary>
    /// Called when damaged
    /// </summary>
    public void DamagedReaction()
    {
        Vector3 newForce = -rb.velocity * damagedImpulseMultiplier;
        rb.AddForce(newForce, ForceMode.VelocityChange);
        rb.useGravity = true;
        material.color = Color.white;

        SoundManager.Instance.PlayAudioClip("PlayerDamaged");
    }

    /// <summary>
    /// Called on entity death
    /// </summary>
    public void Die()
    {
        rb.useGravity = false;
        material.color = Color.black;

        StartCoroutine(DeathSequence());
    }

    /// <summary>
    /// Death sequence
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(timeToDie);
        respawnChannel?.RaiseEvent(controller);
    }

    /// <summary>
    /// Player respawned in level
    /// </summary>
    public void Respawn()
    {
        rb.rotation = Quaternion.Euler(Vector3.zero);
        rb.velocity = Vector3.zero;
        rb.useGravity = true;
        material.color = Color.cyan;
        healthController.SetHealth(defaultHealth);
    }

    /// <summary>
    /// Checks if player is grounded
    /// </summary>
    private void CheckGrounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, groundedDistance))
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
    /// Change player material depending on current health
    /// </summary>
    private void CheckHealth()
    {
        if (healthController.Health == defaultHealth / 2.0f)
        {
            material.color = Color.white;
        }
        else if (healthController.Health <= 0)
        {
            material.color = Color.black;
        }
        else
        {
            material.color = Color.cyan;
        }
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

                anim.SetTrigger("Impulse");

                SoundManager.Instance.PlayAudioClip("PlayerJump");
            }
        }
    }

    /// <summary>
    /// Controls movement via rigidbody and a given direction
    /// </summary>
    /// <param name="relativeMovement"></param>
    public void Move(Vector3 relativeMovement)
    {
        if (characterGrounded)
            relativeMovement /= 2.0f;
        
        VelocityCap(relativeMovement);

        if (rb.useGravity)
        {
            HorizontalMovement(relativeMovement);

            //Vertical movement
            if (rb.velocity.y < 0.0f)
            {
                rb.velocity += Vector3.up * playerSettings.currentFallingMultiplier * Physics.gravity.y *
                               Time.deltaTime;
            }
            else if (rb.velocity.y > 0f && !characterGrounded)
                rb.velocity += Vector3.up * Physics.gravity.y * playerSettings.lowJumpMultiplier * Time.deltaTime;
        }

        if (characterJumping)
            currentTimeJumping += Time.deltaTime;
    }

    private void VelocityCap(Vector3 relativeMovement)
    {
        if (Mathf.Abs(rb.velocity.z) > playerSettings.maxHorVelocity)
        {
            relativeMovement.z = 0;
        }

        if (Mathf.Abs(rb.velocity.x) > playerSettings.maxHorVelocity)
        {
            relativeMovement.x = 0;
        }

        if (Mathf.Abs(rb.velocity.y) > playerSettings.maxVertVelocity)
        {
            relativeMovement.y = 0;
        }
    }

    private void HorizontalMovement(Vector3 relativeMovement)
    {
        float normTime = currentAccelerationTime / playerSettings.timeToFullAcceleration;
        float accelerationSpeed = playerSettings.accelerationCurve.Evaluate(normTime);
        
        rb.AddForce(new Vector3(relativeMovement.x * playerSettings.currentSpeed,
            0.0f,
            relativeMovement.z * playerSettings.currentSpeed), ForceMode.VelocityChange);
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

        SoundManager.Instance.PlayAudioClip("PlayerLaunch");
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

        anim.SetTrigger("Impulse");

        if (other != null)
        {
            other.gameObject.GetComponentInParent<ITargetable>()?.SetTargettedState(false);
            other.gameObject.GetComponentInParent<HealthController>()?.RecieveDamage(1.0f);
        }

        rb.AddForce(Vector3.up * reboundMultiplier, ForceMode.Impulse);

        SoundManager.Instance.PlayAudioClip("PlayerRebound");

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
        hookJoint.spring = hookSpring;
        hookJoint.maxDistance = hookMaxDistance;
        hookJoint.connectedAnchor = Vector3.zero;

        lineRenderer.enabled = true;
    }

    /// <summary>
    /// Draw hook between player and hookpoint
    /// </summary>
    /// <param name="endHookPos"></param>
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

        rb.AddForce(Vector3.up * reboundMultiplier, ForceMode.Impulse);
        anim.SetTrigger("Impulse");

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
    public void AddForce(Vector3 direction, float speedBoost, ForceMode forceMode = ForceMode.Impulse)
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