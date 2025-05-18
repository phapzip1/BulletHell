using Phac.Controller;
using UnityEngine;
using UnityEngine.AI;

namespace Phac.State
{
    public abstract class BaseEnemyState : IState
    {
        protected readonly EnemyController Controller;

        public BaseEnemyState(EnemyController controller)
        {
            Controller = controller;
        }
        protected const float CrossFadeDuration = 0.1f;

        public virtual void FixedUpdate()
        {
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual void Update()
        {
        }
    }

    public class EnemyChaseState : BaseEnemyState
    {
        private readonly NavMeshAgent m_Agent;
        private readonly Transform m_Player;

        public EnemyChaseState(EnemyController controller, NavMeshAgent agent, Transform player) : base(controller)
        {
            m_Agent = agent;
            m_Player = player;
        }

        public override void OnEnter()
        {
            // Setup animation if any
        }

        public override void Update()
        {
            m_Agent.SetDestination(m_Player.transform.position);
        }
    }

    public class EnemyAttackState : BaseEnemyState
    {
        private readonly NavMeshAgent m_Agent;
        private readonly Transform m_Player;

        public EnemyAttackState(EnemyController controller, NavMeshAgent agent, Transform Player) : base(controller)
        {
            m_Agent = agent;
            m_Player = Player;
        }

        public override void OnEnter()
        {
            m_Agent.isStopped = true;
        }

        public override void OnExit()
        {
            m_Agent.isStopped = false;
        }

        public override void Update()
        {
            if (Vector3.Angle(Controller.transform.forward, m_Player.forward) > 5.0f)
            {
                Quaternion newRotation = Quaternion.LookRotation(m_Player.position - Controller.transform.position);
                Controller.transform.rotation = Quaternion.Lerp(Controller.transform.rotation, newRotation, Time.deltaTime * 10.0f);
            }


            if (Controller.AttackCooldown.IsFinished)
            {
                Controller.Attack();
                Controller.AttackCooldown.Reset();
                Controller.AttackCooldown.Start();
            }
        }
    }
}