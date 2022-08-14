using System;
using _Scripts.Model.TankSpecificationsFolder;
using UnityEngine;

namespace _Scripts.Model {
    [Obsolete]
    public class TankModel : MonoBehaviour {
        [SerializeField] private TankColor color;
        private enum TankColor {
            Yellow,
            Green,
            Blue,
            Red
        }

        private void Awake() {
            switch (color) {
                case TankColor.Yellow: gameObject.layer = LayerMask.NameToLayer(YellowTank.Layer); break;
            }
        }

        public float GetVelocity() {
            switch (color) {
                case TankColor.Yellow: return YellowTank.Velocity;
                default: return 0;
            }
        }
        
        public float GetRotateVelocity() {
            switch (color) {
                case TankColor.Yellow: return YellowTank.RotateVelocity;
                default: return 0;
            }
        }
        
        public float GetTurretRotateVelocity() {
            switch (color) {
                case TankColor.Yellow: return YellowTank.TurretRotateVelocity;
                default: return 0;
            }
        }
        
        public float GetCoolDown() {
            switch (color) {
                case TankColor.Yellow: return YellowTank.CoolDown;
                default: return 0;
            }
        }
        
        public int GetHitPoints() {
            switch (color) {
                case TankColor.Yellow: return YellowTank.HitPoints;
                default: return 0;
            }
        }
        
        public string GetLayer() {
            switch (color) {
                case TankColor.Yellow: return YellowTank.Layer;
                default: return "";
            }
        }
    }
}