using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class BehaviourAI : MonoBehaviour
{
    Animator animator;
    GameObject mineCart;
    GameObject playerHead;
    // Start is called before the first frame update

    private bool isWandering = false;
    private bool turnBias = false;
    private bool correctDirection = false;
    private Vector3 midPoint;
    private float prevTime = 0f;
    private string randNextAni = "";
    private bool isReactAni = false;
    public bool collidedWithHands = false;
    private bool collidedWithObjectsOrAnimals = false;
//    private bool moveTowardsCart = false;
//    private bool turnTowardsCart = false;
    private bool idleBreathe = true;
    private bool faceToPlayer = false;
    private bool endsWhenAniEnds = false;
    private AnimationClip[] animationClips;
    private List<AnimationClip> normalAnimations = new List<AnimationClip>();
    private List<AnimationClip> turningAnimations = new List<AnimationClip>();
    private List<AnimationClip> reactingAnimations = new List<AnimationClip>();

    public float startX;
    public float startZ;
    public float endX;
    public float endZ;
    public List<AnimationClip> reactAnimations;
//    public AnimationClip runTowardsCartAnimation;
    public List<AnimationClip> movingAniforNoRM;
    public List<AnimationClip> movingLeftRightForNoRM;
    public float movingSpeed;
    public float turningSpeed;
    public new AudioSource[] audio;


    void Start()
    {
        animator = GetComponent<Animator>();
        mineCart = GameObject.Find("MineCart");
        playerHead = GameObject.Find("Camera");

        midPoint = new Vector3((endX - startX)/2 + startX, transform.position.y, (endZ - startZ)/2 + startZ);

        // Get a list of the animation clips
        animationClips = animator.runtimeAnimatorController.animationClips;

        // Iterate over the clips and gather their information
        foreach (AnimationClip animClip in animationClips)
        {
            if (animClip.name.Contains("Turn"))
            {
                turningAnimations.Add(animClip);
            } else if (!reactingAnimations.Contains(animClip))
            {
                normalAnimations.Add(animClip);
            }
        }
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
            collidedWithHands = true;
            
        }else if (collision.gameObject.CompareTag("Animal") || collision.gameObject.CompareTag("Object"))
        {
            collidedWithObjectsOrAnimals = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collidedWithHands = false;
        collidedWithObjectsOrAnimals = false;
    }

    IEnumerator Wander()
    {
        int randSteps;
        AnimationClip randNextAni = nextAni();
        
        //turnBias = false;
        /*
        if (correctDirection) { randNextAni = runTowardsCartAnimation; correctDirection = false; }
        else { randNextAni = nextAni();}*/
     
        float startTime = Time.time;
        isWandering = true;

        // if position not in range or collision, stop the wait halfway

        animator.SetBool("is" + randNextAni.name, true);
            


        if (turnBias)
        {
            randSteps = Random.Range(2000, 3000);
            //turnBias = false;
        }
        else if (endsWhenAniEnds) //TODO
        {
            randSteps = (int)(animator.GetCurrentAnimatorStateInfo(0).length +
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime) * 1000;
        }else
        {
            randSteps = Random.Range(500, 2500);
        }


        yield return new WaitUntil(() => {
            // if no RM
            // TODO: translate always until waituntil stops
            if (movingAniforNoRM.Count != 0 && movingAniforNoRM.Contains(randNextAni))
            {
                transform.Translate(0, 0, movingSpeed);
                //Vector3 _randDirection = new Vector3(Random.Range);
                //Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Random.rotation);
            }

            //TODO: random turn direction


            if (faceToPlayer)
            {
                Vector3 _direction = (playerHead.transform.position - transform.position).normalized;
                _direction.y = 0;

                Quaternion _lookRotation = Quaternion.LookRotation(_direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, 0.5f);

                if(Quaternion.Angle(transform.rotation, _lookRotation) < 10)
                {
                    faceToPlayer = false;
                }
            }


            // if collision with hands
            if (collidedWithHands)
            {
                return true;
            }
            // let it turn until it is pointing inside the area (rotation)
            else if (turnBias)
            {
                Vector3 _direction;
                /*
                if (turnTowardsCart)
                {
                    _direction = (mineCart.transform.position - transform.position).normalized;
                }
                else
                {*/
                _direction = (midPoint - transform.position).normalized;
                //                }
                //TODO: how to change rotating direction
                Quaternion _lookRotation = Quaternion.LookRotation(_direction);

                if (turningAnimations.Count == 0)
                {
                    // slerp thinghy
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                        _lookRotation, turningSpeed);
                }

                if (Quaternion.Angle(transform.rotation, _lookRotation) < 10)
                {
                    turnBias = false;
                    correctDirection = true;
                    //                    turnTowardsCart = false;
                    return true;
                }
                else if (5000 <= Time.time - startTime)
                    return true;
                else if (isReactAni)
                {
                    if (randSteps / 1000 <= Time.time - startTime)
                        return true;
                }
            }
            // if out of bounds
            else if (transform.position.x > endX || transform.position.x < startX || transform.position.z > endZ ||
            transform.position.z < startZ || collidedWithObjectsOrAnimals) 
            {
                turnBias = true;
                return true;
            }
            /*
                        // if it is moving towards cart
                        else if (moveTowardsCart)
                        {
                            // TODO: if no RM, walk

                            if (Vector3.Distance(mineCart.transform.position, transform.position) < 1)
                            {
                                moveTowardsCart = false;
                                return true;
                            }

                        } */
            // if time reached
            else if (randSteps / 1000 <= Time.time - startTime)
                return true;

            // if cart is nearby
 /*           else if (Vector3.Distance(mineCart.transform.position, transform.position) < 20)
            {
                Vector3 _direction = (mineCart.transform.position - transform.position).normalized;
                Quaternion _lookRotation = Quaternion.LookRotation(_direction);

                if (Quaternion.Angle(transform.rotation, _lookRotation) < 10)
                {
                    moveTowardsCart = true;
                    return true;
                }
                else
                {
                    print("nearby cart");
                    turnBias = true;
                    turnTowardsCart = true;
                    return true;
                }

            }*/

            return false;
        });
        animator.SetBool("is" + randNextAni.name, false);
        isReactAni = false;
        foreach (AudioSource audioSource in audio)
        {
            audioSource.Stop();
        }
        if (/*!moveTowardsCart &&*/ !collidedWithHands ) idleBreathe = !idleBreathe;
        isWandering = false;
    }
    

    AnimationClip nextAni()
    {
        int index;
        if (collidedWithHands && reactAnimations.Count != 0)
        {
            index = Random.Range(0, reactAnimations.Count);
            collidedWithHands = false;
            //audio.Play();
            print(reactAnimations[index]);
            isReactAni = true;
            if (!reactAnimations[index].name.Contains("Turn")) { faceToPlayer = true; }
            return reactAnimations[index];

        }
        else if (turnBias && turningAnimations.Count != 0)
        {
            index = Random.Range(0,turningAnimations.Count);
            return turningAnimations[index];
        }
        /*
        else if (turnBias && movingLeftRightForNoRM.Count != 0)
        {
            index = Random.Range(0, movingLeftRightForNoRM.Count);
            return movingLeftRightForNoRM[index];
        }*/
        /*
        else if (moveTowardsCart)
        {
            return runTowardsCartAnimation;
        } */
        else if (idleBreathe)
        {
            foreach (AnimationClip ani in normalAnimations)
            {
                if (ani.name.Equals("IdleBreathe"))
                    return ani;
            }
            return null;
        }
        else
        {
            index = Random.Range(0, animationClips.Length);

            return animationClips[index];
        }
    }

    void playSound(int index)
    {
        audio[index].Play();
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
}
