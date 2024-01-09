using UnityEngine;

public class BaseTargetteable : MonoBehaviour, ITargetable
{
    [SerializeField] private ToggleOutline toggleOutline;

    /// <summary>
    /// Set object targetted state
    /// </summary>
    /// <param name="value"></param>
    public void SetTargettedState(bool value)
    {
        toggleOutline.SetOutlines(value);
    }
}
