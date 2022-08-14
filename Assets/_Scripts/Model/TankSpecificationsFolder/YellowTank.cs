using System;

namespace _Scripts.Model.TankSpecificationsFolder {
    [Obsolete]
    public struct YellowTank{
        public static float Velocity { get; set; } = 7;

        public static float RotateVelocity { get; set; } = 14;

        public static float TurretRotateVelocity { get; set; } = 5;

        public static float CoolDown { get; set; } = 2;

        public static int HitPoints { get; set; } = 300;

        public static string Layer { get; set; } = "Player";
    }
}