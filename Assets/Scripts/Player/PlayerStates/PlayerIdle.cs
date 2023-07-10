using Microsoft.Cci;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdle : PlayerState
{
    public override void EnterState(Player player)
    {
        //Debug.Log("0");
        player.animator.SetInteger("State", 0);
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
