using System;
using System.Collections;
using _Scripts.Model;
using UnityEngine;

namespace _Scripts.Controller {
    public class TankController : IController<TankSpecifications> {
        private float _reloadTime;
        private float _timeLeft = -0.02f;
        private int _hp;

        public bool Reload { get; private set; }

        public void SetValues(TankSpecifications ts) {
            _reloadTime = ts.Cooldown;
            _hp = ts.HitPoints;
        }

        [Obsolete]
        public bool DoReload(ref bool reload) {
            if ((_timeLeft += 0.02f) < _reloadTime) return false;

            _timeLeft = 0;
            reload = false;
            return true;
        }

        public IEnumerator DoReload() {
            Reload = true;
            
            yield return new WaitForSeconds(_reloadTime);
            Reload = false;
        }

        public int Hit(int damage) {
            return (_hp -= damage) > 0 ? _hp : 0;
        }

        public bool IsAlive() {
            return _hp > 0;
        }
    }
}