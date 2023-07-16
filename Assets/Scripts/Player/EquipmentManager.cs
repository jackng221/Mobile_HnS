using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentList
{
    Sword,
    Shield
}

public class EquipmentManager : MonoBehaviour
{

    [SerializeField] List<EquipSlot> equipSlots;

    void DisplayEquipment()
    {
        foreach (EquipSlot slot in equipSlots)
        {
            if (slot.data == null)
            {
                Destroy(slot.equipmentObj);
            }
            else
            {
                if (slot.equipmentObj != null) { continue; }

                slot.equipmentObj = Instantiate(slot.data.prefabObj, slot.HoldingPos);

                if (slot.SlotName == SlotList.RightHand)
                {
                    slot.equipmentObj.transform.Rotate(new Vector3(180, 0, 0));
                }
            }
        }
    }
    void Equip(SlotList slotName, EquipmentList equipmentName)
    {
        EquipmentData equipmentData = Resources.Load<EquipmentData>("Equipments/" +  equipmentName.ToString() );
        if (equipmentData == null)
        {
            Debug.Log("Failed to load equipment data");
            return;
        };

        foreach (EquipSlot slot in equipSlots)
        {
            if (slot.SlotName == slotName)
            {
                if (slot.data != null)
                {
                    Debug.Log("Requested slot is already equipped");
                    return;
                }

                slot.data = equipmentData;
                DisplayEquipment();
                return;
            }
        }
        Debug.Log("Requested slot doesn't exist");
    }
    void Unequip(SlotList slotName)
    {
        foreach (EquipSlot slot in equipSlots)
        {
            if (slot.SlotName == slotName)
            {
                slot.data = null;
                DisplayEquipment();
                return;
            }
        }
        Debug.Log("Requested slot doesn't exist");
    }


    [SerializeField] SlotList testSlot;
    [SerializeField] EquipmentList testEquipment;
    [ContextMenu("testEquip")]
    void TestEquip()
    {
        Equip(testSlot, testEquipment);
    }
    [ContextMenu("testUnequip")]
    void TestUnequip()
    {
        Unequip(testSlot);
    }
}
