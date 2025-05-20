using System.Collections.Generic;
using Phac.Utility;
using UnityEngine;
using UnityEngine.AI;
using Phac.State;
using Phac.UI;

namespace Phac.Controller
{
    public class EnemyController : MonoBehaviour
    {
        private NavMeshAgent m_Agent;
        private EnemyDetector m_EnemyDetector;
        private StateMachine m_StateMachine;
        private List<Timer> m_Timers;
        public CountdownTimer AttackCooldown { get; private set; }

        private void Awake()
        {
            m_Agent = GetComponent<NavMeshAgent>();
            m_EnemyDetector = GetComponent<EnemyDetector>();

            AttackCooldown = new CountdownTimer(0.5f);
            m_Timers = new List<Timer>(1)
            {
                AttackCooldown,
            };

            SetupStateMachine();
        }

        private void FixedUpdate()
        {
            foreach (Timer timer in m_Timers)
            {
                timer.Tick(Time.fixedDeltaTime);
            }
            m_StateMachine.FixedUpdate();
        }


        private void Update() => m_StateMachine.Update();

        public void Initialize()
        {
            AttackCooldown.Reset();
        }

        private void SetupStateMachine()
        {
            m_StateMachine = new StateMachine();

            EnemyAttackState attack = new EnemyAttackState(this, m_Agent, m_EnemyDetector.PlayerTransform);
            EnemyChaseState chase = new EnemyChaseState(this, m_Agent, m_EnemyDetector.PlayerTransform);

            m_StateMachine.At(chase, attack, new FuncPredicate(() => m_EnemyDetector.CanAttackPlayer()));
            m_StateMachine.At(attack, chase, new FuncPredicate(() => !m_EnemyDetector.CanAttackPlayer()));
            m_StateMachine.SetState(chase);
        }


        public void Attack()
        {
            
        }
    }
}