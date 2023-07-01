using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.Windows;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions playerInputActions;
    Rigidbody rb;
    [SerializeField] GameObject cam;
    [SerializeField] CinemachineInputProvider inputProvider;
    [SerializeField] GameObject charObj;

    Vector2 moveInput;
    Vector3 lookInput;
    public bool doLook = false;
    public bool doMove = false;
    public bool doJump = false;
    bool inAir = false;
    GroundDetect grdDetect;
    bool isGrounded;

    [SerializeField] float moveMultiplier = 150;
    [SerializeField] float lookSpeed = 100f;
    float pitchDegree = 0;
    [SerializeField] float jumpVelocity = 0.5f;

    Animator animator;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        grdDetect = GetComponentInChildren<GroundDetect>();
    }

    private void Start()
    {
        //Application.targetFrameRate = 30;
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    private void Update()
    {
        moveInput = playerInputActions.Player.Move.ReadValue<Vector2>();
        lookInput = playerInputActions.Player.Look.ReadValue<Vector2>();

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
        if (playerInputActions.Player.Jump.WasPressedThisFrame())
        {
            doJump = true;
        }
    }

    private void FixedUpdate()
    {
        IsGrounded();
        if (doMove)
        {
            Move(moveInput);

            doMove = false;
        }
        else
        {
            animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), 0, 0.25f));
        }

        if (doJump)
        {
            doJump = false;
            if (isGrounded == false) {
                return;
            }
            Jump();
        }
        if (inAir && isGrounded && rb.velocity.y <= 0.5f)
        {
            Land();
            inAir = false;
        }
        animator.SetBool("FreeFall", inAir);
        animator.SetBool("Grounded", isGrounded);

    }


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
    void Jump()
    {
        animator.SetBool("Jump", true);
        rb.velocity += new Vector3(0, jumpVelocity, 0);
        inAir = true;
    }
    void Land()
    {
        animator.SetBool("Jump", false);
        Debug.Log("Land");
    }
    void IsGrounded()
    {
        if (grdDetect.IsGrounded)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }    
    }
}
