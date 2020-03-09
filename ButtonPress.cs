using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battlehub.MeshDeformer2;
using RootMotion.FinalIK;

public class ButtonPress : MonoBehaviour
{



    public BoxTracer bt;
    public float returnSpeed;

    public float activationDistance;
    public AudioSource movingSound;

    public SplineFollow splineFollowCart;
    public SplineFollow splineFollowCam;
    public VRIK VRIKHuman;


    BoxCollider bc;

    protected bool pressed = false;

    protected bool released = false;

    protected Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        // Remember start position of button

        startPosition = transform.localPosition;
        bc = GetComponent<BoxCollider>();

        //splineFollowCart = GameObject.Find("MineCart").GetComponent<SplineFollow>();
        //splineFollowCam = GameObject.Find("[CameraRig]").GetComponent<SplineFollow>();
        //VRIKHuman = GameObject.Find("HumanPlayer").GetComponent<VRIK>();
    }
    void Update()
    {

        released = false;


        if (bt.colliding) {
            // Use local position instead of global, so button can be rotated in any direction
            
            Vector3 localPos = transform.localPosition;
            localPos.y = bt.height - bc.size.y/2;

            transform.localPosition = localPos;
        }
        // Return button to startPosition
        else {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, Time.deltaTime * returnSpeed);
        }
        
        //Get distance of button press. Make sure to only have one moving axis.

        Vector3 allDistances = transform.localPosition - startPosition;

        float pressComplete = Mathf.Clamp(1 / activationDistance * Math.Abs(allDistances.y), 0f, 1f);



        //Activate pressed button

        if (pressComplete >= 0.7f && !pressed)

        {

            pressed = true;
        }

        //Dectivate unpressed button

        else if (pressComplete <= 0.2f && pressed)

        {

            pressed = false;

            released = true;

            splineFollowCart.IsRunning = !splineFollowCart.IsRunning;
            splineFollowCam.IsRunning = !splineFollowCam.IsRunning;
            if (splineFollowCart.IsRunning) { movingSound.Play(); }else { movingSound.Stop(); }
            //if (VRIKHuman.solver.locomotion.weight == 0)
            //{
                //VRIKHuman.solver.locomotion.weight = 1;
            //}
            //else
                //VRIKHuman.solver.locomotion.weight = 0;

        }

    }




}
