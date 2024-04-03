using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePlatform : MonoBehaviour
{
    private PlayerTriggerDetector detector;
    private void Awake()
    {
        detector.OnPlayerTrigger += PlayerDetectedHandler;
    }

    private void PlayerDetectedHandler(PlayerController controller)
    {
        
    }
}
