using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battlehub.MeshDeformer2;
using RootMotion.FinalIK;

public class Controller : MonoBehaviour
{
    public AudioSource movingSound;

    public SplineFollow splineFollowCart;
    public SplineFollow splineFollowCam;

    private Vector3[] locationsToStop = { new Vector3(281.5278f,22.3434f,113.615f),
        new Vector3(273.3073f, 21.47905f, 164.1747f), new Vector3(300.2364f,21.65816f,222.7851f),
        new Vector3(293.2451f, 24.09601f, 259.4263f), new Vector3(303.116f, 25.53071f, 269.1382f), 
        new Vector3(337.0114f, 21.76785f, 260.5444f),
        new Vector3(342.584f, 21.56022f, 254.479f), new Vector3(344.5617f, 21.54091f, 229.1817f),
        new Vector3(301.1013f, 21.32434f, 188.3435f), new Vector3(334.7188f, 20.76489f, 111.8791f),
        new Vector3(368.385f, 23.765f, 100.3236f), new Vector3(370.243f, 27.3774f, 77.56998f),
        new Vector3(355.9366f,25.17817f,61.26697f),new Vector3(322.4761f,20.39331f,36.90228f),
        new Vector3(309.7224f,20.33245f,43.5674f),new Vector3(275.2621f,22.22f,57.19666f)};
    int n;

    private void Start()
    {
        n = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (n < locationsToStop.Length && V3Equal(transform.position,locationsToStop[n]))
        {
            StartCoroutine(startStop());
            n++;
        }
    }

    IEnumerator startStop()
    {
        StopMoving();
        yield return new WaitForSeconds(5);
        if (n == locationsToStop.Length)
            yield break;
        StartMoving();
    }

    private void StartMoving()
    {
        splineFollowCart.IsRunning = true;
        splineFollowCam.IsRunning = true;
        movingSound.Play();
    }

    private void StopMoving()
    {
        splineFollowCart.IsRunning = false;
        splineFollowCam.IsRunning = false;
        movingSound.Stop();
    }

    public bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.1;
    }
}
