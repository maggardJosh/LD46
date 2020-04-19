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
            if (_state == null)
                _state = new IdleSlimeState(this);
            _state.FixedUpdate();
            CheckDamagePlayer();
        }

        private void CheckDamagePlayer()
        {
             var results = new List<Collider2D>();
             var numHits =
                 _collider.OverlapCollider(
                     new ContactFilter2D {useLayerMask = true, layerMask = settings.damageLayer}, results);
             if (numHits <= 0)
                 return;
             foreach (var hit in results)
             {
                 var damageable = hit.GetComponent<IDamageable>();
                 damageable?.TakeDamage(this);
             }
        }

        public void TakeDamage(MonoBehaviour damager)
        {
            if(damager is PlayerController)
                Destroy(gameObject);
        }
    }
}