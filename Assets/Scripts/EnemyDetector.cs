using UnityEngine;

namespace Phac
{
    public class EnemyDetector : MonoBehaviour
    {
        public float AttackRange;
        public Transform PlayerTransform { get; private set; }
        private void Awake()
        {
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public bool CanAttackPlayer() => (PlayerTransform.position - transform.position).magnitude <= AttackRange;
    }
}