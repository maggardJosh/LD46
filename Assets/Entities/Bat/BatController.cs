using Entities.Player;
using Entity.Base;
using UnityEngine;

namespace Entities.Bat
{
    
    [RequireComponent(typeof(BaseEntity))]
    [RequireComponent(typeof(Animator))]
    public class BatController : MonoBehaviour, IDamageable
    {
        private PlayerController _pc;

        public BatSettings settings;

        private BoxCollider2D _collider;
        [SerializeField] private PolygonCollider2D sightCollider;
        
        private bool _seenPlayer = false;
        private Animator _animator;
        private static readonly int Active = Animator.StringToHash("Active");
        private BaseEntity _entity;

        // Start is called before the first frame update
        void Start()
        {
            _pc = FindObjectOfType<PlayerController>();
            _collider = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
            _entity = GetComponent<BaseEntity>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(!CameraFunctions.IsOnScreen(transform.position))
                return;
                
            CheckDamagePlayer();

            if(!_seenPlayer)
                CheckSeenPlayer();
            
            if(_seenPlayer)
                MoveTowardsPlayer();
        }

        private void CheckSeenPlayer()
        {
            _seenPlayer = sightCollider.bounds.Intersects(_pc.Entity.BoxCollider.bounds);
            if(_seenPlayer)
                AudioManager.PlayOneShot(AudioClips.Instance.BatSeePlayer);
            _animator.SetBool(Active, _seenPlayer);
        }

        public void PlayFlapSound()
        {
            AudioManager.PlayOneShot(AudioClips.Instance.BatFlap);
        }

        private void MoveTowardsPlayer()
        {
            Vector3 diff = _pc.transform.position - transform.position;
            _entity.AddToVelocity(diff.normalized * settings.speed);
        }

        private void CheckDamagePlayer()
        {
            if (_collider.bounds.Intersects(_pc.Entity.BoxCollider.bounds))
            {
                _pc.TakeDamage(this);
                Vector3 diff = _pc.transform.position - transform.position;
                _entity.SetVelocity(-diff * settings.damageBounceValue);
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
