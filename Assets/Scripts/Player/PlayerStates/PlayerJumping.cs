using Microsoft.Cci;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumping : PlayerState
{
    float jumpVelocity;
    Rigidbody rb;

    public PlayerJumping(float jumpVelocity, Rigidbody rb)
    {
        this.jumpVelocity = jumpVelocity;
        this.rb = rb;
    }
    public override void EnterState(Player player)
    {
        //Debug.Log("1");

        player.animator.Play(Player.animations.JumpStart.ToString());

        rb.velocity += new Vector3(0, jumpVelocity, 0);
    }

    public override void ExitState(Player player)
    {

    }

    public override void UpdateState(Player player)
    {
        if (player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 >= 0.99f)
        {
            player.SwitchState(player.inAirState);
        }
    }
    public override void FixedUpdateState(Player player)
    {

    }
}
