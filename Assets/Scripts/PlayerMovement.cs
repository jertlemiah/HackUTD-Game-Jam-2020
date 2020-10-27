﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool lookAtMouse = true;

    public InputMaster controls;
    public Rigidbody2D rb;
    public Animator animator;
    public new Camera camera;
    public GameObject selectedBone;

    Vector2 movement;
    Vector2 mousePos;

    [SerializeField] private float ThrowArcOffsetY = 1;
    [SerializeField] private float ThrowArcDuration = 1;

    // Update is called once per frame
    private void Awake()
    {
        controls = new InputMaster();
        //controls.Player.Movement.performed += context => Move(context.ReadValue<Vector2>());
        controls.Player.Throw.performed += context => ThrowBone ();
    }

    void Update()
    {
        // Input
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");
        mousePos = camera.ScreenToWorldPoint(controls.Player.MousePosition.ReadValue<Vector2>());
        movement = controls.Player.Movement.ReadValue<Vector2>();
        if (lookAtMouse)
            LookAtPos(mousePos);
        if (movement.SqrMagnitude() > 0)
            Move(movement);
    }

    private void LookAtPos(Vector2 lookPos)
    {
        Vector2 lookDir = lookPos - rb.position;
        animator.SetFloat("Horizontal", lookDir.x);
        animator.SetFloat("Vertical", lookDir.y);
        animator.SetFloat("Speed", 0f);
    }

    private void ThrowBone()
    {
        Debug.Log("Player throwing bone at: " + mousePos);
        GameObject bone = Instantiate(selectedBone, rb.position, Quaternion.identity) as GameObject;
        //bone.transform.SetParent("_Dynamic");
        Vector2 midPoint = new Vector2((mousePos.x + rb.position.x) / 2, (mousePos.y + rb.position.y) / 2 + ThrowArcOffsetY);

        bone.transform.DOPath(new Vector3[] { rb.position, midPoint, mousePos }, ThrowArcDuration, PathType.CatmullRom, PathMode.TopDown2D);
    }

    private void Move(Vector2 direction)
    {
        //Debug.Log("Plyaer wants to move: " + direction);
        // Movement
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        if (!lookAtMouse)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
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
