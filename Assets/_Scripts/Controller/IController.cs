using _Scripts.Model;
using JetBrains.Annotations;

namespace _Scripts.Controller {
    public interface IController<in T> {
        public void SetValues(T s);
    }
}