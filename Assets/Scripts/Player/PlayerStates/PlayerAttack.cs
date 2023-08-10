using Microsoft.Cci;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerAttack : PlayerState
{
    Player _player;
    EquipmentManager equipManager;

    public override void EnterState(Player player)
    {
        _player = player;
        equipManager = _player.GetComponent<EquipmentManager>();

        _player.animator.applyRootMotion = true;
        Attack();
    }

    public override void ExitState(Player player)
    {
        player.animator.applyRootMotion = false;
    }

    public override void FixedUpdateState(Player player)
    {
        RotateAttack(player.AttackRotateLerp);
    }

    public override void UpdateState(Player player)
    {
        if (player.doAttack)
        {
            Attack();
        }
    }
    [SerializeField] int currentCombo = 0;
    public int CurrentCombo { get { return currentCombo; } }
    Coroutine lastCoroutine;
    bool canAttack = true;

    public void Attack()
    {
        _player.doAttack = false;
        if (canAttack == false) { return; }
        switch (currentCombo)
        {
            case 0:
                lastCoroutine = _player.StartCoroutine(SimpleComboTimer());
                _player.GetComponent<CombatManager>().ReceiveAttackCommand(0);
                _player.animator.Play("Combo1");
                currentCombo = 1;
                break;
            case 1:
                if (currentCombo > _player.GetComponent<CombatManager>().HighestCombo) return; ;
                _player.StopCoroutine(lastCoroutine);

                lastCoroutine = _player.StartCoroutine(SimpleComboTimer());
                _player.GetComponent<CombatManager>().ReceiveAttackCommand(1);
                _player.animator.SetTrigger("Combo");
                currentCombo = 2;
                break;
            case 2:
                if (currentCombo > _player.GetComponent<CombatManager>().HighestCombo) return; ;
                _player.StopCoroutine(lastCoroutine);

                lastCoroutine = _player.StartCoroutine(SimpleComboTimer());
                _player.GetComponent<CombatManager>().ReceiveAttackCommand(2);
                _player.animator.SetTrigger("Combo");
                currentCombo = 3;
                break;
            default: break;
        }
    }
    void ResetCombo()
    {
        _player.GetComponent<CombatManager>().ReceiveAttackCommand(-1);
        currentCombo = 0;
        _player.SwitchState(_player.idleState);
    }
    IEnumerator SimpleComboTimer()
    {
        canAttack = false;
        yield return new WaitForSeconds(0.6f);  //wait time before next attack
        canAttack = true;
        yield return new WaitForSeconds(0.8f);  //wait time until combo is ended
        ResetCombo();
    }

    void RotateAttack(float lerpValue)
    {
        _player.CharObj.transform.rotation = Quaternion.Lerp(_player.CharObj.transform.rotation, Quaternion.LookRotation(new Vector3(_player.moveInput.x, 0, _player.moveInput.y) + _player.CharObj.transform.forward), lerpValue);
        Debug.DrawRay(_player.CharObj.transform.position, _player.CharObj.transform.right);
    }
}
