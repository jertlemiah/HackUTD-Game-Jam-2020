using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public InputMaster controls;

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    // Update is called once per frame
    private void Awake()
    {
        controls = new InputMaster();
        //controls.Player.Movement.performed += context => Move(context.ReadValue<Vector2>());
    }

    void Update()
    {
        // Input
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");
        movement = controls.Player.Movement.ReadValue<Vector2>();
        if (movement.SqrMagnitude() > 0)
            Move(movement);
    }

    private void Move(Vector2 direction)
    {
        //Debug.Log("Plyaer wants to move: " + direction);
        // Movement
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
