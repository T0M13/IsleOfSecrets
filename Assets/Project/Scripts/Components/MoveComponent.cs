using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveBehaviour", menuName = "Behaviours/MoveBehaviour")]
public class MoveComponent : ScriptableObject, MoveBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float RunSpeed { get => runSpeed; set => runSpeed = value; }

    public void Move(Vector2 movement, bool isSprinting, Transform followTransform, Transform playerTransform)
    {
        float speed = isSprinting ? runSpeed : moveSpeed;

        if (movement != Vector2.zero)
        {
            Vector3 moveDirection = (followTransform.forward * movement.y + followTransform.right * movement.x).normalized;

            moveDirection.y = 0;

            moveDirection = moveDirection.normalized;

            playerTransform.position += moveDirection * speed * Time.deltaTime;

            playerTransform.rotation = Quaternion.Euler(0, followTransform.rotation.eulerAngles.y, 0);
            followTransform.localEulerAngles = new Vector3(followTransform.localEulerAngles.x, 0, 0);
        }
    }
}
