using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Player : MonoBehaviour
{
    private string moveInputAxis = "Vertical";
    private string turnInputAxis = "Horizontal";

    public float rotationRate = 360;
    public float moveSpeed = 10;

    private Rigidbody rb;
    public SteamVR_Action_Vector2 Movement;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 touchpadValue = Movement.GetAxis(SteamVR_Input_Sources.Any);

        if (touchpadValue != Vector2.zero){
            float moveAxis = touchpadValue.y;
            float turnAxis = touchpadValue.x;
            ApplyInput(moveAxis, turnAxis);
        }

        else
        {
            float moveAxis = Input.GetAxis(moveInputAxis);
            float turnAxis = Input.GetAxis(turnInputAxis);

            ApplyInput(moveAxis, turnAxis);
        }
    }

    private void ApplyInput(float moveInput,
                            float turnInput)
    {
        Move(moveInput);
        Turn(turnInput);
    }

    private void Move(float input)
    {
        //transform.Translate(Vector3.forward * input * moveSpeed);
        rb.AddForce(transform.forward * input * moveSpeed, ForceMode.Force);

    }

    private void Turn(float input)
    {
        transform.Rotate(0, input * rotationRate * Time.deltaTime, 0);
    }

    

}

