using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace kuznickiAttackables
{
    public class HealthController : MonoBehaviour
    {
        public UnityEvent OnDeath;
        private float health;
        public float Health
        {
            get => health;
            private set
            {
                health = value;

                if(health == 0)
                {
                    OnDeath?.Invoke();
                }
            }
        }

        public float RecieveDamage(float damage)
        {
            Health -= damage;
            return Health;
        }

        public void Kill()
        {
            Health = 0;
        }
    }
}

