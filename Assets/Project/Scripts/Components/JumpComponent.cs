using UnityEngine;

[CreateAssetMenu(fileName = "JumpBehaviour", menuName = "Behaviours/JumpBehaviour")]
public class JumpComponent : ScriptableObject, JumpBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;

    public void Jump(Rigidbody rb, PlayerReferences playerReferences)
    {
        if (playerReferences.IsGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

}
