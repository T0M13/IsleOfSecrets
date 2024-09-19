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
    [SerializeField][ShowOnly] private bool isCrouching;

    [Header("Jump")]
    [SerializeField] private JumpComponent jumpComponent;
    [SerializeField] private bool canJump = true;
    [SerializeField][ShowOnly] private bool hasJumped = false;
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
    public bool IsSprinting
    {
        get => isSprinting;
        set
        {
            if (isSprinting != value)
            {
                isSprinting = value;
                OnSprintingChanged();
            }
        }
    }
    public bool IsJumping
    {
        get => isJumping;
        set
        {
            if (isJumping != value)
            {
                isJumping = value;
                OnJumpingChanged();
            }
        }
    }
    public bool IsFalling
    {
        get => isFalling;
        set
        {
            if (isFalling != value)
            {
                isFalling = value;
                OnFallingChanged();
            }
        }
    }
    public bool IsCrouching
    {
        get => isCrouching;
        set
        {
            if (isCrouching != value)
            {
                isCrouching = value;
                OnCrouchingChanged();
            }
        }
    }

    public bool HasJumped { get => hasJumped; set => hasJumped = value; }

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
        CheckJumpAndFallThresholds();
    }

    private void Move()
    {
        if (!canMove) return;

        moveComponent.Move(Movement, IsSprinting, IsCrouching, playerReferences.FollowPlayerTarget, playerReferences.PlayerBody);
    }

    private void Jump()
    {
        if (!canJump || !isJumping || HasJumped) return;

        jumpComponent.Jump(playerReferences.PlayerBody, playerReferences);
        HasJumped = true;
    }

    private void CheckJumpAndFallThresholds()
    {
        verticalVelocity = playerReferences.PlayerBody.velocity.y;

        if (verticalVelocity > jumpThreshold)
        {
            isJumping = true;
            HasJumped = true;
            isFalling = false;

        }
        else if (verticalVelocity < fallThreshold && !playerReferences.IsGrounded)
        {
            isJumping = false;
            HasJumped = true;
            isFalling = true;
        }
        else
        {
            isJumping = false;
            isFalling = false;
            HasJumped = false;
        }
    }


    private void OnSprintingChanged()
    {

    }

    private void OnJumpingChanged()
    {

    }

    private void OnFallingChanged()
    {

    }

    private void OnCrouchingChanged()
    {
        if (isCrouching)
        {
            playerReferences.SetCrouchCollider();
        }
        else
        {
            playerReferences.SetDefaultCollider();
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
        if (isCrouching) return;
        IsSprinting = value.ReadValue<float>() >= 1f;
    }
    public void OnCrouch(InputAction.CallbackContext value)
    {
        if (isSprinting) return;
        IsCrouching = value.ReadValue<float>() >= 1f;
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (isJumping || hasJumped) return;
        if (isCrouching) return;
        isJumping = value.ReadValue<float>() >= 1f;
    }
}
