using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, ICharacter
{
    public PlayerInputActions playerInputActions;
    Rigidbody rb;
    [SerializeField] GameObject cam;
    [SerializeField] CinemachineInputProvider inputProvider;
    [SerializeField] GameObject charObj;
    public GameObject CharObj { get { return charObj; } }
    public Animator animator;
    RuntimeAnimatorController defaultController;

    //Inputs
    public Vector2 moveInput { get; private set; }
    public bool doLook = false;
    public bool doMove = false;
    public bool doAttack = false;

    //Ground detect script
    public GroundDetect grdDetect;

    //Stats
    [SerializeField] float moveMultiplier = 150;
    [SerializeField] float jumpVelocity = 0.5f;
    [SerializeField] float moveRotateLerp = 0.15f;
    [SerializeField] float attackRotateLerp = 0.01f;
    public float AttackRotateLerp { get { return attackRotateLerp; } }

    [field: SerializeField] public float maxHealth { get; set; } = 100f;
    [field: SerializeField] public float currentHealth { get; set; }
    [field: SerializeField] public float attackPt { get; set; } = 1;
    [field: SerializeField] public float defencePt { get; set; } = 1;
    [field: SerializeField] public float speedPt { get; set; } = 1;

    //State
    PlayerState currentState;
    public PlayerIdle idleState;
    public PlayerJumping jumpingState;
    public PlayerInAir inAirState;
    public PlayerLanding landingState;
    public PlayerAttack attackState;

    //Stance
    public event Action OnChangeStance;

    public enum animations 
    {
        IdleWalkRunBlend,
        JumpStart,
        InAir,
        JumpLand,
        Combo1,
        Combo2,
        Combo3
    };

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        defaultController = animator.runtimeAnimatorController;
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
        attackState = new PlayerAttack();

        //Default state: idle
        currentState = idleState;
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);

        moveInput = playerInputActions.Player.Move.ReadValue<Vector2>();

        //get from ScreenSwipe script
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

        if (playerInputActions.Player.Fire.WasPressedThisFrame())
        {
            //doAttack = true;
            StartCoroutine(AttackBuffer());
        }
        //Debug.Log(doAttack);
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);

        //if (inAir && grdDetect.IsGrounded && rb.velocity.y <= 0.5f)
        //{
        //    Land();
        //    inAir = false;
        //}
        //animator.SetBool("FreeFall", inAir);
        //animator.SetBool("Grounded", grdDetect.IsGrounded);
    }

    //Functions
    public void SwitchState(PlayerState state)
    {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }
    IEnumerator AttackBuffer()
    {
        float bufferTime = 0.2f;
        do
        {
            doAttack = true; //Debug.Log(doAttack);
            bufferTime -= Time.deltaTime;
            yield return null;
        } while (bufferTime > 0);
    }

    //Movement
    public void RotateChar(float lerpValue)
    {
        charObj.transform.rotation = Quaternion.Lerp(charObj.transform.rotation, Quaternion.LookRotation(new Vector3 (rb.velocity.x, 0, rb.velocity.z) ), lerpValue);
    }
    public void Move()
    {
        Vector3 direction = (moveInput.x * cam.transform.right + moveInput.y * cam.transform.forward);  //<- somehow normalized doesn't work here
        direction = new Vector3(direction.x, 0, direction.z).normalized;
        rb.velocity = direction * moveInput.magnitude * moveMultiplier * Time.deltaTime + new Vector3(0, rb.velocity.y, 0);

        RotateChar(moveRotateLerp);
        animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), moveInput.magnitude, 0.1f));
    }
    public void Move(Vector2 input)
    {
        Vector3 direction = (input.x * cam.transform.right + input.y * cam.transform.forward);  //<- somehow normalized doesn't work here
        direction = new Vector3(direction.x, 0, direction.z).normalized;
        rb.velocity = direction * input.magnitude * moveMultiplier * Time.deltaTime + new Vector3(0, rb.velocity.y, 0);

        RotateChar(moveRotateLerp);
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
    public void ChangeStance()
    {
        animator.runtimeAnimatorController = defaultController;
        OnChangeStance?.Invoke();
    }
    public void ChangeStance(EquipmentData data)
    {
        animator.runtimeAnimatorController = data.stanceData.stanceAnimator;
        OnChangeStance?. Invoke();
    }


    //=========Combat

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

    public void AddAttack(float atk)
    {
        attackPt += atk;
    }

    public void AddDefence(float def)
    {
        defencePt += def;
    }

    public void AddSpeed(float spd)
    {
        speedPt += spd;
    }
}
