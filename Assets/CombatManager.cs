using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    Weapon leftWep;
    Weapon rightWep;
    [SerializeField] int highestCombo; //0..1..2..
    public int HighestCombo { get { return highestCombo; } set { highestCombo = value; } }

    private void Awake()
    {
        gameObject.GetComponent<EquipmentManager>().OnWeaponEquip += UpdateWeapon;
    }

    void UpdateWeapon()
    {
        leftWep = gameObject.GetComponent<EquipmentManager>().LeftHandEquip?.GetComponent<Weapon>();
        rightWep = gameObject.GetComponent<EquipmentManager>().RightHandEquip?.GetComponent<Weapon>();
        if (leftWep) highestCombo = leftWep.WeaponData.stanceData.comboMultiplier.Count -1;
        else highestCombo = rightWep.WeaponData.stanceData.comboMultiplier.Count - 1;
    }

    public void ReceiveAttackCommand(int combo)
    {

        ControlHitbox(combo);

        if (combo == -1) return;
        SendAttackData(combo);
    }

    void ControlHitbox(int combo)
    {
        if (combo == -1)
        {
            leftWep? .EnableHitbox(false);
            rightWep? .EnableHitbox(false);
            return;
        }

        leftWep? .EnableHitbox(leftWep.WeaponData.HitboxOnOffList[combo].leftHitbox);
        rightWep? .EnableHitbox(rightWep.WeaponData.HitboxOnOffList[combo].rightHitbox);
    }
    void SendAttackData(int combo)
    {
        if (leftWep) leftWep.currentComboOnCharacter = combo;
        if (rightWep) rightWep.currentComboOnCharacter = combo;
    } 
}
