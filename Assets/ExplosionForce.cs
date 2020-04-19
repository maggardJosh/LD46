using System.Collections;
using System.Collections.Generic;
using Entity.Base;
using UnityEngine;

[RequireComponent(typeof(BaseEntity))]
public class ExplosionForce : MonoBehaviour
{
    
    public float xForce = 5f;
    public float yForceMax = 10f;
    public float yForceMin = 4f;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 explosionVel = new Vector3(Random.Range(-xForce, xForce),
            yForceMin + Random.Range(0, yForceMax - yForceMin), 0);
        GetComponent<BaseEntity>().SetVelocity(explosionVel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
