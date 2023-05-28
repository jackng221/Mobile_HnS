using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions playerInputActions;
    Rigidbody rb;
    [SerializeField] GameObject camYaw;
    [SerializeField] GameObject camPitch;

    Vector2 moveInput;
    Vector3 lookInput;

    [SerializeField] float moveSpeed = 100f;
    [SerializeField] float lookSpeed = 100f;
    float pitchDegree;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
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
        moveInput = playerInputActions.action.move.ReadValue<Vector2>();
        //Debug.Log(moveInput);
        lookInput = playerInputActions.action.look.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move(moveInput);
        Look(lookInput);
    }

    void Move(Vector2 input)
    {
        rb.velocity = (input.x * camYaw.transform.right + input.y * camYaw.transform.forward).normalized * moveSpeed * Time.deltaTime + new Vector3(0, rb.velocity.y, 0);
        //Debug.Log(rb.velocity);
    }
    void Look(Vector2 input)
    {
        camYaw.transform.Rotate(new Vector3(0, input.x, 0) * lookSpeed * Time.deltaTime);

        pitchDegree -= input.y * lookSpeed * Time.deltaTime;
        pitchDegree = Mathf.Clamp(pitchDegree, -90, 90);
        camPitch.transform.eulerAngles = new Vector3(pitchDegree, camPitch.transform.eulerAngles.y, camPitch.transform.eulerAngles.z);
    }
}
