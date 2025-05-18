using Phac.Scriptable;
using UnityEngine;


namespace Phac
{
    public class BulletController : MonoBehaviour
    {
        public Vector3 Origin;
        private TrailRenderer m_TrailRenderer;
        private float m_Life;
        private BulletConfig m_Config;

        void Awake()
        {
            m_TrailRenderer = GetComponent<TrailRenderer>();
            m_Life = 0.0f;
        }

        private void OnDisable()
        {
   
            m_TrailRenderer.Clear();
        }

        public bool Move(LayerMask collisionMask, out RaycastHit hits)
        {
            Vector3 movement = Time.deltaTime * m_Config.Speed * transform.forward;

            if (Physics.Raycast(transform.position, movement, out hits, movement.magnitude, collisionMask))
            {
                m_Life = 0.0f;
                return true;
            }

            transform.position += movement;
            m_Life -= Time.deltaTime;

            if (m_Life <= 0.0f)
            {
                return true;
            }

            return false;
        }

        public void Initialize(BulletConfig config, Vector3 position, Quaternion rotation)
        {
            m_Config = config;
            transform.SetPositionAndRotation(position, rotation);
            m_Config.SetupTrail(m_TrailRenderer);
            Origin = position;
            m_Life = config.LifeTime;
        }
    }
}