using UnityEngine;

namespace kuznickiAttackables
{
    public interface IAttackable
    {
        public void ReceiveAttack();
        public Transform GetTransform();
    }
}