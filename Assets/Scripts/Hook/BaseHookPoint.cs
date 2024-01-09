using UnityEngine;
using UnityEngine.Events;

public class BaseHookPoint : MonoBehaviour, IHookable, ITargetable
{
    public UnityEvent onHooked;
    public void RecieveHook()
    {
        onHooked.Invoke();
    }

    public void SetTargettedState(bool value)
    {
        throw new System.NotImplementedException();
    }
}
