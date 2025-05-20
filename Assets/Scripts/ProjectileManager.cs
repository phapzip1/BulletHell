using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

namespace Phac
{
    public class ProjectileManager : Utility.Singleton<ProjectileManager>
    {
        [SerializeField] private int DefaultCapacity;
        [SerializeField] private GameObject BulletPrefab;
        [SerializeField] private LayerMask CollisionMask;

        private ObjectPool<GameObject> m_BulletPool;
        private readonly List<GameObject> m_ActiveBullets = new List<GameObject>();
        private readonly List<GameObject> m_BulletsToReturn = new List<GameObject>();

        protected override void Awake()
        {
            base.Awake();
            m_BulletPool = new ObjectPool<GameObject>(
                createFunc: () =>
                {
                    GameObject obj = Instantiate(BulletPrefab, transform.position, transform.rotation);
                    obj.SetActive(false);
                    return obj;

                },
                actionOnGet: bullet => bullet.SetActive(true),
                actionOnRelease: bullet => bullet.SetActive(false),
                actionOnDestroy: bullet =>
                {
                    if (bullet) Destroy(bullet);
                },
                defaultCapacity: DefaultCapacity,
                collectionCheck: false,
                maxSize: DefaultCapacity * 100

            );
        }

        private void LateUpdate()
        {
            foreach (GameObject bullet in m_BulletsToReturn)
            {
                m_ActiveBullets.Remove(bullet);
                m_BulletPool.Release(bullet);
            }

            m_BulletsToReturn.Clear();
        }

        private void OnCollision(GameObject bullet, RaycastHit hit)
        {
            m_BulletsToReturn.Add(bullet);
        }
        
        private void OnEnd(GameObject bullet)
        {
            m_BulletsToReturn.Add(bullet);
        }

        public void Spawn(Vector3 position, Quaternion rotation)
        {
            GameObject bullet = m_BulletPool.Get();
            bullet.GetComponent<BulletController>().Initialize(
                position,
                rotation,
                CollisionMask,
                OnCollision,
                OnEnd
            );
            m_ActiveBullets.Add(bullet);
        }
    }
}