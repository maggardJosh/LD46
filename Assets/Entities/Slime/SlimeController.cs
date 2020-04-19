using System;
using System.Collections.Generic;
using Entities.Player;
using Entity.Base;
using UnityEngine;

namespace Entities.Slime
{
    [RequireComponent(typeof(BaseEntity))]
    public class SlimeController : MonoBehaviour, IDamageable
    {
        [NonSerialized] public BaseEntity Entity;
        public SlimeSettings settings;

        void Start()
        {
            Entity = GetComponent<BaseEntity>();
            _collider = GetComponent<BoxCollider2D>();
        }

        private SlimeState _state;
        private BoxCollider2D _collider;


        public void SetState(SlimeState newState)
        {
            _state = newState;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(!CameraFunctions.IsOnScreen(transform.position))
                return;
            if (_state == null)
                _state = new IdleSlimeState(this);
            _state.FixedUpdate();
            CheckDamagePlayer();
        }

        private void CheckDamagePlayer()
        {
            foreach (var h in Entity.LastHitResult.HorizontalHits)
            {
                var pc = h.collider.GetComponent<PlayerController>();
                if (pc == null)
                    continue;
                pc.TakeDamage(this);
            }
            
            foreach (var h in Entity.LastHitResult.VerticalHits)
            {
                var pc = h.collider.GetComponent<PlayerController>();
                if (pc == null)
                    continue;
                if(Entity.LastHitResult.HitDown)
                    pc.TakeDamage(this);
                else
                    pc.Entity.SetYVelocity(Mathf.Max(7,Mathf.Abs(pc.Entity.GetYVelocity())));
            }
        }

        public void TakeDamage(MonoBehaviour damager)
        {
            if (damager is PlayerController)
            {
                Destroy(gameObject);
                AudioManager.PlayOneShot(AudioClips.Instance.HitEnemy);
            }
        }
    }
}