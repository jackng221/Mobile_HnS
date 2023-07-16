using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum SlotList
    {
        Head,
        Body,
        LeftHand,
        RightHand,
        Legs
    }

[System.Serializable]
public class EquipSlot
{
    [SerializeField] SlotList slotName;
    public SlotList SlotName { get { return slotName; } }

    [SerializeField] Transform holdingPos;
    public Transform HoldingPos { get { return holdingPos; } }

    public EquipmentData data;

    /*[HideInInspector]*/ public GameObject equipmentObj;
}
