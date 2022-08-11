using UnityEngine;

namespace _Scripts.Model {
    [CreateAssetMenu(fileName = "Bullet", menuName = "Managers/Bullet", order = 0)]
    public class Bullet : ScriptableObject {
        [SerializeField] private int countOfRicochets;
        [SerializeField] private int damage;
        [SerializeField] private float velocity;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject ricochetEffect;
        [SerializeField] private LayerMask layer;
        

        public int CountOfRicochets => countOfRicochets;

        public int Damage => damage;

        public GameObject BulletPrefab => bulletPrefab;

        public LayerMask Layer => layer;

        public float Velocity => velocity;

        public GameObject RicochetEffect => ricochetEffect;
    }
}