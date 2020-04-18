using UnityEngine;

namespace Entities.Player.States
{
    public abstract class PlayerState
    {
        protected PlayerController Controller;

        protected PlayerState(PlayerController controller)
        {
            Controller = controller;
        }

        protected float timeInState = 0;

        public void HandleUpdate()
        {
            HandleUpdateInternal();
        }

        public void HandleFixedUpdate()
        {
            timeInState += Time.fixedDeltaTime;
            HandleFixedUpdateInternal();
        }
        protected abstract void HandleUpdateInternal();
        protected abstract void HandleFixedUpdateInternal();


        public void OnExitState()
        {
            HandleExit();
        }
        protected virtual void HandleExit() { }

        public void OnEnterState()
        {
            HandleEnter();
        }

        protected virtual void HandleEnter() { }
    }
}