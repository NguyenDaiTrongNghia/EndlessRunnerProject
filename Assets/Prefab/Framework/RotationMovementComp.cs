using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMovementComp : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float RotationSpeed = 20f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
    }
}
