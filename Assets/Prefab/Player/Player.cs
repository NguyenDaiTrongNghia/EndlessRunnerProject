using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInput playerInput;

    [SerializeField] Transform[] LaneTransforms;

    [SerializeField] float MoveSpeed = 20.0f;

    [SerializeField] float JumpHeight = 2.5f;

    [SerializeField] Transform GroundCheckTransform;

    [SerializeField] [Range(0, 1)] float GroundCheckRadious = 0.2f;

    [SerializeField] LayerMask GroundCheckLayerMask;

    Vector3 Destination;

    int CurrentLaneIndex;

    //Reference of animator
    Animator animator;

    Camera playerCamera;
    Vector3 playerCameraOffset;
    private void OnEnable()
    {
        if(playerInput==null)
        {
            playerInput = new PlayerInput();
        }    
        playerInput.Enable();

    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerInput.gameplay.Move.performed += MovePerformed;
        playerInput.gameplay.Jump.performed += JumpPerformed;
        for (int i = 0; i < LaneTransforms.Length; i++)
        {
            if (LaneTransforms[i].position == transform.position)
            {
                CurrentLaneIndex = 1;
                Destination = LaneTransforms[i].position;
            }
        }
        animator = GetComponent<Animator>();

        playerCamera = Camera.main;
        playerCameraOffset = playerCamera.transform.position - transform.position;
    }

    private void JumpPerformed(InputAction.CallbackContext context)
    {
        if(!isOnGround())
        {
            return;
        }
        Rigidbody rigidBody = GetComponent<Rigidbody>();
        if (rigidBody!= null)
        {
            //Calculate gravity
            float jumpUpSpeed = Mathf.Sqrt(2 * JumpHeight * Physics.gravity.magnitude);
            rigidBody.AddForce(new Vector3(0.0f, jumpUpSpeed, 0.0f ), ForceMode.VelocityChange);
        }
    }

    private void MovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!isOnGround())
        {
            return;
        }
        float InputValue = context.ReadValue<float>();
        //Debug.Log($"move action performed, with value {InputValue}.");
        if (InputValue > 0)
        {
            MoveRight();
        }
        else
        {
            MoveLeft();
        }
    }

    private void MoveLeft()
    {
        if(CurrentLaneIndex == 0)
        {
            return;
        }

        CurrentLaneIndex--;
        Destination = LaneTransforms[CurrentLaneIndex].position;
    }

    private void MoveRight()
    {
        if (CurrentLaneIndex == LaneTransforms.Length - 1)
        {
            return;
        }

        CurrentLaneIndex++;
        Destination = LaneTransforms[CurrentLaneIndex].position;
    }
    // Update is called once per frame
    void Update()
    {
        //ground check
        if(!isOnGround())
        {
            //set the IsOnGround parameter based on ground check
            animator.SetBool("IsOnGround", false);
            //Debug.Log("Player is not on ground ");
            return;
        }
        animator.SetBool("IsOnGround", true);
        //Debug.Log("Player is on ground ");
        //Lerping 
        //transform.position = Vector3.Lerp(transform.position, Destination, Time.deltaTime * MoveSpeed);
        float TransformX = Mathf.Lerp(transform.position.x, Destination.x, Time.deltaTime * MoveSpeed);
        transform.position = new Vector3(TransformX, transform.position.y, transform.position.z);
        
    }

    private void LateUpdate()
    {
        playerCamera.transform.position = transform.position + playerCameraOffset;
    }

    bool isOnGround()
    {
        //ground check Physics CheckSphere
        return Physics.CheckSphere(GroundCheckTransform.position, GroundCheckRadious, GroundCheckLayerMask);

    }
}
