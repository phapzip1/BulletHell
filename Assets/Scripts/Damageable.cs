using System;
using Phac.UI;
using UnityEngine;

namespace Phac
{
    public class Damageable : MonoBehaviour, IHealth
    {
        [SerializeField] private HealthBar HealthBar;
        [SerializeField, Min(1.0f)] private float MaxHealth;
        public float Health { get => m_Health; set => m_Health = value; }
        private float m_Health = 10.0f;
        private Action<GameObject> m_OnDied;


        public void Initialize(Action<GameObject> onDiedCallback)
        {
            m_Health = MaxHealth;
            m_OnDied = onDiedCallback;
            HealthBar.Reset();
        }

        public void TakeDamage(float amount)
        {
            Health -= amount;

            if (Health < 0.0f)
            {
                Health = 0.0f;
                m_OnDied?.Invoke(gameObject);
            }
            HealthBar.HealthPoint = Health / MaxHealth;
        }
    }
}