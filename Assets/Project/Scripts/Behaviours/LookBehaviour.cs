using Cinemachine;
using UnityEngine;

    public interface LookBehaviour
    {
        public void Look(Vector2 mouse, Transform followTargetTransform);
        public void LockMouse();
        public void UnlockMouse();

    }
