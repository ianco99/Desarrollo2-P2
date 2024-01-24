using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class BaseHookPoint : BaseTargetteable, IHookable, ITargetable
{
    private Rigidbody rb;
    public UnityEvent onHooked;

    private void Awake()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }

    public void RecieveHook()
    {
        onHooked.Invoke();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }
}
