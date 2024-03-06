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

        /// <summary>
        /// Sets god mode cheat to ignore death and damage
        /// </summary>
        /// <param name="value"></param>
        public void SetGodMode(bool value)
        {
            godMode = value;
        }

        /// <summary>
        /// Decreases health by a given value
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public float RecieveDamage(float damage)
        {
            if (!godMode)
            {
                Health -= damage;
                OnDamaged?.Invoke();
            }

            return Health;
        }

        /// <summary>
        /// Increases health by a given value
        /// </summary>
        /// <param name="healthToHeal"></param>
        public void Heal(float healthToHeal)
        {
            Health += healthToHeal;
        }

        /// <summary>
        /// Sets health to zero
        /// </summary>
        public void Kill()
        {
            if (godMode)
                Health = 0;
        }

        /// <summary>
        /// Sets Health to a given value
        /// </summary>
        /// <param name="newHealth"></param>
        public void SetHealth(float newHealth)
        {
            Health = newHealth;
        }
    }
}

