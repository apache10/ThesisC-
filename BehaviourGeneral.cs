using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class BehaviourGeneral : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update

    private bool isWandering = false;
    //private bool turnBias = false;
    //private bool correctDirection = false;
    //private Vector3 midPoint;
    private float prevTime = 0f;
    private string randNextAni = "";
    public bool collided = false;
    private bool currently2Legs = false;

    public List<AnimationClip> animations;
    public List<AnimationClip> reactAnimations;
    public new AudioSource[] audio;

    void Start()
    {
        animator = GetComponent<Animator>();
        //midPoint = new Vector3((endX - startX) / 2 + startX, transform.position.y, (endZ - startZ) / 2 + startZ);
    }

    // Update is called once per frame
    void Update()
    {
        //Wander();
        if (isWandering == false)
        {
            StartCoroutine(Wander());
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Hand"))
        {
            collided = true;
        }
    }

    IEnumerator Wander()
    {
        int randSteps;
        string randNextAni;
        randSteps = Random.Range(500, 2500);
        randNextAni = nextAni();
        if (randNextAni == "GoToRest") { randSteps = 10000; }
        else if (randNextAni.Contains("2Legs")) { currently2Legs = true; }

        float startTime = Time.time;
        isWandering = true;

        // if position not in range or collision, stop the wait halfway
        animator.SetBool("is" + randNextAni, true);
        
        yield return new WaitUntil(() => {
            // if collision with hands
            if (collided)
            {
                return true;
            }
            // if time reached
            else if (randSteps / 1000 <= Time.time - startTime)
                return true;

            return false;
        });
        animator.SetBool("is" + randNextAni, false);
        foreach (AudioSource audioSource in audio)
        {
            audioSource.Stop();
        }

        isWandering = false;
    }


    string nextAni()
    {
        int index;
        if (collided)
        {
            index = Random.Range(0, reactAnimations.Count);
            if (currently2Legs)
            {
                while (!reactAnimations[index].name.Contains("2Legs"))
                {
                    index = Random.Range(0, reactAnimations.Count);
                }
            }
            collided = false;
            currently2Legs = false;
            return reactAnimations[index].name;
        }
        else
        {
            index = Random.Range(0, animations.Count);
            return animations[index].name;
        }
    }

    //    void OnAnimatorMove()
    //    {
    //        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Gallop_RM"))
    //        {
    //            if (transform.position.z > 227.58)
    //            {
    //                animator.SetBool("isGalloping", false);
    //            }
    //        }
    //   }

    void playSound(int index)
    {
        audio[index].Play();
    }
}
