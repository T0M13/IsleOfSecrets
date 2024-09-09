using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerReferences playerReferences;

    [Header("Look")]
    [SerializeField] private LookComponent lookComponent;
    [SerializeField] private Vector2 lookPos;
    public Vector2 LookPos { get => lookPos; set => lookPos = value; }


    private void Awake()
    {
        GetReferences();
    }

    private void OnValidate()
    {
        GetReferences();
    }

    private void Start()
    {
        LockMouse();
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

    public void LockMouse()
    {
        lookComponent.LockMouse();
    }

    private void LateUpdate()
    {
        Look();
    }

    private void Look()
    {
        lookComponent.Look(LookPos, playerReferences.FollowPlayerTarget);
    }

    private void LockLook()
    {
        lookComponent.LockMouse();
    }

    private void UnlockLook()
    {
        lookComponent.UnlockMouse();
    }

    public void OnLook(InputAction.CallbackContext value)
    {
        LookPos = value.ReadValue<Vector2>();
    }

}
