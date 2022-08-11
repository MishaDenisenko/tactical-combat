using UnityEngine;

namespace _Scripts.Model {
    public class BulletSpecifications : Specifications {
        [SerializeField] private int countOfRicochets;
        [SerializeField] private int damage;
        [SerializeField] private float velocity;
        // [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject ricochetEffect;
        
        public int CountOfRicochets => countOfRicochets;

        public int Damage => damage;

        // public GameObject BulletPrefab => bulletPrefab;

        public float Velocity => velocity;

        public GameObject RicochetEffect => ricochetEffect;
    }
}