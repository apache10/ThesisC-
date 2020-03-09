using Battlehub.MeshDeformer2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startStop : MonoBehaviour
{
    SplineFollow splineFollowCart;
    SplineFollow splineFollowCam;
    // Start is called before the first frame update
    void Start()
    {
        splineFollowCart = GameObject.Find("MineCart").GetComponent<SplineFollow>();
        splineFollowCam = GameObject.Find("[CameraRig]").GetComponent<SplineFollow>();
}

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        print(collider.gameObject.name);
        if (collider.gameObject.name == "LeftHand" || collider.gameObject.name == "RightHand")
        {
            splineFollowCart.IsRunning = !splineFollowCart.IsRunning;
            splineFollowCam.IsRunning = !splineFollowCam.IsRunning;
        }
    }
}
