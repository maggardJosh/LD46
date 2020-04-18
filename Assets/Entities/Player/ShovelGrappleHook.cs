using Entities.Player.States;
using Entity.Base;
using UnityEngine;

namespace Entities.Player
{
    [RequireComponent(typeof(BaseEntity))]
    public class ShovelGrappleHook : MonoBehaviour
    {
        public float speed = 5f;

        public float length = 5f;

        private Vector3 _startPos;

        public delegate void ReturnHook(bool hitSomething);

        public event ReturnHook OnReturnHook ;

        void Start()
        {
            _baseEntity = GetComponent<BaseEntity>();
            _startPos = transform.position;
        }
        
        private BaseEntity _baseEntity;
        private PlayerController _controller;
        public SpriteRenderer vine;
        
        private bool _hasBeenActivated = false;
        public void ShootGrapple(PlayerController controller)
        {
            _controller = controller;
            _hasBeenActivated = true;
        }

        void Update()
        {
            Vector3 diff = transform.position - _controller.transform.position;
            var s = vine.size;
            s.x = diff.magnitude;
            vine.size = s;
            _baseEntity.SetVelocity(Vector3.zero);
            if (!_hasBeenActivated)
                return;
            
            _baseEntity.SetVelocity(transform.rotation * Vector3.right * speed);

            if (_baseEntity.LastHitResult.HitAny)
            {
                OnReturnHook?.Invoke(true);
                _hasBeenActivated = false;
                return;
            }

            var dist = (_baseEntity.transform.position - _startPos).sqrMagnitude;
            if (dist > length * length)
            {
                OnReturnHook?.Invoke(false);
                _hasBeenActivated = false;
            }
        }

       
    }  
} 
 