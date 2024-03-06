using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace kuznickiAttackables
{
    public class HealthController : MonoBehaviour
    {
        public UnityEvent OnDamaged;
        public UnityEvent OnDeath;
        [SerializeField] private bool godMode = false;
        [SerializeField] private float health;
        public float Health
        {
            get => health;
            private set
            {
                health = value;

                if (health <= 0)
                {
                    OnDeath?.Invoke();
                }
            }
        }


        public void SetGodMode(bool value)
        {
            godMode = value;
        }

        public float RecieveDamage(float damage)
        {
            if (!godMode)
            {
                Health -= damage;
                OnDamaged?.Invoke();
            }

            return Health;
        }

        public void Heal(float healthToHeal)
        {
            Health += healthToHeal;
        }

        public void Kill()
        {
            if (godMode)
                Health = 0;
        }

        public void SetHealth(float newHealth)
        {
            Health = newHealth;
        }
    }
}

