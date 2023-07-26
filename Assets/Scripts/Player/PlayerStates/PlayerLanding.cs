using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLanding : PlayerState
{
    public override void EnterState(Player player)
    {
        //Debug.Log("3");

        player.animator.Play(Player.animations.JumpLand.ToString());
    }

    public override void ExitState(Player player)
    {

    }

    public override void FixedUpdateState(Player player)
    {
        
    }

    public override void UpdateState(Player player)
    {
        if (player.doMove)
        {
            player.Move();

            player.doMove = false;
        }
        else
        {
            player.animator.SetFloat("Speed", Mathf.Lerp(player.animator.GetFloat("Speed"), 0, 0.25f));
        }

        if (player.animator.GetAnimatorTransitionInfo(0).normalizedTime == 0)
        {
            player.SwitchState(player.idleState);
        }
    }
}
