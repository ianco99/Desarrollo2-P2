using Patterns.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAimingState<T> : BaseState<T>
{
    private LineRenderer lineRenderer;
    private Transform transform;
    private PlayerCharacter foundPlayer;

    public BeeAimingState(LineRenderer lineRenderer, Transform transform, T id, string name) : base(id, name)
    {
        this.lineRenderer = lineRenderer;
        this.transform = transform;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        bool lostPlayer = true;

        Collider[] foundCollliders = Physics.OverlapSphere(transform.position, 30.0f, LayerMask.GetMask("Water"));

        for (int i = 0; i < foundCollliders.Length; i++)
        {
            if(foundCollliders[i].tag == "Player")
            {
                if (foundCollliders[i].TryGetComponent(out foundPlayer))
                {
                    lostPlayer = false;
                    lineRenderer.SetPosition(0, Vector3.zero);
                    lineRenderer.SetPosition(1, foundPlayer.transform.position - transform.position);
                }
            }
        }

        if(lostPlayer)
        {
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);
        }
    }
}
