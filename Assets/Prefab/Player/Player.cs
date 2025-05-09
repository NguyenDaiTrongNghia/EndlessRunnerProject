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
    [SerializeField] Vector3 BlockageCheckHalfExtend;
    [SerializeField] string BlockageCheckTag = "Threat";


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
                CurrentLaneIndex = i;
                Destination = LaneTransforms[i].position;
            }
        }
        animator = GetComponent<Animator>();

        playerCamera = Camera.main;
        playerCameraOffset = playerCamera.transform.position - transform.position;
    }

    private void JumpPerformed(InputAction.CallbackContext context)
    {
        if(isOnGround())
        {
            Rigidbody rigidBody = GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                //Calculate gravity
                float jumpUpSpeed = Mathf.Sqrt(2 * JumpHeight * Physics.gravity.magnitude);
                rigidBody.AddForce(new Vector3(0.0f, jumpUpSpeed, 0.0f), ForceMode.VelocityChange);
            }
        }
    }

    private void MovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        float InputValue = context.ReadValue<float>();
        int GoalIndex = CurrentLaneIndex;
        //Debug.Log($"move action performed, with value {InputValue}.");
        if (InputValue > 0)
        {
            if (GoalIndex == LaneTransforms.Length - 1) return;
            GoalIndex++;
        }
        else
        {
            if (CurrentLaneIndex == 0) return;
            GoalIndex--;
        }

        Vector3 GoalPos = LaneTransforms[GoalIndex].position;
        if(GameplayStatics.IsPositionOccupied(GoalPos, BlockageCheckHalfExtend, BlockageCheckTag))
        {
            return;
        }

        CurrentLaneIndex = GoalIndex;
        Destination = GoalPos;
    }
    // Update is called once per frame
    void Update()
    {
        //Lerping 
        //transform.position = Vector3.Lerp(transform.position, Destination, Time.deltaTime * MoveSpeed);
        float TransformX = Mathf.Lerp(transform.position.x, Destination.x, Time.deltaTime * MoveSpeed);
        transform.position = new Vector3(TransformX, transform.position.y, transform.position.z);
        //ground check
        if (!isOnGround())
        {
            //set the IsOnGround parameter based on ground check
            animator.SetBool("IsOnGround", false);
            //Debug.Log("Player is not on ground ");
        }
        else
        {
            animator.SetBool("IsOnGround", true);
            //Debug.Log("Player is on ground ");
        }

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
