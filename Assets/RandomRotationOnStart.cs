using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotationOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
