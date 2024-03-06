using Patterns.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeLaunchState<T> : BaseState<T>
{
    private Animator anim;
    private Transform model;
    private Rigidbody rb;

    private float launchForce = 35.0f;

    public BeeLaunchState(Animator anim, Transform model, Rigidbody rigidbody, T id, string name) : base(id, name)
    {
        this.anim = anim;
        this.model = model;
        this.rb = rigidbody;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        anim.SetTrigger("Launching");
        rb.AddForce(model.forward * launchForce, ForceMode.VelocityChange);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
