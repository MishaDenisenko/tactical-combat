using _Scripts.Controller;
using _Scripts.Model;
using UnityEngine;

namespace _Scripts.View {
    [RequireComponent(typeof(GunRecoil))]
    public class GunShoot : MonoBehaviour {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject shootExplosion;

        private GunRecoil _gunRecoil;

        private void Start() {
            _gunRecoil = GetComponent<GunRecoil>();
        }

        public void Shoot() {
            var tr = transform.GetChild(0);
            var pos = tr.position;
            var bullet = Instantiate(bulletPrefab, pos, tr.rotation);
            var effect = Instantiate(shootExplosion, pos, Quaternion.identity);
            
            bullet.layer = LayerController.GetLayer(bulletPrefab.GetComponent<BulletSpecifications>().Layer);
            if (!_gunRecoil.DoRecoil) _gunRecoil.DoRecoil = true;
        }
    }
}