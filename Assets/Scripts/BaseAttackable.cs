using UnityEngine;
using UnityEngine.Events;

namespace kuznickiAttackables
{
    [RequireComponent(typeof(HealthController))]
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

        public Transform GetTransform()
        {
            return transform;
        }
    }
}
