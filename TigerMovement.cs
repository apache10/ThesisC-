using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class TigerMovement : MonoBehaviour
{
    public PathCreator pathCreator;
    public float speed = 5;
    private Animator anim;
    float distanceTravelled;

    void Start()
    {
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo curr_state = anim.GetCurrentAnimatorStateInfo(0);
        if (curr_state.IsName("Base Layer.walk"))
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);

        }

        if (curr_state.IsName("Base Layer.run"))
        {
            speed = 8;
            distanceTravelled += speed * Time.deltaTime;
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);

        }
    }
}
