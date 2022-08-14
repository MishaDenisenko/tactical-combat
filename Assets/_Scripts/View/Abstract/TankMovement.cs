using System;
using _Scripts.Model;
using UnityEngine;

namespace _Scripts.View.Abstract {
    public abstract class TankMovement : MonoBehaviour {
        private Rigidbody _rb;
        private TankSpecifications _tankSpecifications;
        private WheelRotator _wheelRotator;
        private Transform _body;

        protected Rigidbody Rb {
            get => _rb;
            set => _rb = value;
        }

        public TankSpecifications TankSpecifications {
            get => _tankSpecifications;
            set => _tankSpecifications = value;
        }

        protected WheelRotator WheelRotator {
            get => _wheelRotator;
            set => _wheelRotator = value;
        }

        protected Transform Body {
            get => _body;
            set => _body = value;
        }

        [Obsolete]
        protected void Move(float dir) {
            var velocity = _tankSpecifications.Velocity;
            _rb.velocity = transform.forward * velocity * dir;
            _wheelRotator.RotateWheels(dir, dir);
        }
        
        protected void Move(float horizontal, float vertical) {
            var newPosition = new Vector3(horizontal, 0, vertical);
            var velocity = _tankSpecifications.Velocity;
            
            _rb.MovePosition(transform.position + newPosition * velocity * Time.deltaTime);
            _wheelRotator.RotateWheels(velocity);
            var target = Quaternion.LookRotation(newPosition); 
            Rotate(target);
        }

        [Obsolete]
        protected void Rotate(float dir) {
            var rotateVelocity = _tankSpecifications.RotateVelocity;
            _rb.rotation *= Quaternion.AngleAxis(dir * rotateVelocity / 3, Vector3.up);
            _wheelRotator.RotateWheels(dir, -dir);
        }
        
        protected void Rotate(Quaternion target) {
            // _rb.rotation = Quaternion.Slerp(_rb.rotation, target, _tankSpecifications.RotateVelocity * Time.fixedDeltaTime);
            _body.rotation = Quaternion.Slerp(_body.rotation, target, _tankSpecifications.RotateVelocity * Time.fixedDeltaTime);
        }
        
    }
}