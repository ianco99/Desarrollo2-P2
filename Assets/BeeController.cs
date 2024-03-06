using kuznickiAttackables;
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

    private FiniteStateMachine<BeeStates> fsm;
    private BeeIdleState<BeeStates> idleState;
    private BeeAimingState<BeeStates> aimingState;
    private BeeLaunchState<BeeStates> launchState;

    private void Awake()
    {
        InitFSM();
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
        rb.AddForce(-rb.velocity + Vector3.up * 5.0f, ForceMode.Force);

        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
