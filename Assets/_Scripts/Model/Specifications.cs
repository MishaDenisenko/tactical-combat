using UnityEngine;

namespace _Scripts.Model {
    public class Specifications : ScriptableObject {
        [SerializeField] protected LayerMask layer;

        public LayerMask Layer => layer;
    }
}