using kuznickiAttackables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<PlayerTriggerDetector> weaponDamagers;
    [SerializeField] private List<Transform> weaponParents;
    [SerializeField] private float damageToDeal;

    private void Start()
    {
        for (int i = 0; i < weaponDamagers.Count; i++)
        {
            weaponDamagers[i].OnPlayerTrigger += DealDamage;
        }
    }

    /// <summary>
    /// Returns a list of all weapons parents
    /// </summary>
    /// <returns></returns>
    public List<Transform> GetWeaponParents()
    {
        return weaponParents;
    }

    /// <summary>
    /// Deals damage with weapon to player
    /// </summary>
    /// <param name="playerController"></param>
    private void DealDamage(PlayerController playerController)
    {
        HealthController playerHealth;

        if(playerController.gameObject.TryGetComponent(out playerHealth))
        {
            playerHealth.RecieveDamage(damageToDeal);
        }
    }
}
