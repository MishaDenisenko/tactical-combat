using _Scripts.View.Abstract;
using UnityEngine;

namespace _Scripts.View.Player {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(WheelRotator))]
    public class PlayerTankMove : TankMovement {
        [SerializeField] private Joystick joystick;

        void Start() {
            Rb = GetComponent<Rigidbody>();
            WheelRotator = GetComponent<WheelRotator>();
            Body = transform.GetChild(0);
        }

        private void FixedUpdate() {
#if UNITY_EDITOR
            // float vertical = Input.GetAxis("Vertical");
            // float horizontal = Input.GetAxis("Horizontal");
            // if (vertical != 0) Move(vertical);
            // if (horizontal != 0 && vertical >= 0) Rotate(horizontal);
            // else if (horizontal != 0 && vertical < 0) Rotate(-horizontal);
            
// #elif UNITY_IOS
            float vertical = joystick.Vertical;
            float horizontal = joystick.Horizontal;
            
            if (vertical != 0 || horizontal != 0) Move(horizontal, vertical);
#endif
        }
    }
}