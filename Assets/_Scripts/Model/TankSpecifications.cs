using System;
using _Scripts.Controller;
using UnityEngine;

namespace _Scripts.Model {
    public class TankSpecifications : MonoBehaviour {
        [SerializeField] private float velocity;
        [SerializeField] private float rotateVelocity;
        [SerializeField] private float turretRotateVelocity;
        [SerializeField] private float cooldown;
        [SerializeField] private int hitPoints;
        [SerializeField] private LayerMask layer;

        private void Start() {
            gameObject.layer = LayerController.GetLayer(layer);
        }

        public float Velocity {
            get => velocity;
            set => velocity = value;
        }

        public float RotateVelocity {
            get => rotateVelocity;
            set => rotateVelocity = value;
        }

        public float TurretRotateVelocity {
            get => turretRotateVelocity;
            set => turretRotateVelocity = value;
        }

        public float Cooldown {
            get => cooldown;
            set => cooldown = value;
        }

        public int HitPoints {
            get => hitPoints;
            set => hitPoints = value;
        }

        public LayerMask Layer {
            get => layer;
            set => layer = value;
        }
    }
}