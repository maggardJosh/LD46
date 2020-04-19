
    using System;
    using System.Collections.Generic;
    using Entities.Player.States;
    using Entities.Slime;
    using Entity.Base;
    using Extensions;
    using Pickups;
    using UnityEngine;

    namespace Entities.Player
    {
        [RequireComponent(typeof(BaseEntity))]
        [RequireComponent(typeof(Animator))]
        [RequireComponent(typeof(DebugLoggerBehavior))]
        public class PlayerController : MonoBehaviour, IDamageable
        {
            [NonSerialized] public BaseEntity Entity;

            public EntitySettings normalSettings;
            public EntitySettings stunSettings;
            
            [NonSerialized] public PlayerAnimatorController AnimController;
            private IPlayerInputProvider _inputProvider;
            [NonSerialized] public SpriteRenderer SRend;
            [NonSerialized] public DebugLoggerBehavior Logger;

            public bool CanVine = true;
            public bool CanSlam = true;
            public bool CanDoubleJump = true;

            public BoxCollider2D AttackCollider;
            public BoxCollider2D AttackUpCollider;
            
            public SpriteRenderer pickupSpriteRendered;

            public GameObject doubleJumpEffect;

            public GameObject grappleHookAnchor;
            public Transform grappleHookUpAnchor;

            public PlayerControllerSettings settings;
            
            private void Start()
            {
                Entity = GetComponent<BaseEntity>();
                AnimController = new PlayerAnimatorController(GetComponent<Animator>());
                SRend = GetComponentInChildren<SpriteRenderer>();
                if(_inputProvider == null)
                    _inputProvider = new UnityPlayerInputProvider();
                Logger = GetComponent<DebugLoggerBehavior>();
                if(_playerState == null)
                    SetPlayerState(new DefaultPlayerState(this));
            }

            public PlayerInput LastInput { get; private set; } = new PlayerInput();
            public PlayerInput CurrentInput { get; private set; } = new PlayerInput();
            public bool CarryingPickup => carryingPickupType != global::Pickups.Pickup.PickupType.None;

            private void Update()
            {
                LastInput = CurrentInput;
                CurrentInput = _inputProvider.GetInput();
                _playerState.HandleUpdate();
            }

            private PlayerState _playerState;
            
            public void SetPlayerState(PlayerState newPlayerState)
            {
                if (newPlayerState == null)
                    return;
                _playerState?.OnExitState();
                _playerState = newPlayerState;
                _playerState.OnEnterState();
            }
            
            private void FixedUpdate()
            {
                _playerState.HandleFixedUpdate();

                foreach (var hit in Entity.LastHitResult.VerticalHits)
                {
                    var sc = hit.collider.GetComponent<SlimeController>();
                    if (sc != null)
                    {
                        if(Entity.LastHitResult.HitDown)
                            Entity.SetYVelocity(7f);
                        
                    }
                    
                }
                
                foreach (var hit in Entity.LastHitResult.HorizontalHits)
                {
                    var sc = hit.collider.GetComponent<SlimeController>();
                    if (sc != null)
                    {
                        TakeDamage(sc);
                        
                    }
                    
                }
            }

            private Pickup.PickupType carryingPickupType = global::Pickups.Pickup.PickupType.None;
            public void Pickup(Pickup p)
            {
                carryingPickupType = p.pickupType;
                pickupSpriteRendered.gameObject.SetActive(true);
                pickupSpriteRendered.sprite = p.pickupSprite;
            }

            public bool CanPickup(Pickup.PickupType pickupType)
            {
                switch (pickupType)
                {
                    case global::Pickups.Pickup.PickupType.None:
                        return false;
                    case global::Pickups.Pickup.PickupType.Slam:
                        return !CanSlam;
                    case global::Pickups.Pickup.PickupType.Vine:
                        return !CanVine;
                    case global::Pickups.Pickup.PickupType.DoubleJump:
                        return !CanDoubleJump;
                }

                return false;
            }

            public void ApplyPickup()
            {
                switch (carryingPickupType)
                {
                    case global::Pickups.Pickup.PickupType.None:
                        break;
                    case global::Pickups.Pickup.PickupType.Slam:
                        CanSlam = true;
                        break;
                    case global::Pickups.Pickup.PickupType.Vine:
                        CanVine = true;
                        break;
                    case global::Pickups.Pickup.PickupType.DoubleJump:
                        CanDoubleJump = true;
                        break;
                }

                ClearCarriedPickup();

                FindObjectOfType<Home>()?.UpdatePlants();
            }

            private void ClearCarriedPickup()
            {
                carryingPickupType = global::Pickups.Pickup.PickupType.None;
                pickupSpriteRendered.sprite = null;
                pickupSpriteRendered.gameObject.SetActive(false);
            }

            public void TryDamageAttack()
            {
                var results = new List<Collider2D>();
                var numHits =
                    AttackCollider.OverlapCollider(
                        new ContactFilter2D {useLayerMask = true, layerMask = settings.damageLayer}, results);
                if (numHits <= 0)
                    return;
                foreach (var hit in results)
                {
                    var damageable = hit.GetComponent<IDamageable>();
                    damageable?.TakeDamage(this);
                }
            }
            
            public void TryDamageAttackUp()
            {
                var results = new List<Collider2D>();
                var numHits =
                    AttackUpCollider.OverlapCollider(
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
                if (damager == this)
                    return;
                if (_playerState is StunPlayerState)
                    return;
                var damageVect = (transform.position + Entity.BoxCollider.offset.ToVector3()) - damager.transform.position + Vector3.up;
                damageVect = damageVect.normalized * settings.stunVelocityStrength;
                SetPlayerState(new StunPlayerState(this, damageVect));
                ClearCarriedPickup();
            }
        }
    }
