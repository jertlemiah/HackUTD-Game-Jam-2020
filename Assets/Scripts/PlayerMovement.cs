using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool lookAtMouse = true;
    public int currentHealth = 10;
    public Color iFrameColor = Color.white;


    public InputMaster controls;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public new Camera camera;
    public GameObject selectedBone;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] grassClips;
    [SerializeField]
    private AudioClip throwBoneClip;

    //public StateManager stateManager;

    Vector2 movement;
    Vector2 mousePos;

    [SerializeField] private float ThrowArcOffsetY = 1;
    [SerializeField] private float ThrowArcDuration = 1;

    private void Awake()
    {
        controls = new InputMaster();
        //controls.Player.Movement.performed += context => Move(context.ReadValue<Vector2>());
        controls.Player.Throw.performed += context => ThrowBone ();
        try
        {
            audioSource = GetComponent<AudioSource>();
        }    
        finally { }
    }

    void Update()
    {
        // Input
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");
        if(StateManager.IsPaused == false)
        {
            mousePos = camera.ScreenToWorldPoint(controls.Player.MousePosition.ReadValue<Vector2>());
            movement = controls.Player.Movement.ReadValue<Vector2>();
            if (lookAtMouse)
                LookAtPos(mousePos);
            if (movement.SqrMagnitude() > 0)
                Move(movement);
        }
    }

    private void LookAtPos(Vector2 lookPos)
    {
        Vector2 lookDir = lookPos - rb.position;
        if(lookDir.x < 1)
        {
            spriteRenderer.flipX = true;
        }
        else
            spriteRenderer.flipX = false;
        animator.SetFloat("Speed", 0f);

        /* use these with the old player animations
        animator.SetFloat("Horizontal", lookDir.x);
        animator.SetFloat("Vertical", lookDir.y);*/
    }

    public GameObject DropBone()
    {
        Debug.Log("Player dropping bone at: " + rb.position);
        GameObject bone = StateManager.CreateBone(selectedBone, rb.position);
        //GameObject bone = Instantiate(selectedBone, rb.position, Quaternion.identity) as GameObject;
        if(--currentHealth <= 0)
        {
            //death
        }
        return bone;
    }

    public void ThrowBone()
    {
        ThrowBone(mousePos);
    }

    public void ThrowBone(Vector2 direction)
    {
        if (StateManager.IsPaused)
            return;
        Debug.Log("Player throwing bone at: " + direction);
        //GameObject bone = Instantiate(selectedBone, rb.position, Quaternion.identity) as GameObject;
        GameObject bone = StateManager.CreateBone(selectedBone, rb.position);
        bone.GetComponent<BoneController>().SetThrowArcDuration(ThrowArcDuration);

        if (audioSource != null && throwBoneClip!=null)
        {
            audioSource.PlayOneShot(throwBoneClip);
        }

        Vector2 midPoint = new Vector2((direction.x + rb.position.x) / 2, (direction.y + rb.position.y) / 2 + ThrowArcOffsetY);

        bone.transform.DOPath(new Vector3[] { rb.position, midPoint, direction }, ThrowArcDuration, PathType.CatmullRom, PathMode.TopDown2D);

        if (--currentHealth <= 0)
        {
            //death
        }
    }

    private void Move(Vector2 direction)
    {
        //Debug.Log("Plyaer wants to move: " + direction);
        // Movement
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        if (!lookAtMouse)
        {
            if (direction.x < 1)
            {
                spriteRenderer.flipX = true;
            }
            else
                spriteRenderer.flipX = false;

            /* use these with the old player animations
            animator.SetFloat("Horizontal", lookDir.x);
            animator.SetFloat("Vertical", lookDir.y);*/
        }
        animator.SetFloat("Speed", movement.sqrMagnitude);
        if(audioSource != null && !audioSource.isPlaying && grassClips.Length>0)
        {
            AudioClip clip = grassClips[UnityEngine.Random.Range(0, grassClips.Length)];
            audioSource.PlayOneShot(clip);
        }       
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
