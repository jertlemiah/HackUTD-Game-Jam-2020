using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public GameObject currentTarget;
    public GameObject PlayerObject;
    public Animator animator;

    public float walkingSpeed = 200f;
    public float runningSpeed = 600f;
    public float noticeDistance = 5f;
    public float eatingDistance = 1f;
    public float nextWaypointDistance = 3f;
    public float eatingTime = 2;
    public float timeRemaining = 0;

    public string currentState = "Idle";

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;


    Seeker seeker;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);     
        
        if(animator == null)
        {
            try
            {
                animator = this.GetComponentInChildren<Animator>();
            }
            finally { }
        }

        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        
    }

    void UpdatePath()
    {
        if(seeker.IsDone() && currentTarget != null)
            seeker.StartPath(rb.position, currentTarget.transform.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetClosestTarget();
        if (currentState == "Idle" && currentTarget != null)
            ChangeState("Chasing");

        switch (currentState)
        {       
            case "Chasing":
                try
                {
                    FollowingState(runningSpeed);
                }
                catch
                { // If a dog eats the bone another dog is running to, it would cause the game to crash
                    currentTarget = null;
                    ChangeState("Idle");
                }
                break;
            case "Eating":
                EatingState();
                break;
            case "Idle":
            default:
                //FollowingState(walkingSpeed);
                break;
        }

        
    }

    void SetClosestTarget()
    {
        /*check if neaby bone
         *      if found, set target to bone
         *  else
         *      set target to player
         */

        if (StateManager.availableBones.Count > 0)
        {
            float distToClosestBone = 100;
            foreach (GameObject bone in StateManager.availableBones)
            {
                float distToThisBone = Vector2.Distance(rb.position, bone.transform.position);
                if (distToThisBone < distToClosestBone)
                {
                    distToClosestBone = distToThisBone;
                    currentTarget = bone;
                }
            }
        }

        // if a bone was not selected, follow the player
        if (currentTarget == null)
        {
            currentTarget = PlayerObject;
        }
    }

    bool HasReachedTarget()
    {
        return (Vector2.Distance(rb.position, currentTarget.transform.position) <= eatingDistance);
    }

    /*Please use this to change states, it handles the handover 
     * in case there is something special that needs to happen */
    void ChangeState(string state)
    {
        Debug.Log(name + ": switching from " + currentState + " to " + state);
        animator.SetBool("IsEating", false);
        animator.SetFloat("Speed", 0f);
        switch (state)
        {
            case "Chasing":
                currentState = "Chasing";
                animator.SetFloat("Speed", 0.1f);
                break;
            case "Eating":
                currentState = "Eating";
                timeRemaining = eatingTime;

                // If the dog has caught the player, force the player to drop a bone, 
                //  then set the current target to that bone
                if(currentTarget == PlayerObject)
                {
                    currentTarget = PlayerObject.GetComponent<PlayerMovement>().DropBone();
                }
                StateManager.RemoveBone(currentTarget);
                Destroy(currentTarget);
                currentTarget = null;         
                animator.SetBool("IsEating", true);
                break;
            case "Idle":
                currentState = "Idle";
                break;
            default:
                Debug.Log("Error:" + name + " has no state called " + state);
                break;
        }
    }

    void EatingState()
    {
        if( timeRemaining <= 0)
        {
            ChangeState("Idle");
        }
        else
        {
            timeRemaining -= Time.deltaTime;
        }
    }

    void HitPlayer()
    {
        Debug.Log("Hit player");
    }

    void FollowingState(float speed)
    {
        if (HasReachedTarget())
        {
            if (currentTarget == PlayerObject)
                HitPlayer();
            else if (currentTarget.tag == "Bone" && currentTarget.GetComponent<BoneController>().isGrounded)
                ChangeState("Eating");
            return;
        }

        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Make sure sprite is facing correct direction
        if (direction.x < 1)
        {
            spriteRenderer.flipX = true;
        }
        else
            spriteRenderer.flipX = false;
    }
}
