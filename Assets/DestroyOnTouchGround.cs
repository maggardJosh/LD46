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
        _sRend = GetComponent<SpriteRenderer>();
    }

    private float _groundedCount = 0;
    public float timeTillDestroy = 1f;
    private SpriteRenderer _sRend;
    public bool flash = true;

    void Update()
    {
        if (_entity.LastHitResult.HitDown)
        {
            _groundedCount += Time.deltaTime;
            if (flash)
            {
                var c = _sRend.color;
                if ((int) (_groundedCount * 1000) % 125 < 62)
                    c.a = .2f;
                else
                    c.a = 1f;
                _sRend.color = c;
            }

            if (_groundedCount > timeTillDestroy)
                Destroy(gameObject);
        }
    }
}