using UnityEngine;

namespace _Scripts.Model {
    [CreateAssetMenu(fileName = "BulletSpecifications", menuName = "Managers/BulletSpecifications")]
    public class BulletSpecifications : Specifications {
        [SerializeField] private int countOfRicochets;
        [SerializeField] private int damage;
        [SerializeField] private float velocity;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject ricochetEffect;
        // [SerializeField] private GameObject destroyEffect;

        public int CountOfRicochets => countOfRicochets;

        public int Damage => damage;

        public GameObject BulletPrefab => bulletPrefab;

        public float Velocity => velocity;

        public GameObject RicochetEffect => ricochetEffect;

        // public GameObject DestroyEffect => destroyEffect;
    }
}