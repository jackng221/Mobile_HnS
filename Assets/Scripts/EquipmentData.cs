using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/Equipment")]
public class EquipmentData : ScriptableObject
{
    public enum EquipmentType
    {
        OneHandSword,
        TwoHandSword,
        Shield,
        ProtectGear
    }
    public GameObject prefabObj;
    public string displayName;
    public float attackPt;
    public float defencePt;
    public EquipmentType type;
}
