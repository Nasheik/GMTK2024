using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cameraOrientation;
    [SerializeField] private Transform cameraTarget;

    private float moveSpeed = 7f;
    [SerializeField] private float walkSpeed = 7f;
    [SerializeField] private float runSpeed = 7f;

    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private RaycastHit slopeHit;

    [SerializeField] private float groundDrag = 1;
    [SerializeField] private float groundMultiplier = 10;
    private Vector3 moveDirection;

    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float jumpCooldown = 1;
    [SerializeField] float airMultiplier = 2;
    private bool readyToJump;

    [SerializeField] private Input input;

    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    private bool isWalking;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        air
    }



    private void Awake()
    {
        input = FindObjectOfType<Input>();
        FindObjectOfType<MoveCamera>().cameraTarget = cameraTarget;
        FindObjectOfType<PlayerCamera>().orientation = cameraOrientation;
        FindObjectOfType<PlayerCamera>().input = input;
        FindAnyObjectByType<GameManager>().player = this;
        FindAnyObjectByType<GameManager>().ResetPlayer();
    }

    private void Start()
    {
        rb.freezeRotation = true;
        readyToJump = true;
    }
    private void FixedUpdate()
    {
        Vector2 moveInput = input.GetMovementVectorNormalized();
        isWalking = moveInput != Vector2.zero;
        if (isWalking) AttemptMove(moveInput);
        rb.AddForce(Vector3.down * downForce, ForceMode.Force);
        if (input.GetDashInput())
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(FindObjectOfType<PlayerCamera>().transform.forward * jumpForce, ForceMode.Impulse);
        }   
    }
    [SerializeField] float downForce;
    private void Update()
    {
        if (!IsOwner) return;

        StateHandler();

        bool jumpInput = input.GetJumpInput();

        isGrounded = Physics.BoxCast(transform.position + Vector3.up * .5f, Vector3.one/4, Vector3.down, Quaternion.identity, .275f, groundLayer);

        if (isGrounded)
        {
            if (jumpInput && readyToJump) AttempJump();
            rb.drag = groundDrag;
        }
        else rb.drag = 0;





    }
    private void StateHandler()
    {
        if(isGrounded)
        {
            state = MovementState.sprinting;
            moveSpeed = runSpeed;
        }
        else
        {
            state = MovementState.air;
            moveSpeed = walkSpeed;
        }
    }
    private void AttemptMove(Vector2 moveInput)
    {
        moveDirection = cameraOrientation.forward * moveInput.y + cameraOrientation.right * moveInput.x;
        //rb.useGravity = !OnSlope();
        if (OnSlope()) rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
        if (isGrounded) rb.AddForce(moveDirection * moveSpeed * groundMultiplier, ForceMode.Force);
        else rb.AddForce(moveDirection * moveSpeed * groundMultiplier * airMultiplier, ForceMode.Force);
        SpeedControl();
    }
    public void TeleportPlayer(Vector3 location)
    {
        rb.MovePosition(location);
        //transform.position = location;
    }

    private void SpeedControl()
    {
        if (OnSlope())
        { 
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
    }
    private void AttempJump()
    {
        readyToJump = false;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        Invoke(nameof(ResetJump), jumpCooldown);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    private bool OnSlope()
    {
        if(Physics.BoxCast(transform.position + Vector3.up * .5f, Vector3.one/4, Vector3.down, out slopeHit, Quaternion.identity, .5f, groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            Debug.Log("SLOPE" + angle);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

}
