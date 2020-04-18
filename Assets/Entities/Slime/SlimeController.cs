using System;
using Entity.Base;
using UnityEngine;

namespace Entities.Slime
{
    [RequireComponent(typeof(BaseEntity))]
    public class SlimeController : MonoBehaviour
    {
        [NonSerialized] public BaseEntity Entity;
        public SlimeSettings settings;

        void Start()
        {
            Entity = GetComponent<BaseEntity>();
        }

        private SlimeState _state;
       

        public void SetState(SlimeState newState)
        {
            _state = newState;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (_state == null)
                _state = new IdleSlimeState(this);
            _state.FixedUpdate();
        }
    }
}