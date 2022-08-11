using _Scripts.Controller;
using _Scripts.Model;
using UnityEngine;

namespace _Scripts.View {
    [RequireComponent(typeof(GunRecoil))]
    public class GunShoot : MonoBehaviour {
        [SerializeField] private Bullet bulletModel;
        [SerializeField] private GameObject shootExplosion;

        private GameObject _bulletPrefab;
        private GunRecoil _gunRecoil;

        private void Start() {
            _bulletPrefab = bulletModel.BulletPrefab;
            _gunRecoil = GetComponent<GunRecoil>();
        }

        public void Shoot() {
            var tr = transform.GetChild(0);
            var pos = tr.position;
            var bullet = Instantiate(_bulletPrefab, pos, tr.rotation);
            var effect = Instantiate(shootExplosion, pos, Quaternion.identity);
            
            bullet.layer = LayerController.GetLayer(bulletModel.Layer);
            if (!_gunRecoil.DoRecoil) _gunRecoil.DoRecoil = true;
        }
    }
}