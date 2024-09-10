using UnityEngine;
    public interface MoveBehaviour
    {
        public void Move(Vector2 movement, bool isSprinting, Transform followTransform, Transform playerTransform);
    }
