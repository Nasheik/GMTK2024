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

    [SerializeField] private float downForce = 20f;
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
        if (!IsOwner) return;
        Vector2 moveInput = input.GetMovementVectorNormalized();
        isWalking = moveInput != Vector2.zero;
        if (isWalking) AttemptMove(moveInput);
        rb.AddForce(Vector3.down * downForce, ForceMode.Force);
        //if (input.GetDashInput())
        //{
        //    rb.velocity = Vector3.zero;
        //    rb.AddForce(FindObjectOfType<PlayerCamera>().transform.forward * jumpForce, ForceMode.Impulse);
        //}

        bool shouldFloat = Physics.BoxCast(transform.position + Vector3.up * .5f, Vector3.one / 4, Vector3.down, out RaycastHit hit, Quaternion.identity, .75f, groundLayer);
        if (shouldFloat)
        {
            float diff = floatHeight - hit.distance;
            rb.AddForce(Vector3.up * diff * floatForce, ForceMode.Force);
        }

        StateHandler();

        GroundCheck();
    }

    [SerializeField] float floatHeight = .5f;
    [SerializeField] float floatForce = 100;
    [SerializeField] float coyoteTime = .2f;
    float timeAtLastGround;
    Vector3 positionAtLastGround;
    void GroundCheck()
    {
        isGrounded = Physics.BoxCast(transform.position + Vector3.up * .5f, Vector3.one / 4, Vector3.down, out RaycastHit hit, Quaternion.identity, .5f, groundLayer);
        bool isOnMountain = hit.collider && hit.collider.gameObject.layer == LayerMask.NameToLayer("Mountain");
        bool isOnSlime = hit.collider && hit.collider.gameObject.layer == LayerMask.NameToLayer("Slime");

        bool jumpInput = input.GetJumpInput();

        if (isOnMountain)
        {
            jumpInput = false;
            isGrounded = false;
            rb.AddForce(3 * downForce * Vector3.down , ForceMode.Force);
            rb.AddForce(10 * downForce * hit.normal, ForceMode.Force);
        }
        if(isOnSlime)
        {
            jumpInput = false;
            rb.AddForce(3 * jumpForce * hit.normal, ForceMode.Impulse);
        }

        if (!jumpInput) downForce = 10;


        if (!isGrounded && timeAtLastGround + coyoteTime > Time.time && jumpInput && readyToJump)
        {
            AttemptJump();
        }

        if (isGrounded)
        {
            if (jumpInput && readyToJump) AttemptJump();
            downForce = 10;
            rb.drag = groundDrag;
            timeAtLastGround = Time.time;
            positionAtLastGround = transform.position;
        }
        else
        {
            if (jumpInput && rb.velocity.y > 0) downForce = .5f;
            else downForce = 10;
            rb.drag = 1;
        }
    }

    private void StateHandler()
    {
        if (isGrounded && input.GetDashInput())
        {
            state = MovementState.sprinting;
            moveSpeed = runSpeed;
        }
        else if (isGrounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }
    }
    private void AttemptMove(Vector2 moveInput)
    {
        moveDirection = cameraOrientation.forward * moveInput.y + cameraOrientation.right * moveInput.x;
        bool isMovingBackwards = Vector3.Dot(moveDirection, cameraOrientation.forward) < -.5f;
        float backwardsMultiplier = isMovingBackwards ? .25f : 1;
        if (OnSlope()) rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f * backwardsMultiplier, ForceMode.Force);
        if (isGrounded) rb.AddForce(moveDirection * moveSpeed * groundMultiplier * backwardsMultiplier, ForceMode.Force);
        else rb.AddForce(moveDirection * moveSpeed * airMultiplier, ForceMode.Force);
        SpeedControl();
    }
    public void TeleportPlayer(Vector3 location)
    {
        rb.MovePosition(location);
    }

    private void SpeedControl()
    {
        if (OnSlope())
        {
            Debug.Log("ONSLOPEEE");
            if (rb.velocity.magnitude > moveSpeed)
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
    private void AttemptJump()
    {
        readyToJump = false;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        Debug.Log("Jump");
        Invoke(nameof(ResetJump), jumpCooldown);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    private bool OnSlope()
    {
        if (Physics.BoxCast(transform.position + Vector3.up * .5f, Vector3.one / 4, Vector3.down, out slopeHit, Quaternion.identity, .5f, groundLayer))
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
