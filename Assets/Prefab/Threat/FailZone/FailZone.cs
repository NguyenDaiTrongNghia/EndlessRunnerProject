using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameplayStatics.GetGameMode().GameOver();
        }
    }
}
