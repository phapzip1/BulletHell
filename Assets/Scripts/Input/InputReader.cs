using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Phac.MyInput
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] PlayerInput PlayerInput;

        public Vector2 MoveDirection { get; private set; }
        public Vector2 LookDirection { get; private set; }
        public event UnityAction<bool> Attack = delegate { };

        public string ControlScheme
        {
            get => PlayerInput.currentControlScheme ?? "Undefined";
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            MoveDirection = ctx.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext ctx)
        {
            LookDirection = ctx.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                Attack.Invoke(true);
            }
            else if (ctx.canceled)
            {
                Attack.Invoke(false);
            }
        }
    }

}