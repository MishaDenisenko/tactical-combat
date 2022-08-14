using System;
using _Scripts.Controller;
using _Scripts.Model;
using UnityEngine;

namespace _Scripts.View.Instance {
    [RequireComponent(typeof(BulletFlight), typeof(DestroyObject))]
    public class BulletInstance : MonoBehaviour, IInstance {
        [SerializeField] private BulletSpecifications bulletSpecifications;

        private BulletController _bulletController;

        private BulletFlight _bulletFlight;

        private void Awake() {
            _bulletController = new BulletController();

            _bulletFlight = GetComponent<BulletFlight>();
            _bulletFlight.DestroyObject = GetComponent<DestroyObject>();
            
            UpdateValues();
        }

        public void UpdateValues() {
            _bulletController.SetValues(bulletSpecifications);
            _bulletFlight.BulletSpecifications = bulletSpecifications;
            _bulletFlight.BulletController = _bulletController;
        }
    }
}