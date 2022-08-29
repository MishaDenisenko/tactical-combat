using System;
using _Scripts.View.Abstract;
using UnityEngine;

namespace _Scripts.View.Player {
    
    public class PlayerTurretRotator : TurretRotator{
        private Camera _camera;
        private RaycastHit _hit;
        private float _startPos;
        private float _lambda = 50f;

        private void Start() {
            _camera = Camera.main;
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
                if (false) gunShoot.Shoot();
            }
            
            #endregion
        }

        private void RotateWithAngel(int lookIndex) {
            var touch = Input.GetTouch(lookIndex);
            var angel = 0f;
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startPos = touch.position.x;
                    break;
        
                case TouchPhase.Moved:
                    angel = touch.position.x - _startPos;
                    break;
            }

            Rotate(angel, Time.deltaTime);
            _startPos = touch.position.x;
        }

        [Obsolete]
        private void RotateWithTarget() {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                
            if (Physics.Raycast(ray, out _hit) && !_hit.collider.tag.Equals("Joystick")) {
                Vector3 target = new Vector3(_hit.point.x, transform.position.y, _hit.point.z);
                Rotate(target, Time.deltaTime);
            }
        }
        
        private int GetLookTouchIndex(int touchCount) {
            for (int i = 0; i < touchCount; i++) {
                var ray = _camera.ScreenPointToRay(Input.GetTouch(i).position);
                if (Physics.Raycast(ray, out var hit) && !hit.collider.tag.Equals("Joystick")) {
                    return i;
                }
            }

            return -1;
        }


        private float GetTouchPosition(Vector2 touchPosition, float cameraPosZ) {
            return _camera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, cameraPosZ)).x;
        }
    }
}