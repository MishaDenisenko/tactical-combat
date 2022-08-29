using System;
using _Scripts.Controller;
using _Scripts.Model;
using _Scripts.View.Instance;
using UnityEngine;

namespace _Scripts.View {
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class BulletFlight : MonoBehaviour {
        private Rigidbody _rb;
        private BulletSpecifications _bs;
        private BulletController _bulletController;
        private DestroyObject _destroyObject;
        private int _countOfRicochets;

        public BulletSpecifications BulletSpecifications {
            set => _bs = value;
        }

        public BulletController BulletController {
            get => _bulletController;
            set => _bulletController = value;
        }

        public new DestroyObject DestroyObject {
            set => _destroyObject = value;
        }

        private void Start() {
            _rb = GetComponent<Rigidbody>();

            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("PlayerBullet"));
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("EnemyBullet"));
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerBullet"), LayerMask.NameToLayer("PlayerBullet"));
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyBullet"), LayerMask.NameToLayer("PlayerBullet"));
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyBullet"), LayerMask.NameToLayer("EnemyBullet"));
        }

        private void FixedUpdate() {
            _rb.velocity = transform.forward * _bs.Velocity;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.tag.Equals("Enemy")) {
                other.gameObject.GetComponent<TankInstance>().Hit(_bs.Damage);
                _destroyObject.BlowUp();
            }
        }

        private void OnCollisionEnter(Collision collision) {
            var tr = transform;
            if (collision.collider.tag.Equals("Wall") || collision.collider.tag.Equals("Rock")) {
                var contactPoint = collision.contacts[0];
                
                if (_bulletController.CanRicochets()) {
                    var dir = Vector3.Reflect(transform.forward, contactPoint.normal);
                    transform.rotation = Quaternion.LookRotation(dir);

                    Instantiate(_bs.RicochetEffect, contactPoint.point, tr.rotation);
                    return;
                }

                _destroyObject.BlowUp();
            }
        }
    }
}