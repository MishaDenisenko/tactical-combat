using System;
using _Scripts.Controller;
using _Scripts.Model;
using UnityEngine;

namespace _Scripts.View {
    [RequireComponent(typeof(GunRecoil))]
    public class GunShoot : MonoBehaviour {
        [SerializeField] private GameObject shootExplosion;

        private BulletSpecifications _bs;
        private GunRecoil _gunRecoil;
        private TankSpecifications _ts;
        private TankController _tankController;

        private bool _reload = true;
        private bool _canShoot;
        private Coroutine _coroutine;

        [Obsolete]
        public TankSpecifications TankSpecifications {
            set => _ts = value;
        }

        public TankController TankController {
            set => _tankController = value;
        }

        public BulletSpecifications BulletSpecifications {
            set => _bs = value;
        }

        private void Start() {
            _gunRecoil = GetComponent<GunRecoil>();
            _coroutine = StartCoroutine(_tankController.DoReload());
        }

        private void FixedUpdate() {
            if (!_tankController.Reload) {
                StopCoroutine(_coroutine);
            }
        }

        public void Shoot() {
            if (!_tankController.Reload) {
                var tr = transform.GetChild(0);
                var pos = tr.position;
                var bullet = Instantiate(_bs.BulletPrefab, pos, tr.rotation);
                var effect = Instantiate(shootExplosion, pos, Quaternion.identity);

                bullet.layer = LayerController.GetLayer(_bs.Layer);
                if (!_gunRecoil.DoRecoil) _gunRecoil.DoRecoil = true;
                _coroutine = StartCoroutine(_tankController.DoReload());
                
            }
        }

        
    }
}