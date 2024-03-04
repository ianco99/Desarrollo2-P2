using Patterns.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAimingState<T> : BaseState<T>
{
    private LineRenderer lineRenderer;

    public BeeAimingState(LineRenderer lineRenderer, T id, string name) : base(id, name)
    {
        this.lineRenderer = lineRenderer;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        
    }
}
