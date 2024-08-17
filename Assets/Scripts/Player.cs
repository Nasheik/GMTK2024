using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cameraOrientation;
    [SerializeField] private Transform cameraTarget;

    [SerializeField] private float moveSpeed = 7f;
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




    private void Awake()
    {
        input = FindObjectOfType<Input>();
        FindObjectOfType<MoveCamera>().cameraTarget = cameraTarget;
        FindObjectOfType<PlayerCamera>().orientation = cameraOrientation;
        FindObjectOfType<PlayerCamera>().input = input;
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
    }
    private void Update()
    {
        if (!IsOwner) return;

        bool jumpInput = input.GetJumpInput();

        isGrounded = Physics.Raycast(transform.position + Vector3.up * .1f, Vector3.down, .3f, groundLayer);

        if (isGrounded)
        {
            if (jumpInput && readyToJump) AttempJump();
            rb.drag = groundDrag;
        }
        else rb.drag = 0;

    }
    private void AttemptMove(Vector2 moveInput)
    {
        moveDirection = cameraOrientation.forward * moveInput.y + cameraOrientation.right * moveInput.x;
        if (isGrounded) rb.AddForce(moveDirection * moveSpeed * groundMultiplier, ForceMode.Force);
        else rb.AddForce(moveDirection * moveSpeed * groundMultiplier * airMultiplier, ForceMode.Force);
        SpeedControl();
    }
    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
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
}
