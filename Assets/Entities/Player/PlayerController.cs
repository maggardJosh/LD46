
    using System;
    using Entities.Player.States;
    using Entity.Base;
    using Pickups;
    using UnityEngine;

    namespace Entities.Player
    {
        [RequireComponent(typeof(BaseEntity))]
        [RequireComponent(typeof(Animator))]
        [RequireComponent(typeof(DebugLoggerBehavior))]
        public class PlayerController : MonoBehaviour
        {
            [NonSerialized] public BaseEntity Entity;
            [NonSerialized] public PlayerAnimatorController AnimController;
            private IPlayerInputProvider _inputProvider;
            [NonSerialized] public SpriteRenderer SRend;
            [NonSerialized] public DebugLoggerBehavior Logger;

            public bool CanVine = true;
            public bool CanSlam = true;
            public bool CanDoubleJump = true;
            
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

                carryingPickupType = global::Pickups.Pickup.PickupType.None;
                pickupSpriteRendered.sprite = null;
                pickupSpriteRendered.gameObject.SetActive(false);
                
                FindObjectOfType<Home>()?.UpdatePlants();
            }
        }
    }
