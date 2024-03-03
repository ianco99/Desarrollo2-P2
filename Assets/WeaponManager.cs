using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<PlayerTriggerDetector> weaponDamagers;
    [SerializeField] private List<Transform> weaponParents;
    public List<Transform> GetWeaponParents()
    {
        return weaponParents;
    }

    public static Transform PlayerTriggerToTransform(PlayerTriggerDetector detector)
    {
        return detector.transform;
    }
}
