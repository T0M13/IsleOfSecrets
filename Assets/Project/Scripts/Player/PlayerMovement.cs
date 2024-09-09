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
    [SerializeField] private bool isSprinting;

    [Header("Gizmo Settings")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private float gizmoLineLength = 2f;
    [SerializeField] private float gizmoSphereRadius = 0.1f;
    public Vector2 Movement { get => movement; set => movement = value; }
    public bool IsSprinting { get => isSprinting; set => isSprinting = value; }

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
    }

    private void Move()
    {
        if (!canMove) return;

        moveComponent.Move(Movement, IsSprinting, playerReferences.FollowPlayerTarget, transform);

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

}
