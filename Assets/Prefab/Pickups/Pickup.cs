using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Spawnable
{
    [SerializeField] int scoreEffect;
    [SerializeField] float SpeedEffect;
    [SerializeField] float SpeedEffectDuration;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //add score, speed change
            SpeedControl speedController = FindObjectOfType<SpeedControl>();
            if (speedController != null && SpeedEffect != 0)
            {
                speedController.ChangeGlobalSpeed(SpeedEffect, SpeedEffectDuration);
            }

            ScoreKeeper scoreKeeper = FindObjectOfType<ScoreKeeper>();
            if (scoreKeeper != null && scoreEffect != 0)
            {
                scoreKeeper.ChangeScore(scoreEffect);
            }

            Destroy(gameObject);
        }

        if(other.gameObject.tag == "Threat")
        {
            //Debug.Log("Over lap");
            Collider col = other.GetComponent<Collider>();
            if(col!=null)
            {
                transform.position = col.bounds.center + (col.bounds.extents.y + gameObject.GetComponent<Collider>().bounds.center.y) * Vector3.up;
            }
        }
    }
}
