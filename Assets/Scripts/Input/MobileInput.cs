using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInput;

namespace Inputs
{
    public class MobileInput : IMobileActions, IInputTransfer
    {
        public event Action OnTouchEvent;
        public event Action OnLetGoEvent;

        public Vector2 TouchPosition { get; private set; }

        public PlayerInput _inputActions;

        public void EnablePlayerInput()
        {
            if (_inputActions == null)
            {
                _inputActions = new PlayerInput();
                _inputActions.Mobile.SetCallbacks(this);
            }

            _inputActions.Enable();
        }

        public void DisablePlayerInput()
        {
            _inputActions.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            TouchPosition = context.ReadValue<Vector2>();
        }

        public void OnTouch(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    OnTouchEvent?.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    OnLetGoEvent?.Invoke();
                    break;
            }
        }
    }
}
