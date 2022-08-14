using System;
using _Scripts.Model;
using UnityEngine;

namespace _Scripts.View.Abstract {
    public abstract class TurretRotator : MonoBehaviour {
        private TankSpecifications _ts;
        protected GunShoot gunShoot;

        public TankSpecifications TankSpecifications {
            set => _ts = value;
        }

        public GunShoot GunShoot {
            set => gunShoot = value;
        }

        [Obsolete]
        public void Rotate(int dir, float deltaTime) {
            transform.Rotate(Vector3.up, dir * _ts.RotateVelocity * deltaTime);
        }

        protected void Rotate(Vector3 target, float deltaTime) {
            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _ts.TurretRotateVelocity * deltaTime);
        }

        protected void Rotate(float angel, float deltaTime) {
            var rotation = transform.rotation;
            var targetRotation = Quaternion.AngleAxis(angel, Vector3.down) * rotation;
            transform.rotation = Quaternion.Slerp(rotation, targetRotation, _ts.TurretRotateVelocity * deltaTime);
        }
    }
}