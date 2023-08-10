using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    //[SerializeField] List<EquipSlot> equipSlots;

    //Weapon list
    [SerializeField] GameObject wepSword;
    public GameObject Weapon_Sword { get { return wepSword; } }
    [SerializeField] GameObject wepHalberd;
    public GameObject Weapon_Halberd { get { return wepHalberd; } }

    [SerializeField] Transform leftHandHoldPos;
    [SerializeField] Transform rightHandHoldPos;

    GameObject leftHandEquip;
    public GameObject LeftHandEquip { get { return leftHandEquip; } }
    GameObject rightHandEquip;
    public GameObject RightHandEquip { get { return rightHandEquip; } }

    [SerializeField] GameObject startWeapon; //Starting weapon

    public event Action OnWeaponEquip;

    private void Start()
    {
        //DisplayEquipment();

        if (startWeapon)
        {
            Equip(startWeapon);
        }
    }

    //void DisplayEquipment()
    //{
    //    foreach (EquipSlot slot in equipSlots)
    //    {
    //        if (slot.data == null)
    //        {
    //            Destroy(slot.equipmentObj);
    //        }
    //        else
    //        {
    //            if (slot.equipmentObj != null) { continue; }

    //            slot.equipmentObj = Instantiate(slot.data.prefabObj, slot.HoldingPos);

    //            if (slot.SlotName == SlotList.RightHand)
    //            {
    //                slot.equipmentObj.transform.Rotate(new Vector3(180, 0, 0));
    //            }
    //        }
    //    }
    //}

    public void Equip(GameObject prefab)
    {
        //foreach (EquipSlot slot in equipSlots)
        //{
        //    if (slot.SlotName == slotName)
        //    {
        //        if (slot.data != null)
        //        {
        //            Debug.Log("Requested slot is already equipped");
        //            return;
        //        }

        //        slot.data = equipmentData;
        //        GetComponentInParent<Player>()?.ChangeStance(slot.data);
        //        DisplayEquipment();
        //        return;
        //    }
        //}
        //Debug.Log("Requested slot doesn't exist");


        if (leftHandEquip) Destroy(leftHandEquip);
        if (rightHandEquip) Destroy(rightHandEquip);

        //a set of weapon(s) should have identical stancedata, double function call is intended and should be fine
        if (prefab.GetComponent<Weapon>().WeaponData.leftHanded)
        {
            leftHandEquip = Instantiate(prefab, leftHandHoldPos);
            if (gameObject.TryGetComponent<Player>(out Player player))
            {
                player.ChangeStance(prefab.GetComponent<Weapon>().WeaponData);
            }
        }
        if (prefab.GetComponent<Weapon>().WeaponData.rightHanded)
        {
            rightHandEquip = Instantiate(prefab, rightHandHoldPos);
            if (gameObject.TryGetComponent<Player>(out Player player))
            {
                player.ChangeStance(prefab.GetComponent<Weapon>().WeaponData);
            }
        }

        OnWeaponEquip? .Invoke();   //Invoke after weapon is equipped, listener is dependant on weapon
    }
    public void Unequip()
    {
        //foreach (EquipSlot slot in equipSlots)
        //{
        //    if (slot.SlotName == slotName)
        //    {
        //        slot.data = null;
        //        DisplayEquipment();
        //        return;
        //    }
        //}
        //Debug.Log("Requested slot doesn't exist");

        Destroy(leftHandEquip);
        Destroy(rightHandEquip);
    }

    //[SerializeField] SlotList testSlot;
    //[SerializeField] EquipmentList testEquipment;
    //[ContextMenu("testEquip")]
    //void TestEquip()
    //{
    //    Equip(testSlot, testEquipment);
    //}
    //[ContextMenu("testUnequip")]
    //void TestUnequip()
    //{
    //    Unequip(testSlot);
    //}

    [ContextMenu("testEquip")]
    void TestEquip()
    {
        Equip(Weapon_Halberd);
    }
    [ContextMenu("testUnequip")]
    void TestUnequip()
    {
        Unequip();
    }
}
