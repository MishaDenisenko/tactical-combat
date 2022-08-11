using System;
using _Scripts.Model;
using UnityEngine;

namespace _Scripts.View {
    [RequireComponent(typeof(TurretRotator))]
    
    public class PlayerTurretRotator : MonoBehaviour {
        private Camera _camera;
        private TurretRotator _turretRotator;
        private GunShoot _gunShoot;
        private RaycastHit _hit;

        private void Start() {
            _camera = Camera.main;
            _gunShoot = GetComponentInChildren<GunShoot>();
            
            _turretRotator = GetComponent<TurretRotator>();
        }

        private void Update() {
            bool inputDown = false;
            bool inputUp = false;
            
#if UNITY_MAC
            inputDown = Input.GetMouseButton(0);
            inputUp = Input.GetMouseButtonUp(0);
#elif UNITY_IOS
            if (Input.touchCount > 0 && Input.touchCount < 3) {
                int lookIndex = GetLookTouchIndex(Input.touchCount);
                if (lookIndex != -1) {
                    inputUp = Input.GetTouch(lookIndex).phase == TouchPhase.Ended || Input.GetTouch(lookIndex).phase == TouchPhase.Canceled;
                    inputDown = !inputUp;
                }
                else inputDown = inputUp = false;
            }
#endif
            
            if (inputDown) {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out _hit) && !_hit.collider.tag.Equals("Joystick")) {
                    Vector3 target = new Vector3(_hit.point.x, transform.position.y, _hit.point.z);
                    _turretRotator.Rotate(target, Time.deltaTime);
                }
            }
            else if (inputUp) {
                _gunShoot.Shoot();
            }
        }
        
        private int GetLookTouchIndex(int touchCount) {
            for (int i = 0; i < touchCount; i++) {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit) && !hit.collider.tag.Equals("Joystick")) return i;
            }

            return -1;
        }
    }
}