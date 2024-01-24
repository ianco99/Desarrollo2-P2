using UnityEngine;

public interface IHookable
{
    public void RecieveHook();
    public Transform GetTransform();
    public Rigidbody GetRigidbody();
}
