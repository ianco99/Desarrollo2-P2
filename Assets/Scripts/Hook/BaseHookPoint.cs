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

    /// <summary>
    /// On being hooked
    /// </summary>
    public void RecieveHook()
    {
        onHooked.Invoke();
    }

    /// <summary>
    /// Returns transform of object
    /// </summary>
    /// <returns></returns>
    public Transform GetTransform()
    {
        return transform;
    }

    /// <summary>
    /// Returns rigidbody of object
    /// </summary>
    /// <returns></returns>
    public Rigidbody GetRigidbody()
    {
        return rb;
    }
}
