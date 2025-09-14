using System;
using UnityEngine;

namespace Inputs
{
    public interface IInputTransfer
    {
        public event Action OnTouchEvent;
        public event Action OnLetGoEvent;

        public Vector2 TouchPosition { get;}

        public void EnablePlayerInput();
        public void DisablePlayerInput();
    }
}
