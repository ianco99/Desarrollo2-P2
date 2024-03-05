using Patterns.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAimingState<T> : BaseState<T>
{
    private LineRenderer lineRenderer;
    private Transform transform;

    public BeeAimingState(LineRenderer lineRenderer, Transform transform, T id, string name) : base(id, name)
    {
        this.lineRenderer = lineRenderer;
        this.transform = transform;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Collider[] sas = Physics.OverlapSphere(transform.position, 10.0f);

        for (int i = 0; i < sas.Length; i++)
        {
            if(sas[i].tag == "Player")
            {
                Debug.Log("lol");
            }
        }
    }
}
