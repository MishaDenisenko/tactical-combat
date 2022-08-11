using System;
using _Scripts.Model;
using UnityEngine;

namespace _Scripts.View {
    public class TurretRotator : MonoBehaviour {
        private TankSpecifications _ts;

        private void Start() {
            _ts = GetComponentInParent<TankSpecifications>();
        }

        [Obsolete]
        public void Rotate(int dir, float deltaTime) {
            transform.Rotate(Vector3.up, dir * _ts.RotateVelocity * deltaTime);
        }
        
        public void Rotate(Vector3 target, float deltaTime) {
            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _ts.RotateVelocity * deltaTime);
        }
    }
}