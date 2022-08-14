using System;
using _Scripts.Model;
using UnityEngine;

namespace _Scripts.View {
    public class WheelRotator : MonoBehaviour {
        [SerializeField] private GameObject[] rightWheels = new GameObject[2];
        [SerializeField] private GameObject[] leftWheels = new GameObject[2];
        private TankModel _tankModel;
        private TankSpecifications _ts;

        public TankModel TankModel {
            set => _tankModel = value;
        }

        private void Start() {
            // _ts = GetComponent<TankSpecifications>();
        }

        [Obsolete("old method")]
        public void RotateWheels(float left, float right) {
            foreach (var wheel in leftWheels) {
                wheel.transform.Rotate(Vector3.right, left * _ts.Velocity * Time.deltaTime);
            }
            foreach (var wheel in rightWheels) {
                wheel.transform.Rotate(Vector3.right, right * _ts.Velocity * Time.deltaTime);
            }
        }
        public void RotateWheels(float velocity) {
            foreach (var wheel in leftWheels) {
                wheel.transform.Rotate(Vector3.right, velocity);
                print(velocity * Time.deltaTime);
            }
            foreach (var wheel in rightWheels) {
                wheel.transform.Rotate(Vector3.right, velocity);
                print(wheel.transform.rotation);

            }
        }
    }
}