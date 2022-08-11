using System;
using UnityEngine;

namespace _Scripts.View {
    public class GunRecoil : MonoBehaviour {
        [SerializeField] private float backPosition;
        [SerializeField] private float startPosition;
        [SerializeField] private float recoilSpeed;
        
        private bool _doRecoil;
        private bool _recoiled;

        public bool DoRecoil {
            get => _doRecoil;
            set => _doRecoil = value;
        }

        private void Update() {
            if (_doRecoil) Recoil();
        }

        private void Recoil() {
            Vector3 pos = transform.localPosition;
            if (!_recoiled) {
                if (pos.z > backPosition) transform.Translate(Vector3.back * Time.deltaTime * recoilSpeed);
                else if (pos.z <= backPosition) _recoiled = true; 
            }
            else {
                if (pos.z < startPosition) transform.Translate(Vector3.forward * Time.deltaTime * recoilSpeed);
                else if (pos.z >= startPosition) {
                    transform.localPosition = new Vector3(pos.x, pos.y, startPosition);
                    _recoiled = false;
                    _doRecoil = false;
                } 
            }
        }
    }
}