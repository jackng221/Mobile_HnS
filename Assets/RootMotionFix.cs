using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionFix : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void OnAnimatorMove()
    {
        if (animator.applyRootMotion)
        {
            //transform.parent.rotation = animator.rootRotation;
            transform.parent.position += animator.deltaPosition;
        }
    }
}
