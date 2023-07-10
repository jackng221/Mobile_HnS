using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAir : PlayerState
{
    Rigidbody rb;

    public PlayerInAir(Rigidbody rb)
    {
        this.rb = rb;
    }

    public override void EnterState(Player player)
    {
        //Debug.Log("2");

        player.animator.SetInteger("State", 2);
    }

    public override void ExitState(Player player)
    {

    }

    public override void UpdateState(Player player)
    {

    }
    public override void FixedUpdateState(Player player)
    {
        if (player.grdDetect.IsGrounded && rb.velocity.y <= 0f)
        {
            player.SwitchState(player.landingState);
        }
    }
}
