using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneController : MonoBehaviour
{
    public string currentState = "Thrown";
    public bool isGrounded = false;
    public Animator animator;
    private Vector2 curPos, lastPos;
    private Rigidbody2D rb;

    private float ThrowArcDuration = 1;
    private float timeRemaining = 0;

    void Start()
    {
        Start(ThrowArcDuration);
    }

    void Start(float ThrowArcDuration)
    {
        if (animator == null)
        {
            animator = this.GetComponentInChildren<Animator>();
        }
        try
        {
            rb = GetComponent<Rigidbody2D>();
        }
        finally { }
        timeRemaining = ThrowArcDuration;
    }

    public void SetThrowArcDuration(float newDuration)
    {
        ThrowArcDuration = newDuration;
    }

    public void ChangeState(string state)
    {
        switch (state)
        {
            case "Thrown":
                currentState = "Thrown";
                animator.SetBool("IsGrounded", false);
                break;
            case "Grounded":
                currentState = "Grounded";
                animator.SetBool("IsGrounded", true);
                isGrounded = true;
                break;
            default:
                Debug.Log("Error:" + name + " has no state called " + state);
                break;
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        if(currentState == "Thrown" && timeRemaining > 0)
        {
            //ChangeState("Grounded");
            timeRemaining -= Time.deltaTime;
        }
        else if(timeRemaining <= 0) 
            ChangeState("Grounded");
    }
}
