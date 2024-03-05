using Patterns.FSM;
using UnityEngine;

public class BeeAimingState<T> : BaseState<T>
{
    private LineRenderer lineRenderer;
    private Transform transform;
    private Transform modelTransform;
    private PlayerCharacter foundPlayer;

    private float timeToAim = 1.0f;
    private float currentTimeToAim = 0.0f;
    private bool readyToLaunch = false;

    public bool ReadyToLaunch
    {
        private set
        {
            readyToLaunch = value;
        }
        get => readyToLaunch;
    }

    public BeeAimingState(Transform model, LineRenderer lineRenderer, Transform transform, T id, string name) : base(id, name)
    {
        this.modelTransform = model;
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
                    modelTransform.LookAt(foundPlayer.transform);
                    currentTimeToAim += Time.deltaTime;
                }
            }
        }

        if(lostPlayer)
        {
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);

            currentTimeToAim = 0.0f;
        }

        if(currentTimeToAim >= timeToAim)
        {
            ReadyToLaunch = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);

        currentTimeToAim = 0.0f;
        ReadyToLaunch = false;
    }
}
