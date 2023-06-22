using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

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

    [SerializeField] float moveSpeed = 100f;
    [SerializeField] float lookSpeed = 100f;
    float pitchDegree;

    Animator animator;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
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

        if (playerInputActions.Player.Move.IsPressed())
        {
            doMove = true;
        }
        if (playerInputActions.Player.Jump.WasPressedThisFrame())
        {
            doJump = true;
            Debug.Log("Pressed");
        }
    }

    private void FixedUpdate()
    {
        if (doMove)
        {
            Move(moveInput);
            doMove = false;
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
        if (doLook)
        {
            Look(lookInput);
        }
        if (doJump)
        {
            doJump = false;
            if (IsGrounded() == false) {
                return;
            }
            Jump();
        }
        if (inAir && IsGrounded())
        {
            Land();
            inAir = false;
        }
        animator.SetBool("FreeFall", inAir);
        animator.SetBool("Grounded", IsGrounded());
    }

    void RotateChar()
    {
        charObj.transform.rotation = Quaternion.Lerp(charObj.transform.rotation, Quaternion.LookRotation(camYaw.transform.forward), 0.25f);
    }

    void Move(Vector2 input)
    {
        rb.velocity = (input.x * camYaw.transform.right + input.y * camYaw.transform.forward).normalized * moveSpeed * Time.deltaTime + new Vector3(0, rb.velocity.y, 0);
        //Debug.Log(rb.velocity);
        RotateChar();
        animator.SetFloat("Speed", input.magnitude * 6);
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
        rb.velocity += new Vector3(0, 3, 0);
        inAir = true;
    }
    void Land()
    {
        animator.SetBool("Jump", false);
        Debug.Log("Land");
    }
    bool IsGrounded()
    {
        CapsuleCollider collider = GetComponentInChildren<CapsuleCollider>();
        RaycastHit hit;

        if (Physics.Raycast(collider.transform.position, collider.transform.up * -1, out hit, 10))
        {
            Debug.DrawRay(collider.transform.position, collider.transform.up * -1 * 1, Color.red);
            return true;
        }
        Debug.DrawRay(collider.transform.position, collider.transform.up * -1 * 1, Color.yellow);
        return false;
    }
    public void test()
    {
        doJump = true;
        Debug.Log("Pressed");
    }
}
