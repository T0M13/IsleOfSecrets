using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LookBehaviour", menuName = "Behaviours/LookBehaviour")]
public class LookComponent : ScriptableObject, LookBehaviour
{
    [Header("Look Settings")]
    [SerializeField] private float minViewDistance = 25f;
    [SerializeField] private float maxViewDistance = 340f;
    [SerializeField] private float sensitivity = 100f;
    [SerializeField] private float rotationPower = 1f;
    [SerializeField] private bool reverseMouse = false;
    [SerializeField] private float midAngleThreshold = 180f;

    public void Look(Vector2 mouse, Transform followTransform)
    {
        float mouseX = mouse.x * sensitivity * rotationPower * Time.deltaTime;
        float mouseY = (reverseMouse ? mouse.y : -mouse.y) * sensitivity * rotationPower * Time.deltaTime;

        followTransform.rotation *= Quaternion.AngleAxis(mouseX, Vector3.up);
        followTransform.rotation *= Quaternion.AngleAxis(mouseY, Vector3.right);

        Vector3 angles = followTransform.localEulerAngles;
        angles.z = 0f;

        float angle = followTransform.localEulerAngles.x;

        if (angle > midAngleThreshold && angle < maxViewDistance)
        {
            angles.x = maxViewDistance;
        }
        else if (angle < midAngleThreshold && angle > minViewDistance)
        {
            angles.x = minViewDistance;
        }

        followTransform.localEulerAngles = angles;
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
