using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool lookAtMouse = true;
    public float CurrentHealth = 10;
    [SerializeField]
    public float MaxHealth = 10;
    public Color ColorIFrame = Color.red;
    public Color ColorOriginal= Color.white;
    public float iFrameDuration = 0.02f;
    public float remainingIFrameDuration = 0;
    public bool isInvun = false;
    public float HitStrengthPlayer = 5;
    public float HitStrengthBone = 5;
    public int NumberOfiFrames = 4;

    public int normalBones = 5;
    public int funnyBones = 0;
    public int ribBones = 0;

    int totalBones;

    public int currentBone = 0; //0 is normal, 1 is funny, 2 is rib

    public InputMaster controls;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public new Camera camera;
    public GameObject selectedBone;

    public GameObject nBone;
    public GameObject fBone;
    public GameObject rBone;

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
        CurrentHealth = MaxHealth;
        normalBones = 10;
        funnyBones = 1;
        ribBones = 1;
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
        if(currentBone == 0)
        {
            selectedBone = nBone;
        }
        else if(currentBone == 1)
        {
            selectedBone = fBone;
        }
        else if(currentBone == 2)
        {
            selectedBone = rBone;
        }

        // Input
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");
        if(StateManager.IsPaused == false)
        {
            mousePos = camera.ScreenToWorldPoint(controls.Player.MousePosition.ReadValue<Vector2>());
            movement = controls.Player.Movement.ReadValue<Vector2>();

            //if (isInvun)
            //{
            //    remainingIFrameDuration -= Time.deltaTime;
            //    if (remainingIFrameDuration <= 0)
            //        isInvun = false;
            //}
            if (lookAtMouse)
                LookAtPos(mousePos);
            if (movement.SqrMagnitude() > 0)
                Move(movement);

            if(currentBone == 0 && Input.GetKeyDown(KeyCode.Q))
            {
                currentBone = 2;
            }
            else if(currentBone == 2 && Input.GetKeyDown(KeyCode.E))
            {
                currentBone = 0;
            }
            else if(Input.GetKeyDown(KeyCode.Q))
            {
                currentBone--;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                currentBone++;
            }
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
        if(--CurrentHealth <= 0)
        {
            StateManager.Instance.OpenGameOverUI();
        }
        return bone;
    }

    public Vector2 RotateVector2(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public void HitPlayer(Vector2 direction)
    {
        //isInvun = true;
        remainingIFrameDuration = iFrameDuration;
        StartCoroutine(IFrameFlash());

        

        Vector2 boneVector1 = RotateVector2(direction, 30).normalized * HitStrengthBone + rb.position;
        Vector2 boneVector2 = RotateVector2(direction, -30).normalized * HitStrengthBone + rb.position;
        Vector2 playerVector = direction.normalized * HitStrengthPlayer + rb.position;

        rb.AddForce(playerVector);
        ThrowBone(boneVector1);
        ThrowBone(boneVector2);
    }

    private IEnumerator IFrameFlash()
    {
        isInvun = true;
        int temp = 0;
        while(temp < NumberOfiFrames)
        {
            spriteRenderer.color = ColorIFrame;
            yield return new WaitForSeconds(iFrameDuration);
            spriteRenderer.color = ColorOriginal;
            yield return new WaitForSeconds(iFrameDuration);
            temp++;
        }

        isInvun = false;
    }

    public void ThrowBone()
    {
        if((currentBone == 1 && funnyBones < 1) || (currentBone == 2 && ribBones < 1))
        {
            currentBone = 0;
        }
        else if(currentBone == 0 && normalBones < 1)
        {
            if(funnyBones > 0)
            {
                currentBone = 1;
            }
            else if(ribBones > 0)
            {
                currentBone = 2;
            }
        }
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

        if(currentBone == 0)
        {
            normalBones--;
        }
        else if(currentBone == 1)
        {
            funnyBones--;
        }
        else if(currentBone == 2)
        {
            ribBones--;
        }

        if (--CurrentHealth <= 0)
        {
            StateManager.Instance.OpenGameOverUI();
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
