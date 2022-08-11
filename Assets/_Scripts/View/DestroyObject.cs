using UnityEngine;

namespace _Scripts.View {
    public class DestroyObject : MonoBehaviour {
        
        [SerializeField] private GameObject explosionEffect;

        public void BlowUp() {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}