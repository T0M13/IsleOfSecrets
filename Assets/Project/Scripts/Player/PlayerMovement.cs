using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerReferences playerReferences;

    [Header("Movement")]
    [SerializeField] private MoveComponent moveComponent;
    [SerializeField] private Vector2 movement;
    [SerializeField] private bool canMove = true;
    [SerializeField][ShowOnly] private bool isSprinting;

    [Header("Jump")]
    [SerializeField] private JumpComponent jumpComponent;
    [SerializeField] private bool canJump = true;
    [SerializeField][ShowOnly] private bool isJumping = false;
    [SerializeField] private float jumpThreshold = .1f;

    [Header("Fall")]
    [SerializeField][ShowOnly] private bool isFalling = false;
    [SerializeField] private float fallThreshold = -2f;

    [Header("Other")]
    [SerializeField][ShowOnly] private float verticalVelocity;

    [Header("Gizmo Settings")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private float gizmoLineLength = 2f;
    [SerializeField] private float gizmoSphereRadius = 0.1f;

    public Vector2 Movement { get => movement; set => movement = value; }
    public bool IsSprinting { get => isSprinting; set => isSprinting = value; }
    public bool IsJumping { get => isJumping; set => isJumping = value; }
    public bool IsFalling { get => isFalling; set => isFalling = value; }

    private void Awake()
    {
        GetReferences();
    }

    private void OnValidate()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        if (playerReferences == null)
        {
            playerReferences = GetComponent<PlayerReferences>();
            if (playerReferences == null)
            {
                Debug.LogWarning("PlayerReferences component is missing from PlayerMovement");
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
        CheckJumpAndFallThresholds(); // Check thresholds in every FixedUpdate
    }

    private void Move()
    {
        if (!canMove) return;

        moveComponent.Move(Movement, IsSprinting, playerReferences.FollowPlayerTarget, transform);
    }

    private void Jump()
    {
        if (!canJump || !isJumping) return;

        jumpComponent.Jump(playerReferences.PlayerBody, playerReferences);
    }

    private void CheckJumpAndFallThresholds()
    {
        verticalVelocity = playerReferences.PlayerBody.velocity.y;

        if (verticalVelocity > jumpThreshold)
        {
            isJumping = true;
            isFalling = false;
        }
        else if (verticalVelocity < fallThreshold && !playerReferences.IsGrounded)
        {
            isJumping = false;
            isFalling = true;
        }
        else
        {
            isJumping = false;
            isFalling = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        Movement = value.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext value)
    {
        IsSprinting = value.ReadValue<float>() >= 1f;
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (isJumping) return;
        isJumping = value.ReadValue<float>() >= 1f;
    }
}
