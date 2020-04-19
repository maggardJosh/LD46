using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotationVel : MonoBehaviour
{
    public RotationSettings settings;
   

    private float _rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        _rotationSpeed = Random.Range(settings.minRotationSpeed, settings.maxRotationSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, _rotationSpeed * 360f * Time.deltaTime);
    }
}