using Phac.Utility;
using UnityEngine;
using UnityEngine.Pool;

namespace Phac.Spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject EnemyPrefab;
        [SerializeField] private Vector2[] SpawnPoints;

        private IObjectPool<GameObject> m_Pool;

        private CountdownTimer m_SpawnTimer;

        private void Awake()
        {
            m_Pool = new ObjectPool<GameObject>(
                OnCreate,
                OnGet,
                OnReturn,
                OnDispose
            );
            m_SpawnTimer = new CountdownTimer(5.0f);
        }

        private void Update()
        {
            if (m_SpawnTimer.IsFinished)
            {
                int index = Random.Range(0, SpawnPoints.Length);

                GameObject enemy = m_Pool.Get();

                enemy.transform.position = new Vector3(SpawnPoints[index].x, 0.0f, SpawnPoints[index].y);
                m_SpawnTimer.Reset();
                m_SpawnTimer.Start();
            }
        }

        private void FixedUpdate()
        {
            m_SpawnTimer.Tick(Time.fixedDeltaTime);
        }

        #region Helpers and Methods
        private GameObject OnCreate()
        {
            GameObject gameObj = Instantiate(EnemyPrefab, transform.position, transform.rotation);
            gameObj.SetActive(false);
            return gameObj;
        }

        private void OnGet(GameObject instance) => instance.SetActive(true);

        private void OnReturn(GameObject instance) => instance.SetActive(false);

        private void OnDispose(GameObject instance) => Destroy(instance);

        #endregion
    }
}