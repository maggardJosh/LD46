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
        public EntitySettings invulnSettings;

        [NonSerialized] public PlayerAnimatorController AnimController;
        private IPlayerInputProvider _inputProvider;
        [NonSerialized] public SpriteRenderer SRend;
        [NonSerialized] public DebugLoggerBehavior Logger;

        public bool CanVine = true;
        public bool CanSlam = true;
        public bool CanDoubleJump = true;

        public BoxCollider2D AttackCollider;
        public BoxCollider2D AttackUpCollider;
        public BoxCollider2D AttackSlamCollider;
        
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
            if (_inputProvider == null)
                _inputProvider = new UnityPlayerInputProvider();
            Logger = GetComponent<DebugLoggerBehavior>();
            if (_playerState == null)
                SetPlayerState(new DefaultPlayerState(this));
        }

        public PlayerInput LastInput { get; private set; } = new PlayerInput();
        public PlayerInput CurrentInput { get; private set; } = new PlayerInput();
        public bool CarryingPickup => carryingPickupType != Pickups.Pickup.PickupType.None;

        [NonSerialized] public float AttackBuffered = float.MaxValue;
        private void Update()
        {
            LastInput = CurrentInput;
            CurrentInput = _inputProvider.GetInput();
            
            if (AttackBuffered < settings.attackBufferLength)
                AttackBuffered += Time.fixedDeltaTime;
            if (CurrentInput.VineInput && !LastInput.VineInput)
                AttackBuffered = 0;
            
            _playerState.HandleUpdate();
        }

        private PlayerState _playerState;

        public void SetPlayerState(PlayerState newPlayerState)
        {
            if (newPlayerState == null)
                return;
            _playerState?.OnExitState(newPlayerState);
            _playerState = newPlayerState;
            _playerState.OnEnterState();
        }

        private void FixedUpdate()
        {
            if (invulnCount > 0)
            {
                invulnCount -= Time.fixedDeltaTime;
                var c = SRend.color;
                if ((int) (invulnCount * 1000) % 125 < 62)
                    c.a = .2f;
                else
                    c.a = 1f;
                SRend.color = c;
                if (!(_playerState is StunPlayerState))
                {
                    Entity.settings = invulnSettings;
                }
            }
            else
            {
                var c = SRend.color;
                c.a = 1f;
                SRend.color = c;
                if (!(_playerState is StunPlayerState))
                {
                    Entity.settings = normalSettings;
                }
            }
            
            if (Entity.LastHitResult.HitDown && _lastFrameYVel < -2f)
            {
                
                AudioManager.PlayOneShot(AudioClips.Instance.Land);
            }

            _lastFrameYVel = Entity.GetYVelocity();


            _playerState.HandleFixedUpdate();

            foreach (var hit in Entity.LastHitResult.VerticalHits)
            {
                var sc = hit.collider.GetComponent<SlimeController>();
                if (sc != null)
                {
                    if (Entity.LastHitResult.HitDown)
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

        private Pickup.PickupType carryingPickupType = Pickups.Pickup.PickupType.None;

        public void Pickup(Pickup p)
        {
            carryingPickupType = p.pickupType;
            pickupSpriteRendered.gameObject.SetActive(true);
            pickupSpriteRendered.sprite = p.pickupSprite;
            
            TutorialManager.Instance.SetTutorialStep(TutorialManager.TutorialStep.ReturnPickup);
        }

        public bool CanPickup(Pickup.PickupType pickupType)
        {
            switch (pickupType)
            {
                case Pickups.Pickup.PickupType.None:
                    return false;
                case Pickups.Pickup.PickupType.Slam:
                    return !CanSlam;
                case Pickups.Pickup.PickupType.Vine:
                    return !CanVine;
                case Pickups.Pickup.PickupType.DoubleJump:
                    return !CanDoubleJump;
            }

            return false;
        }

        public void ApplyPickup()
        {
            switch (carryingPickupType)
            {
                case Pickups.Pickup.PickupType.None:
                    break;
                case Pickups.Pickup.PickupType.Slam:
                    CanSlam = true;
                    TutorialManager.Instance.SetTutorialStep(TutorialManager.TutorialStep.Slam);
                    break;
                case Pickups.Pickup.PickupType.Vine:
                    CanVine = true;
                    TutorialManager.Instance.SetTutorialStep(TutorialManager.TutorialStep.Vine);
                    break;
                case Pickups.Pickup.PickupType.DoubleJump:
                    CanDoubleJump = true;
                    TutorialManager.Instance.SetTutorialStep(TutorialManager.TutorialStep.DoubleJump);
                    break;
            }

            AudioManager.PlayOneShot(AudioClips.Instance.PlantFlower);
            ClearCarriedPickup();

            FindObjectOfType<Home>()?.UpdatePlants();
        }

        private void ClearCarriedPickup()
        {
            carryingPickupType = Pickups.Pickup.PickupType.None;
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
        
        public void TryDamageAttackSlam()
        {
            var results = new List<Collider2D>();
            var numHits =
                AttackSlamCollider.OverlapCollider(
                    new ContactFilter2D {useLayerMask = true, layerMask = settings.damageLayer}, results);
            if (numHits <= 0)
                return;
            foreach (var hit in results)
            {
                var damageable = hit.GetComponent<IDamageable>();
                damageable?.TakeDamage(this);
            }
        }

        private float invulnCount = 0f;
        private double _lastFrameYVel;

        public void TakeDamage(MonoBehaviour damager)
        {
            if (invulnCount > 0)
                return;
            if (damager == this)
                return;
            if (_playerState is StunPlayerState || _playerState is SlamPlayerState)
                return;
            var damageVect = (transform.position + Entity.BoxCollider.offset.ToVector3()) - damager.transform.position +
                             Vector3.up;
            damageVect = damageVect.normalized * settings.stunVelocityStrength;
            SetPlayerState(new StunPlayerState(this, damageVect));
            invulnCount = settings.InvulnerabilityTime;
            if (CarryingPickup)
            {
                SpawnPickupDiscard();
                ClearCarriedPickup();
            }
        }

        private void SpawnPickupDiscard()
        {
            var go = Instantiate(settings.PickupDiscardPrefab, pickupSpriteRendered.transform.position,
                Quaternion.identity);
            go.GetComponent<PickupDiscard>().DiscardPickup(pickupSpriteRendered.sprite);
        }
        
        public void PlayStepSound()
        {
            AudioManager.PlayOneShot(AudioClips.Instance.Step);
        }

        public void PlaySwingSound()
        {
            AudioManager.PlayOneShot(AudioClips.Instance.Swing);
        }
    }
}