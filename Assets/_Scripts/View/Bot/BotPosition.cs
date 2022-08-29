using UnityEngine;

namespace _Scripts.View.Bot {
    public class BotPosition {
        private bool _activate;
        private BotTankMove _bot;
        private Vector3 _position;

        public BotPosition(Vector3 position) {
            _position = position;
        }

        public void SetPosition(BotTankMove bot) {
            if (!_activate) _activate = true;
            _bot = bot;
        }

        public void ResetPosition() {
            _activate = true;
            _bot = null;
        }

        public bool Activate => _activate;

        public BotTankMove Bot => _bot;

        public Vector3 Position => _position;
    }
}