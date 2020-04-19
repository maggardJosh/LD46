using System.Collections;
using System.Collections.Generic;
using Entity.Base;
using UnityEngine;

[RequireComponent(typeof(BaseEntity))]
[RequireComponent(typeof(SpriteRenderer))]
public class PickupDiscard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DiscardPickup(Sprite pickupSprite)
    {
        GetComponent<SpriteRenderer>().sprite = pickupSprite;
        GetComponent<BaseEntity>().SetVelocity(new Vector3(Random.Range(0,2) == 1 ? -3f : 3f, 5, 0));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
