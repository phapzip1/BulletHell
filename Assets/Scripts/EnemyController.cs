using System.Collections.Generic;
using Phac.Utility;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Phac.Controller
{
    public enum EnemyState
    {
        None,
        Idle,
        Running,
        Attacking
    }
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Transform Player;
        [SerializeField] private LayerMask PlayerMask;
        public float AttackRadius = 10.0f;
        private NavMeshAgent m_Agent;
        private EnemyState m_State;
        private List<Timer> m_Timers;
        private CountdownTimer m_AttackCooldown;

        private void Awake()
        {
            m_Agent = GetComponent<NavMeshAgent>();
            m_Agent.stoppingDistance = AttackRadius;
            m_State = EnemyState.None;
            m_AttackCooldown = new CountdownTimer(0.5f);
            m_Timers = new List<Timer>(1)
            {
                m_AttackCooldown,
            };
        }

        private void OnDisable()
        {
            m_State = EnemyState.None;
        }

        private void OnEnable()
        {
            m_State = EnemyState.Idle;
        }

        private void FixedUpdate()
        {
            foreach (Timer timer in m_Timers)
            {
                timer.Tick(Time.fixedDeltaTime);
            }
        }

        private void Update()
        {

            if (m_Agent.destination != Player.position)
            {
                m_Agent.ResetPath();
                m_Agent.SetDestination(Player.position);
            }

            switch (m_State)
            {
                case EnemyState.Idle:
                    if (m_Agent.remainingDistance <= m_Agent.stoppingDistance)
                    {
                        m_State = EnemyState.Attacking;
                    }
                    else
                    {
                        m_State = EnemyState.Running;
                    }
                    break;
                case EnemyState.Running:
                    if (m_Agent.remainingDistance <= m_Agent.stoppingDistance)
                    {
                        m_State = EnemyState.Attacking;
                    }
                    break;
                case EnemyState.Attacking:
                    if (m_Agent.remainingDistance > m_Agent.stoppingDistance)
                    {
                        m_State = EnemyState.Running;
                    }
                    else
                    {
                        Vector3 lookDir = Player.position - transform.position;

                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * m_Agent.angularSpeed);

                        // Debug.DrawLine(point, point + transform.forward, Color.red);
                        if (m_AttackCooldown.IsFinished)
                        {
                            ProjectileManager.Instance.Spawn(transform.position + 0.8f * transform.forward, transform.rotation);
                            m_AttackCooldown.Reset();
                            m_AttackCooldown.Start();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

    }
}