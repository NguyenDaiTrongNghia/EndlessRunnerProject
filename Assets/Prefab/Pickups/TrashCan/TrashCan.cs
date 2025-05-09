using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : Pickup
{
    [SerializeField] float CollisionPushSpeed = 20f;
    [SerializeField] float DestroyDelay = 3f;
    [SerializeField] Vector3 CollisionTorque = new Vector3(2f,2f,2f);
    protected override void PickedUpBy(GameObject picker)
    {
        //base.PickedUpBy(picker);
        GetMovementComponent().enabled = false;
        GetComponent<Collider>().enabled = false;
        Rigidbody rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        rigidBody.useGravity = true;
        rigidBody.AddForce((transform.position - picker.transform.position).normalized * CollisionPushSpeed, ForceMode.VelocityChange);
        rigidBody.AddTorque(CollisionTorque, ForceMode.VelocityChange);

        Invoke("DestroySelf", DestroyDelay);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
