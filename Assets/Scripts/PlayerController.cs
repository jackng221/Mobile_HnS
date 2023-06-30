using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
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
    [SerializeField] GameObject camYaw;
    [SerializeField] GameObject camPitch;
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
    float pitchDegree;
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
        if (doLook)
        {
            Look(lookInput);
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
        rb.velocity = (input.x * camYaw.transform.right + input.y * camYaw.transform.forward).normalized * input.magnitude * moveMultiplier * Time.deltaTime + new Vector3(0, rb.velocity.y, 0);
        //Debug.Log(rb.velocity);
        RotateChar();
        animator.SetFloat("Speed", Mathf.Lerp (animator.GetFloat ("Speed"), input.magnitude, 0.1f) );
    }
    void Look(Vector2 input)
    {
        camYaw.transform.Rotate(new Vector3(0, input.x, 0) * lookSpeed * Time.deltaTime);

        pitchDegree -= input.y * lookSpeed * Time.deltaTime;
        pitchDegree = Mathf.Clamp(pitchDegree, -90, 90);
        camPitch.transform.eulerAngles = new Vector3(pitchDegree, camPitch.transform.eulerAngles.y, camPitch.transform.eulerAngles.z);
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
