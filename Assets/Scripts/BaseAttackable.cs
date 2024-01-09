using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseAttackable : BaseTargetteable, IAttackable
{
    public UnityEvent OnAttacked;
    /// <summary>
    /// Runs when receiving attack by player
    /// </summary>
    public void ReceiveAttack()
    {
        OnAttacked.Invoke();
    }
}
