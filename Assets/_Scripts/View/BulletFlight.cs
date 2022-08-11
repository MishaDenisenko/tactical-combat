using System;
using _Scripts.Model;
using UnityEngine;

namespace _Scripts.View {
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    [RequireComponent(typeof(DestroyObject), typeof(BulletSpecifications))]
    public class BulletFlight : MonoBehaviour {
        private Rigidbody _rb;
        private BulletSpecifications _bs;

        private void Start() {
            _rb = GetComponent<Rigidbody>();
            _bs = GetComponent<BulletSpecifications>();

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
            print("trigger" + other.gameObject);
        }

        private void OnCollisionEnter(Collision collision) {
            if (collision.collider.tag.Equals("Wall") || collision.collider.tag.Equals("Rock")) {
                var contactPoint = collision.contacts[0];
                var dir = Vector3.Reflect(transform.forward, contactPoint.normal);
                transform.rotation = Quaternion.LookRotation(dir);

                Instantiate(_bs.RicochetEffect, contactPoint.point, transform.rotation);
            }
        }
    }
}