using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform currentTarget;
    public Transform PlayerTransform;
    

    public float walkingSpeed = 200f;
    public float runningSpeed = 600f;
    public float noticeDistance = 5f;
    public float nextWaypointDistance = 3f;
    public float eatingTime = 2;

    public string currentState = "Idle";

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);      
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, currentTarget.position, OnPathComplete);
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
        /*check if neaby bone
         *      if found, set target to bone
         *  else
         *      set target to player
         */
        if(StateManager.availableBones.Count > 0)
        {
            float distToClosestBone = noticeDistance;
            foreach (GameObject bone in StateManager.availableBones)
            {
                float distToThisBone = Vector2.Distance(rb.position, bone.transform.position);
                if (distToThisBone < distToClosestBone){
                    distToClosestBone = distToThisBone;
                    currentTarget = bone.transform;
                }
            }
        }
        else
        {
            currentTarget = PlayerTransform;
        }

        switch (currentState)
        {
            case "Idle":
                FollowingState(walkingSpeed);
                break;
            case "Chasing":
                FollowingState(runningSpeed);
                break;
            case "Eating":
                //EatingState();
                break;
        }
    }

    void FollowingState(float speed)
    {
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
    }
}
