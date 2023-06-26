using RotaryHeart.Lib.PhysicsExtension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetect : MonoBehaviour
{
    int collision;
    bool isGrounded;
    public bool IsGrounded { get { return isGrounded; } }

    private void Start()
    {
        Collider[] colliders = GetComponentsInParent<Collider>();
        foreach (Collider collider in colliders)
        {
            UnityEngine.Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), collider);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        collision++;
    }
    private void OnTriggerExit(Collider other)
    {
        collision--;
    }
    private void Update()
    {
        if (collision == 0)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
    }
}
