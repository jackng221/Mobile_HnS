using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable
{
    public PlayerInputActions playerInputActions;
    Rigidbody rb;
    [SerializeField] GameObject cam;
    [SerializeField] CinemachineInputProvider inputProvider;
    [SerializeField] GameObject charObj;
    public Animator animator;

    //Inputs
    Vector2 moveInput;
    public bool doLook = false;
    public bool doMove = false;

    //Ground detect script
    public GroundDetect grdDetect;

    //Stats
    [SerializeField] float moveMultiplier = 150;
    [SerializeField] float jumpVelocity = 0.5f;
    [SerializeField] public float maxHealth { get; set; } = 100f;
    public float currentHealth { get; set; }

    //State
    PlayerState currentState;
    public PlayerIdle idleState;
    public PlayerJumping jumpingState;
    public PlayerInAir inAirState;
    public PlayerLanding landingState;

    public enum animations 
    {
        IdleWalkRunBlend,
        JumpStart,
        InAir,
        JumpLand,
    };

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        grdDetect = GetComponentInChildren<GroundDetect>();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    private void Start()
    {
        //Application.targetFrameRate = 30;
        currentHealth = maxHealth;

        //state constructors
        idleState = new PlayerIdle();
        jumpingState = new PlayerJumping(jumpVelocity, rb);
        inAirState = new PlayerInAir(rb);
        landingState = new PlayerLanding();

        //Default state: idle
        currentState = idleState;
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);

        moveInput = playerInputActions.Player.Move.ReadValue<Vector2>();

        if (doLook)
        {
            inputProvider.enabled = true;
        }
        else
        {
            inputProvider.enabled = false;
        }

        if (moveInput.magnitude > 0.1f)
        {
            doMove = true;
        }
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
        if (doMove)
        {
            Move(moveInput);

            doMove = false;
        }
        else
        {
            animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), 0, 0.25f));
        }

        //if (inAir && grdDetect.IsGrounded && rb.velocity.y <= 0.5f)
        //{
        //    Land();
        //    inAir = false;
        //}
        //animator.SetBool("FreeFall", inAir);
        //animator.SetBool("Grounded", grdDetect.IsGrounded);

    }

    public void SwitchState(PlayerState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    //Movement
    void RotateChar()
    {
        //charObj.transform.rotation = Quaternion.Lerp(charObj.transform.rotation, Quaternion.LookRotation(camYaw.transform.forward), 0.2f);
        charObj.transform.rotation = Quaternion.Lerp(charObj.transform.rotation, Quaternion.LookRotation(new Vector3 (rb.velocity.x, 0, rb.velocity.z) ), 0.15f);
    }
    void Move(Vector2 input)
    {
        Vector3 direction = (input.x * cam.transform.right + input.y * cam.transform.forward);  //<- somehow normalized doesn't work here
        direction = new Vector3(direction.x, 0, direction.z).normalized;
        rb.velocity = direction * input.magnitude * moveMultiplier * Time.deltaTime + new Vector3(0, rb.velocity.y, 0);

        RotateChar();
        animator.SetFloat("Speed", Mathf.Lerp (animator.GetFloat ("Speed"), input.magnitude, 0.1f) );
    }
    //void Jump()
    //{
    //    animator.SetBool("Jump", true);
    //    rb.velocity += new Vector3(0, jumpVelocity, 0);
    //    inAir = true;
    //}
    //void Land()
    //{
    //    animator.SetBool("Jump", false);
    //    Debug.Log("Land");
    //}

    //Combat
    public void Damage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
