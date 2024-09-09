using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerReferences playerReferences;
    [SerializeField] private Animator playerAnimator;

    [Header("Variables")]
    private static readonly string xPosParam = "xPos";
    private static readonly string yPosParam = "yPos";
    private static readonly string isMovingParam = "isMoving";
    private static readonly string isSprintingParam = "isSprinting";

    private Vector2 movement;

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

    private void Update()
    {
        UpdateAnimatorParameters();
    }

    private void UpdateAnimatorParameters()
    {
        if (playerReferences != null && playerAnimator != null)
        {
            movement = playerReferences.PlayerMovement.Movement;

            playerAnimator.SetFloat(xPosParam, movement.x);
            playerAnimator.SetFloat(yPosParam, movement.y);

            bool isMoving = movement.magnitude > 0.1f;
            playerAnimator.SetBool(isMovingParam, isMoving);
            playerAnimator.SetBool(isSprintingParam, playerReferences.PlayerMovement.IsSprinting);
        }
    }
}
