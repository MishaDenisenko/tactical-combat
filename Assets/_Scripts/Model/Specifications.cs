using UnityEngine;

namespace _Scripts.Model {
    public class Specifications : MonoBehaviour {
        [SerializeField] protected LayerMask layer;

        public LayerMask Layer => layer;
    }
}