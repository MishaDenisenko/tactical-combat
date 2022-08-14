using _Scripts.Model;

namespace _Scripts.Controller {
    public class BulletController : IController<BulletSpecifications> {
        private int _countOfRicochets;
        private int _currentCountOfRicochets;

        public void SetValues(BulletSpecifications bs) {
            _countOfRicochets = bs.CountOfRicochets;
        }
        
        public bool CanRicochets() {
            return ++_currentCountOfRicochets < _countOfRicochets;
        }
    }
}