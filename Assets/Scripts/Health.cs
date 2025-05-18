using Phac.Utility;
using UnityEngine;

namespace Phac
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float MaxHealthPoint = 10.0f;
        public Observer<float> CurrentHP;
        private void Awake()
        {
            CurrentHP = new Observer<float>(MaxHealthPoint);
        }

        private void OnEnable()
        {
            CurrentHP.AddListener(OnHealthChanged);

        }

        private void OnDisable()
        {
            CurrentHP.RemoveAllListener();
        }

        public void ResetHealthPoint()
        {
            CurrentHP.Value = MaxHealthPoint;
        }

        public void OnHealthChanged(float health)
        {
            if (health <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}