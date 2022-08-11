using System;
using _Scripts.Model;
using UnityEngine;

namespace _Scripts.View {
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    [RequireComponent(typeof(WheelRotator), typeof(DestroyObject), typeof(TankSpecifications))]
    public class PlayerTankMove : MonoBehaviour {
        [SerializeField] private Joystick joystick;
        
        private TankSpecifications _tankSpecifications;
        private Rigidbody _rb;
        private WheelRotator _wheelRotator;

        void Start() {
            _tankSpecifications = GetComponent<TankSpecifications>();
            
            _rb = GetComponent<Rigidbody>();
            _wheelRotator = GetComponent<WheelRotator>();
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

        [Obsolete]
        private void Move(float dir) {
            var velocity = _tankSpecifications.Velocity;
            _rb.velocity = transform.forward * velocity * dir;
            _wheelRotator.RotateWheels(dir, dir);
        }
        
        private void Move(float horizontal, float vertical) {
            var newPosition = new Vector3(horizontal, 0, vertical);
            var velocity = _tankSpecifications.Velocity;
            
            _rb.MovePosition(transform.position + newPosition * velocity * Time.deltaTime);
            _wheelRotator.RotateWheels(velocity);
            var target = Quaternion.LookRotation(newPosition); 
            Rotate(target);
        }

        [Obsolete]
        private void Rotate(float dir) {
            var rotateVelocity = _tankSpecifications.RotateVelocity;
            _rb.rotation *= Quaternion.AngleAxis(dir * rotateVelocity / 3, Vector3.up);
            _wheelRotator.RotateWheels(dir, -dir);
        }
        
        private void Rotate(Quaternion target) {
            _rb.rotation = Quaternion.Slerp(_rb.rotation, target, _tankSpecifications.RotateVelocity * Time.fixedDeltaTime);
        }
    }
}