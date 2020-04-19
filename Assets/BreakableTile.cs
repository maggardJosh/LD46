using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableTile : MonoBehaviour
{
    public GameObject objectToDestroy;

    private void OnDestroy()
    {
        Destroy(objectToDestroy.gameObject);
    }
}
