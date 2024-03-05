using Patterns.FSM;
using UnityEngine;

public class BeeIdleState<T> : BaseState<T>
{
    private Rigidbody rb;

    private bool movingUp = true;
    private float timeToMove = 0.5f;
    private float currentTimeToMove = 0.0f;
    private float speed = 75.0f;
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

        if(currentTimeToMove >= timeToMove)
        {
            movingUp = !movingUp;
            currentTimeToMove = 0.0f;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Collider[] sas = Physics.OverlapSphere(rb.position, 30.0f, LayerMask.GetMask("Water"));

        if(sas.Length > 0)
        {
            Debug.Log("ojo ian eh");
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        currentTimeToMove = 0.0f;
    }

}