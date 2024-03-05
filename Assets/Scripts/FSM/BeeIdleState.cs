using Patterns.FSM;
using UnityEngine;

public class BeeIdleState<T> : BaseState<T>
{
    private Rigidbody rb;

    private float timeToMove = 0.5f;
    private float currentTimeToMove = 0.0f;
    private float speed = 75.0f;
    private bool movingUp = true;
    private bool foundTarget;
    public bool FoundTarget
    {
        private set => foundTarget = value;
        get => foundTarget;
    }

    public BeeIdleState(T id, Rigidbody rigidbody) : base(id)
    {
        rb = rigidbody;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (movingUp)
            rb.velocity = Vector3.up * speed * Time.fixedDeltaTime;
        if (!movingUp)
            rb.velocity = -Vector3.up * speed * Time.fixedDeltaTime;

        currentTimeToMove += Time.fixedDeltaTime;

        if (currentTimeToMove >= timeToMove)
        {
            movingUp = !movingUp;
            currentTimeToMove = 0.0f;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Collider[] sas = Physics.OverlapSphere(rb.position, 30.0f, LayerMask.GetMask("Water"));

        if (sas.Length > 0)
        {
            FoundTarget = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        FoundTarget = false;
        currentTimeToMove = 0.0f;
    }

}