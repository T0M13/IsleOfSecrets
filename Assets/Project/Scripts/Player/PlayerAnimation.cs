using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerReferences playerReferences;
    [SerializeField] private Animator playerAnimator;

    [Header("Settings")]
    [SerializeField][ShowOnly] private AnimationState currentState = AnimationState.Idle;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private float smoothInputSpeed = 5f;

    [Header("Variables")]
    private static readonly string xPosParam = "xPos";
    private static readonly string yPosParam = "yPos";
    private static readonly string isMovingParam = "isMoving";
    private static readonly string isSprintingParam = "isSprinting";
    private static readonly string isJumpingParam = "isJumping";
    private static readonly string isFallingParam = "isFalling";
    private static readonly string isCrouchingParam = "isCrouching";

    private Vector2 smoothedMovement = Vector2.zero;
    private Vector2 movementVelocity = Vector2.zero;

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
                Debug.LogWarning("PlayerReferences component is missing from PlayerAnimation");
            }
        }

        if (playerAnimator == null)
        {
            playerAnimator = GetComponentInChildren<Animator>();
            if (playerAnimator == null)
            {
                Debug.LogWarning("Animator component is missing from PlayerAnimation");
            }
        }
    }

    public void ChangeAnimationState(AnimationState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        playerAnimator.Play(newState.ToString());
    }

    private void Update()
    {
        UpdateAnimatorParameters();
    }

    private void UpdateAnimatorParameters()
    {
        if (playerReferences != null && playerAnimator != null)
        {
            Vector2 rawMovement = playerReferences.PlayerMovement.Movement;

            smoothedMovement.x = Mathf.SmoothDamp(smoothedMovement.x, rawMovement.x, ref movementVelocity.x, 1f / smoothInputSpeed);
            smoothedMovement.y = Mathf.SmoothDamp(smoothedMovement.y, rawMovement.y, ref movementVelocity.y, 1f / smoothInputSpeed);

            playerAnimator.SetBool(isFallingParam, playerReferences.PlayerMovement.IsFalling);

            float targetX = Mathf.Lerp(playerAnimator.GetFloat(xPosParam), smoothedMovement.x, smoothTime);
            float targetY = Mathf.Lerp(playerAnimator.GetFloat(yPosParam), smoothedMovement.y, smoothTime);

            playerAnimator.SetFloat(xPosParam, targetX);
            playerAnimator.SetFloat(yPosParam, targetY);

            bool isMoving = smoothedMovement.magnitude > 0.1f;
            playerAnimator.SetBool(isMovingParam, isMoving);
            playerAnimator.SetBool(isSprintingParam, playerReferences.PlayerMovement.IsSprinting);

            playerAnimator.SetBool(isJumpingParam, playerReferences.PlayerMovement.IsJumping);
            playerAnimator.SetBool(isCrouchingParam, playerReferences.PlayerMovement.IsCrouching);
        }
    }
}
