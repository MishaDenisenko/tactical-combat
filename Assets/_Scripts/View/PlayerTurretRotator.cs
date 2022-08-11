using System;
using _Scripts.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.View {
    [RequireComponent(typeof(TurretRotator))]
    
    public class PlayerTurretRotator : MonoBehaviour{
        private Camera _camera;
        private TurretRotator _turretRotator;
        private GunShoot _gunShoot;
        private RaycastHit _hit;
        private float _startPos;
        private float _lambda = 50f;

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
            if (Input.touchCount is > 0 and < 3) {
                int lookIndex = GetLookTouchIndex(Input.touchCount);
                print(lookIndex);
                if (lookIndex != -1) {

                    if (true) {
                        RotateWithAngel(lookIndex);
                    }
                    
                    if (false) {
                        inputUp = Input.GetTouch(lookIndex).phase == TouchPhase.Ended || Input.GetTouch(lookIndex).phase == TouchPhase.Canceled;
                        inputDown = !inputUp;
                    }
                    
                }
            }
#endif
            #region ===================================== FOR ROTATION ON TOUCH
            
            if (inputDown) {
                if (false) RotateWithTarget();
            }
            else if (inputUp) {
                if (false) _gunShoot.Shoot();
            }
            
            #endregion
        }

        private void RotateWithAngel(int lookIndex) {
            var touch = Input.GetTouch(lookIndex);
            var cameraPosZ = _camera.transform.position.z;
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startPos = GetTouchPosition(touch.position, cameraPosZ);
                    break;
        
                case TouchPhase.Moved:
                    var angel = (GetTouchPosition(touch.position, cameraPosZ) - _startPos) * _lambda;
                    _turretRotator.Rotate(angel, Time.deltaTime);
                    _startPos = GetTouchPosition(touch.position, cameraPosZ);
                    break;
            }
        }

        private void RotateWithTarget() {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                
            if (Physics.Raycast(ray, out _hit) && !_hit.collider.tag.Equals("Joystick")) {
                Vector3 target = new Vector3(_hit.point.x, transform.position.y, _hit.point.z);
                _turretRotator.Rotate(target, Time.deltaTime);
            }
            
        }
        
        private int GetLookTouchIndex(int touchCount) {
            for (int i = 0; i < touchCount; i++) {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                print($"{Input.mousePosition} - mp");
                if (Physics.Raycast(ray, out var hit)) {
                    if (hit.collider.tag.Equals("Joystick")) print($"{i} - j");
                    if (!hit.collider.tag.Equals("Joystick")) {
                        print($"{i} - nj");
                        return i;
                    }
                }
            }

            return -1;
        }


        private float GetTouchPosition(Vector2 touchPosition, float cameraPosZ) {
            return _camera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, cameraPosZ)).x;
        }
    }
}