using UnityEngine;

namespace _Scripts.View.Player {
    public class ShootButton : MonoBehaviour {
        private GunShoot _gunShoot;

        public GunShoot GunShoot {
            set => _gunShoot = value;
        }

        public void Shoot() {
            if (_gunShoot) _gunShoot.Shoot();
        }
    }
}