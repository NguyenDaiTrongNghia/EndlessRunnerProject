using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    [SerializeField] float _SpawnInterval = 2f;
    [SerializeField] MovementComp movementComp;


    public float SpawnInterval

    {
        get { return _SpawnInterval; }
    }

    public MovementComp GetMovementComponent()
    {
        return movementComp;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
