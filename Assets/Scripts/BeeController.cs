using kuznickiAttackables;
using kuznickiEventChannel;
using Patterns.FSM;
using System.Collections;
using UnityEngine;

enum BeeStates
{
    Idle,
    Aiming,
    Launch
}

[RequireComponent(typeof(Rigidbody))]
public class BeeController : MonoBehaviour
{
    [SerializeField] private Transform model;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LineRenderer aimingLine;
    [SerializeField] private HealthController healthController;
    [SerializeField] private PlayerControllerEventChannel respawnChannel;
    [SerializeField] private float timeTillDeath = 3.0f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Quaternion startModelRotation;

    private FiniteStateMachine<BeeStates> fsm;
    private BeeIdleState<BeeStates> idleState;
    private BeeAimingState<BeeStates> aimingState;
    private BeeLaunchState<BeeStates> launchState;

    private float DeathUpMultiplier = 5.0f;

    private void Awake()
    {
        InitFSM();
        respawnChannel.Subscribe(Respawn);

        startPosition = transform.position;
        startRotation = transform.rotation;
        startModelRotation = model.rotation;
    }

    private void OnDestroy()
    {
        respawnChannel.Unsubscribe(Respawn);
    }

    private void Respawn(PlayerController controller)
    {
        StopAllCoroutines();

        anim.SetTrigger("Flaps");

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.rotation = startRotation;

        transform.SetPositionAndRotation(startPosition, startRotation);
        
        gameObject.SetActive(true);
        
        model.eulerAngles = Vector3.zero;


        fsm.SetCurrentState(idleState);
    }

    private void Update()
    {
        fsm.Update();
    }

    private void FixedUpdate()
    {
        fsm.FixedUpdate();
    }

    private void InitFSM()
    {
        fsm = new FiniteStateMachine<BeeStates>();

        idleState = new BeeIdleState<BeeStates>(BeeStates.Idle, rb);
        aimingState = new BeeAimingState<BeeStates>(model, aimingLine, transform, BeeStates.Aiming, "Aiming");
        launchState = new BeeLaunchState<BeeStates>(anim, model, rb, BeeStates.Launch, "Launch");

        fsm.AddState(idleState);
        fsm.AddState(aimingState);

        fsm.AddTransition(idleState, aimingState, () => idleState.FoundTarget);
        fsm.AddTransition(aimingState, launchState, () => aimingState.ReadyToLaunch);

        fsm.SetCurrentState(idleState);

        fsm.Init();
    }

    private void OnCollisionEnter(Collision collision)
    {
        healthController.RecieveDamage(10);
    }

    public void Die()
    {
        rb.AddTorque(-rb.velocity);
        rb.AddForce(-rb.velocity + Vector3.up * DeathUpMultiplier, ForceMode.Force);

        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(timeTillDeath);
        gameObject.SetActive(false);
    }
}
