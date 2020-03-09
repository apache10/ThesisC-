using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTracer : MonoBehaviour
{
    //[HideInInspector]
    public float height;
    public bool colliding;
    public float offset;

    public Transform pushButton;

    private void Update() {
    }
    private void OnTriggerStay(Collider other) {

        if (other.CompareTag("Hand")) height = (other.transform.position.y - other.bounds.extents.y) - transform.position.y -offset;//h=-0.6
        //if (other.CompareTag("Hand")) { height = other.ClosestPointOnBounds(transform.localPosition).y - transform.position.y - offset;} //h=-0.63
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Hand")) colliding = true;
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Hand")) colliding = false;
    }
}
