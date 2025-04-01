using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovementComp : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 20f;
    [SerializeField] Vector3 MoveDirect = Vector3.forward;

    [SerializeField] Vector3 Destination;
    // Start is called before the first frame update
    void Start()
    {
        SpeedControl speedController = FindObjectOfType<SpeedControl>();
        if(speedController!=null)
        {
            speedController.onGlobalSpeedChanged += SetMoveSpeed;
            SetMoveSpeed(speedController.GetGlobalSpeed());
        }
    }

    public void SetMoveDirect(Vector3 dir)
    {
        MoveDirect = dir;
    }

    public void SetDestination(Vector3 newDestination)
    {
        Destination = newDestination;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += MoveDirect * MoveSpeed * Time.deltaTime;
        if(Vector3.Dot((Destination - transform.position).normalized, MoveDirect) < 0)
        {
            Destroy(gameObject);
        }
    }

    internal void SetMoveSpeed(float newMoveSpeed)
    {
        MoveSpeed = newMoveSpeed;
    }

    public void CopyFrom(MovementComp other)
    {
        SetMoveSpeed(other.MoveSpeed);
        SetMoveDirect(other.MoveDirect);
        SetDestination(other.Destination);
    }
}
