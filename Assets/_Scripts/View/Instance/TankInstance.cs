using _Scripts.Controller;
using _Scripts.Model;
using _Scripts.View.Abstract;
using UnityEngine;

namespace _Scripts.View.Instance {
    [RequireComponent(
            typeof(DestroyObject),
            typeof(TankMovement)
        )
    ]
    public class TankInstance : MonoBehaviour, IInstance{
        [SerializeField] private TankSpecifications tankSpecifications;
        [SerializeField] private BulletSpecifications bulletSpecifications;
        private TankController _tankController;

        private TankMovement _tankMovement;
        private ShootButton _shootButton;
        private GunShoot _gunShoot;
        private DestroyObject _tankDestruction;
        private TurretRotator _turretRotator;

        private void Awake() {
            _tankController = new TankController();
            
            _tankMovement = GetComponent<TankMovement>();
            _shootButton = GameObject.FindWithTag("ShootButton").GetComponent<ShootButton>();
            _gunShoot = transform.GetChild(1).GetComponentInChildren<GunShoot>();
            _tankDestruction = GetComponent<DestroyObject>();
            _turretRotator = GetComponentInChildren<TurretRotator>();
            
            UpdateValues();
        }

        private void Start() {
            gameObject.layer = LayerController.GetLayer(tankSpecifications.Layer);
        }

        private void Update() {
            if (!_tankController.IsAlive()) _tankDestruction.BlowUp();
        }

        public void Hit(int damage) {
            _tankController.Hit(damage);
        }

        public void UpdateValues() { 
            _tankController.SetValues(tankSpecifications);
            
            if (_tankMovement) _tankMovement.TankSpecifications = tankSpecifications;

            if (_gunShoot) {
                _gunShoot.TankSpecifications = tankSpecifications;
                _gunShoot.BulletSpecifications = bulletSpecifications;
                _gunShoot.TankController = _tankController;
            }
            
            
            if (_shootButton) _shootButton.GunShoot = _gunShoot;

            if (_turretRotator) {
                _turretRotator.TankSpecifications = tankSpecifications;
                _turretRotator.GunShoot = _gunShoot;
            }
            
            
        }

        public void UpdateValues(bool bullet) {
            if (bullet) {
                _gunShoot.BulletSpecifications = bulletSpecifications;
                return;
            }
            
            _tankController.SetValues(tankSpecifications);

            _tankMovement.TankSpecifications = tankSpecifications;

            _gunShoot.TankSpecifications = tankSpecifications;
            _gunShoot.TankController = _tankController;

            _shootButton.GunShoot = _gunShoot;

            _turretRotator.TankSpecifications = tankSpecifications;
            _turretRotator.GunShoot = _gunShoot;
            
        }
    }
}