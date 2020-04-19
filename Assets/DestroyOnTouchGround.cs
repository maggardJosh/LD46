using System.Collections;
using System.Collections.Generic;
using Entity.Base;
using UnityEngine;

[RequireComponent(typeof(BaseEntity))]
public class DestroyOnTouchGround : MonoBehaviour
{
    private BaseEntity _entity;

    // Start is called before the first frame update
    void Start()
    {
        _entity = GetComponent<BaseEntity>();
    }

    private float _groundedCount = 0;
    public float timeTillDestroy = 1f;
    void Update()
    {
        if (_entity.LastHitResult.HitDown)
        {
            _groundedCount += Time.deltaTime;
            if(_groundedCount > timeTillDestroy)
                Destroy(gameObject);
        }
    }
}
