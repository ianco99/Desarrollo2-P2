using Patterns.FSM;
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
    [SerializeField] private Rigidbody rb;

    private FiniteStateMachine<BeeStates> fsm;
    private BeeIdleState<BeeStates> idleState;

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

        fsm.AddState(idleState);

        fsm.SetCurrentState(idleState);

        fsm.Init();
    }
}
