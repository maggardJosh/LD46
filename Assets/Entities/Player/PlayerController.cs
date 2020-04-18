
    using System;
    using Entities.Player.States;
    using Entity.Base;
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
        }
    }
