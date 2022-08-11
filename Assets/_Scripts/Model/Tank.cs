using UnityEngine;

namespace _Scripts.Model {
    [CreateAssetMenu(fileName = "Tank", menuName = "Managers/Tank")]
    public class Tank : ScriptableObject{
        [SerializeField] protected float velocity;
        [SerializeField] protected float rotateVelocity;
        [SerializeField] protected float turretRotateVelocity;
        [SerializeField] protected float coolDown;
        [SerializeField] protected int hitPoints;
        [SerializeField] private LayerMask layer;

        public float Velocity => velocity;

        public float RotateVelocity => rotateVelocity;

        public float TurretRotateVelocity => turretRotateVelocity;

        public float CoolDown => coolDown;

        public int HitPoints {
            get => hitPoints;
            set => hitPoints = value;
        }

        public bool IsAlive => hitPoints > 0;
        
        public LayerMask Layer => layer;
    }
}