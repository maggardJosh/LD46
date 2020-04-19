using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;

    [NonSerialized] public GameObject spawnedObject;

    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update()
    {
        if (CameraFunctions.IsOnScreen(transform.position))
            return;

        if (spawnedObject == null)
            spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }

   
}