using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTriggerDetector : MonoBehaviour
{
    public Action<PlayerCharacter> OnPlayerTrigger;
    public Action<PlayerCharacter> OnPlayerTriggerExit;

    [SerializeField] private UnityEvent<PlayerCharacter> OnPlayerTriggerEvent;


    private void Awake()
    {
        OnPlayerTrigger += (controller) => { OnPlayerTriggerEvent?.Invoke(controller); };
    }

    private void OnDestroy()
    {
        OnPlayerTrigger -= (controller) => { OnPlayerTriggerEvent?.Invoke(controller); };
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.TryGetComponent(out PlayerCharacter playerCharacter))
        {
            OnPlayerTrigger?.Invoke(playerCharacter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerCharacter playerCharacter))
        {
            OnPlayerTriggerExit?.Invoke(playerCharacter);
        }
    }
}
