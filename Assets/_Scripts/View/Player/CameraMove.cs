using System;
using UnityEngine;

namespace _Scripts.View.Player {
    public class CameraMove : MonoBehaviour {
        [SerializeField] private float leftPosition;
        [SerializeField] private float topPosition;
        [SerializeField] private float rightPosition;
        [SerializeField] private float bottomPosition;

        [SerializeField] private Transform player;
        [SerializeField] private float backDistance;

        [SerializeField] private float velocity;

        private float _delta = 0.5f;
        // private Vector3 _cameraPosition;
        // private Vector3 _playerPosition;

        private void Start() {
        }

        private void Update() {
            if (!player) return;
            var cameraPosition = transform.position;
            var playerPosition = player.position;
            
            if (cameraPosition.z - player.position.z < backDistance) {
                VerticalMove(cameraPosition, playerPosition);
            }

            if (cameraPosition.x < playerPosition.x - _delta || cameraPosition.x > playerPosition.x + _delta) {
                HorizontalMove(cameraPosition, playerPosition);
            }
        }

        private void VerticalMove(Vector3 camPos, Vector3 playerPos) {
            var distance = playerPos.z - backDistance;
            var targetPosition = Vector3.zero;
                
            if (distance > bottomPosition && distance < topPosition) {
                targetPosition = new Vector3(camPos.x, camPos.y, distance);
            }
            else if (distance < bottomPosition) {
                targetPosition = new Vector3(camPos.x, camPos.y, bottomPosition);
            }
            else if (distance > topPosition) {
                targetPosition = new Vector3(camPos.x, camPos.y, topPosition);
            }

            Move(transform.position, targetPosition);
        }

        private void HorizontalMove(Vector3 camPos, Vector3 playerPos) {
            // Vector3 targetPosition;
            var targetPosition = new Vector3(playerPos.x, camPos.y, camPos.z);
            // if (targetPosition.x > leftPosition && targetPosition.x < rightPosition) {
            //     Move(transform.position, targetPosition);
            //     return;
            // }
            
            if (targetPosition.x < leftPosition) {
                targetPosition = new Vector3(leftPosition, camPos.y, camPos.z);
            }
            else if (targetPosition.x > rightPosition) {
                targetPosition = new Vector3(rightPosition, camPos.y, camPos.z);
            }

            Move(transform.position, targetPosition);
        }

        private void Move(Vector3 currentPos, Vector3 targetPos) {
            transform.position = Vector3.Lerp(currentPos, targetPos, velocity * Time.deltaTime);
        }
    }
}