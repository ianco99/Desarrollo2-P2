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
        Collider[] foundCollliders = Physics.OverlapSphere(transform.position, 30.0f, LayerMask.GetMask("Water"));

        for (int i = 0; i < foundCollliders.Length; i++)
        {
            if(foundCollliders[i].tag == "Player")
            {
                Debug.Log("Es por acá");

                if (foundCollliders[i].TryGetComponent(out foundPlayer))
                {
                    lineRenderer.SetPosition(0, Vector3.zero);
                    lineRenderer.SetPosition(1, foundPlayer.transform.position - transform.position);
                }
            }
        }
    }
}
