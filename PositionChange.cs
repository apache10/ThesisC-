using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChange : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }


    //if touching the collider of the cart
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "MineCart")
            transform.position.Set(transform.position.x, transform.position.y + 0.822f, transform.position.z);
    }


    // if not touching the cart
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "MineCart")
            transform.position.Set(transform.position.x, transform.position.y - 0.822f, transform.position.z);
    }
}
