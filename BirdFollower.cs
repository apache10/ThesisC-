using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class BirdFollower : MonoBehaviour
{
    public PathCreator pathCreator;
    public float speed = 10;
    private Animator anim;
    float distanceTravelled;
    bool flyAway = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if ((Time.time > 16 && (pathCreator.path.length*0.6) > (distanceTravelled+0.5))||
            flyAway)
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
            anim.SetBool("flying", true);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "LeftHand" || collision.gameObject.name == "RightHand")
        {
            flyAway = true;
        }
    }
}
