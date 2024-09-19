using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private CapsuleCollider playerCollider;
    [Header("Grounded")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 1f;
    [SerializeField] private float groundCheckDistance = 1.1f;
    [Header("Camera")]
    [SerializeField] private GameObject virtualPlayerCameraPrefab;
    [SerializeField][ShowOnly] private CinemachineVirtualCamera virtualPlayerCamera;
    [SerializeField] private Transform followPlayerTarget;

    [Header("Collider Settings")]
    [SerializeField] private Vector3 defaultColliderCenter = new Vector3(0, 0, 0);
    [SerializeField] private float defaultColliderHeight = 1.75f;

    [SerializeField] private Vector3 crouchColliderCenter = new Vector3(0.075f, -0.25f, 0.2f);
    [SerializeField] private float crouchColliderHeight = 1.5f;

    public PlayerMovement PlayerMovement { get => playerMovement; set => playerMovement = value; }
    public PlayerLook PlayerLook { get => playerLook; set => playerLook = value; }
    public Rigidbody PlayerBody { get => playerBody; set => playerBody = value; }
    public CapsuleCollider PlayerCollider { get => playerCollider; set => playerCollider = value; }
    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
    public CinemachineVirtualCamera VirtualPlayerCamera { get => virtualPlayerCamera; set => virtualPlayerCamera = value; }
    public Transform FollowPlayerTarget { get => followPlayerTarget; set => followPlayerTarget = value; }
    public PlayerAnimation PlayerAnimation { get => playerAnimation; set => playerAnimation = value; }

    private void OnValidate()
    {
        GetReferences();
    }

    private void Awake()
    {
        GetReferences();
        SpawnVirtualCamera();
    }

    private void GetReferences()
    {
        if (PlayerMovement == null)
        {
            try
            {
                PlayerMovement = GetComponent<PlayerMovement>();
            }
            catch
            {
                Debug.Log("PlayerMovement Missing from PlayerReferences");
            }
        }

        if (PlayerLook == null)
        {
            try
            {
                PlayerLook = GetComponent<PlayerLook>();
            }
            catch
            {
                Debug.Log("PlayerLook Missing from PlayerReferences");
            }
        }

        if (PlayerAnimation == null)
        {
            try
            {
                PlayerAnimation = GetComponent<PlayerAnimation>();
            }
            catch
            {
                Debug.Log("PlayerAnimation Missing from PlayerReferences");
            }
        }

        if (PlayerBody == null)
        {
            try
            {
                PlayerBody = GetComponent<Rigidbody>();
            }
            catch
            {
                Debug.Log("Rigidbody Missing from PlayerReferences");
            }
        }
        if (playerCollider == null)
        {
            try
            {
                playerCollider = GetComponent<CapsuleCollider>();
            }
            catch
            {
                Debug.Log("CapsuleCollider Missing from PlayerReferences");
            }
        }
    }

    private void Update()
    {
        isGrounded = CheckIsGrounded(playerBody);
    }

    private bool CheckIsGrounded(Rigidbody rb)
    {
        return Physics.SphereCast(rb.position, groundCheckRadius, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer);
    }


    private void SpawnVirtualCamera()
    {
        if (VirtualPlayerCamera == null)
        {
            GameObject newCamera = Instantiate(virtualPlayerCameraPrefab, null);
            VirtualPlayerCamera = newCamera.GetComponent<CinemachineVirtualCamera>();
            VirtualPlayerCamera.Follow = FollowPlayerTarget;
        }
    }

    public void SetDefaultCollider()
    {
        playerCollider.center = defaultColliderCenter;
        playerCollider.height = defaultColliderHeight;
    }
    public void SetCrouchCollider()
    {
        playerCollider.center = crouchColliderCenter;
        playerCollider.height = crouchColliderHeight;
    }

    private void OnDrawGizmosSelected()
    {
        if (playerBody != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(playerBody.position + Vector3.down * groundCheckDistance, groundCheckRadius);
        }
    }


}
