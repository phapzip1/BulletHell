using UnityEngine;
using Phac.MyInput;
using Phac.Utility;
using System.Collections.Generic;

namespace Phac.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [Range(0.0f, 100.0f)] public float Speed = 3.0f;
        [Range(0.0f, 100.0f)] public float RotateSpeed = 10.0f;

        private CharacterController m_CharacterController;
        private InputReader m_InputHandler;
        private bool m_Attacking;
        private CountdownTimer m_AttackCooldown;
        private List<Timer> m_Timers;

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_InputHandler = GetComponent<InputReader>();

            m_Attacking = false;

            m_AttackCooldown = new CountdownTimer(0.25f);
            m_Timers = new List<Timer>(1) {
                m_AttackCooldown,
            };
        }

        private void OnEnable()
        {
            m_InputHandler.Attack += Shoot;
        }

        private void OnDisable()
        {
            m_InputHandler.Attack -= Shoot;
        }

        private void Update()
        {
            /// Move 
            Vector3 input = new Vector3(m_InputHandler.MoveDirection.x, 0.0f, m_InputHandler.MoveDirection.y).normalized;

            if (m_Attacking && !m_AttackCooldown.IsRunning) {
                ProjectileManager.Instance.Spawn(transform.position, transform.rotation);
                m_AttackCooldown.Reset();
                m_AttackCooldown.Start();
            }

            if (m_InputHandler.ControlScheme == "Keyboard&Mouse")
            {
                Plane plane = new Plane(Vector3.up, transform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (plane.Raycast(ray, out float distance))
                {
                    transform.rotation = Quaternion.LookRotation(ray.GetPoint(distance) - transform.position);
                }
            }
            else
            {
                Vector3 look = new Vector3(m_InputHandler.LookDirection.x, 0.0f, m_InputHandler.LookDirection.y);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(look), Time.fixedDeltaTime * RotateSpeed);
            }

            if (input.magnitude > 0.1f)
            {
                m_CharacterController.Move(Speed * Time.fixedDeltaTime * input);
            }
        }

        void FixedUpdate()
        {
            foreach (Timer timer in m_Timers)
            {
                timer.Tick(Time.fixedDeltaTime);
            }
        }

        private void Shoot(bool performed) => m_Attacking = performed;
    }

}