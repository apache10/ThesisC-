using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Follower : MonoBehaviour
{
    public PathCreator pathCreator;
    public float speed = 5;
    float distanceTravelled;
    private bool move = true;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > 17 && Time.time < 22)
        {
            move = false;
        }
        else
        {
            move = true;
        }
        if (move)
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
        }
        
    }
}
