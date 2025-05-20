using System;
using UnityEngine;


namespace Phac
{
    public class BulletController : MonoBehaviour
    {
        private Vector3 m_Origin;
        private float m_Life;
        private float m_HitBoxRadius = 0.1f;
        private LayerMask m_CollisionMask;
        public Action<GameObject, RaycastHit> m_OnCollision;
        public Action<GameObject> m_OnEnd;

        void Awake()
        {
            m_Life = 0.0f;
        }

        private void Update()
        {
            if (m_Life <= 0.0f)
            {
                m_OnEnd.Invoke(gameObject);
                return;
            }

            Vector3 movement = Time.deltaTime * 10.0f * transform.forward;

            if (Physics.SphereCast(transform.position, m_HitBoxRadius, movement.normalized, out RaycastHit hit, movement.magnitude, m_CollisionMask))
            {
                m_OnCollision.Invoke(gameObject, hit);

                if (hit.collider.gameObject.TryGetComponent(out Damageable hurt))
                {
                    hurt.TakeDamage(2.0f);
                }
            }

            transform.position += movement;

            m_Life -= Time.deltaTime;
            if (m_Life <= 0.0f)
            {
                m_OnEnd.Invoke(gameObject);
            }
        }
        public void Initialize(
            Vector3 position,
            Quaternion rotation,
            LayerMask mask,
            Action<GameObject, RaycastHit> collisionCallback,
            Action<GameObject> onEndCallback
            )
        {
            transform.SetPositionAndRotation(position, rotation);
            m_Origin = position;
            m_CollisionMask = mask;
            m_OnCollision = collisionCallback;
            m_OnEnd = onEndCallback;
            m_Life = 10.0f;
        }
    }
}