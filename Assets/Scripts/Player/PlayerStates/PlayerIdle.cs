using Microsoft.Cci;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdle : PlayerState
{
    public override void EnterState(Player player)
    {
        //Debug.Log("0");
        player.animator.SetTrigger("Idle");
    }

    public override void ExitState(Player player)
    {

    }

    public override void FixedUpdateState(Player player)
    {
        if (player.grdDetect.IsGrounded == false)
        {
            player.SwitchState(player.inAirState);
        }

        if (player.doMove)
        {
            player.Move();

            player.doMove = false;
        }
        else
        {
            player.animator.SetFloat("Speed", Mathf.Lerp(player.animator.GetFloat("Speed"), 0, 0.25f));
        }

        if (player.doAttack)
        {
            player.SwitchState(player.attackState);
        }
    }

    public override void UpdateState(Player player)
    {
        if (player.playerInputActions.Player.Jump.WasPressedThisFrame())
        {
            if (player.grdDetect.IsGrounded == false)
            {
                return;
            }
            player.SwitchState(player.jumpingState);
        }
    }
}
