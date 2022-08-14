using System;
using _Scripts.Model;

namespace _Scripts.Controller {
    public class TankController : IController<TankSpecifications> {
        private float _reloadTime;
        private float _timeLeft = -0.02f;
        private int _hp;

        public void SetValues(TankSpecifications ts) {
            _reloadTime = ts.Cooldown;
            _hp = ts.HitPoints;
        }

        public bool DoReload(ref bool reload) {
            if ((_timeLeft += 0.02f) < _reloadTime) return false;

            _timeLeft = 0;
            reload = false;
            return true;
        }

        public int Hit(int damage) {
            return (_hp -= damage) > 0 ? _hp : 0;
        }

        public bool IsAlive() {
            return _hp > 0;
        }
    }
}