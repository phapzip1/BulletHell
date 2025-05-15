using System.Collections.Generic;
using Phac.Scriptable;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

namespace Phac
{
    public class ProjectileManager : Utility.Singleton<ProjectileManager> {
        [SerializeField] private int DefaultCapacity;
        [SerializeField] private GameObject BulletPrefab;
        [SerializeField] private BulletConfig[] BulletConfigs;
        [SerializeField] private LayerMask CollisionMask;
        
        private ObjectPool<BulletController> m_BulletPool;
        private readonly List<BulletController> m_ActiveBullets = new List<BulletController>();
        private readonly List<BulletController> m_BulletsToReturn = new List<BulletController>();

        protected override void Awake()
        {
            base.Awake();
            m_BulletPool = new ObjectPool<BulletController>(
                createFunc: () => {
                    GameObject obj = Instantiate(BulletPrefab,  transform.position, transform.rotation);
                    obj.SetActive(false);
                    return obj.GetOrAddComponent<BulletController>();;
                },
                actionOnGet: bullet => bullet.gameObject.SetActive(true),
                actionOnRelease: bullet => bullet.gameObject.SetActive(false),
                actionOnDestroy: bullet => {
                    if (bullet) Destroy(bullet);
                },
                defaultCapacity: DefaultCapacity,
                collectionCheck: false,
                maxSize: DefaultCapacity * 100

            );
        }

        private void Update()
        {
            foreach (BulletController bullet in m_ActiveBullets)
            {
                if (bullet.Move(CollisionMask, out RaycastHit hit)) {
                   m_BulletsToReturn.Add(bullet); 
                   if (hit.collider != null) {
                        hit.collider.gameObject.GetComponent<Health>().CurrentHP.Value -= 2.0f;
                   }
                }
            }
        }

        private void LateUpdate()
        {
            foreach (BulletController bullet in m_BulletsToReturn)
            {
                m_ActiveBullets.Remove(bullet);
                m_BulletPool.Release(bullet);
            }
            m_BulletsToReturn.Clear();
        }

        public void Spawn(Vector3 position, Quaternion rotation) {
            BulletController bullet = m_BulletPool.Get();
            bullet.Initialize(BulletConfigs[0], position, rotation);
            m_ActiveBullets.Add(bullet);
        }
    }
}