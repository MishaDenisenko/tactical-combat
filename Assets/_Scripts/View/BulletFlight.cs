using System;
using _Scripts.Model;
using UnityEngine;

namespace _Scripts.View {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(DestroyObject))]
    public class BulletFlight : MonoBehaviour {
        [SerializeField] private Bullet bulletModel;

        private Rigidbody _rb;

        private void Start() {
            _rb = GetComponent<Rigidbody>();

            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("PlayerBullet"));
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("EnemyBullet")); 
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerBullet"), LayerMask.NameToLayer("PlayerBullet")); 
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyBullet"), LayerMask.NameToLayer("PlayerBullet")); 
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyBullet"), LayerMask.NameToLayer("EnemyBullet"));
        }
        
        private void FixedUpdate() {
            _rb.velocity = transform.forward * bulletModel.Velocity;
        }

        private void OnTriggerEnter(Collider other) {
            print("trigger" + other.gameObject);
        }

        private void OnCollisionEnter(Collision collision) {
            if (collision.collider.tag.Equals("Wall") || collision.collider.tag.Equals("Rock")) {
                var contactPoint = collision.contacts[0];
                var dir = Vector3.Reflect(transform.forward, contactPoint.normal);
                transform.rotation = Quaternion.LookRotation(dir);

                Instantiate(bulletModel.RicochetEffect, contactPoint.point, transform.rotation);
            }
        }
    }
}