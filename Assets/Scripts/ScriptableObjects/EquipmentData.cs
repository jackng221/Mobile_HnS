using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/Equipment")]
public class EquipmentData : ScriptableObject
{
    public enum EquipmentType
    {
        OneHand,
        TwoHand
    }
    public string displayName;
    public float attackPt;
    public float defencePt;
    public StanceData stanceData;
    public bool leftHanded;
    public bool rightHanded;
    public List<LeftRightHitboxOnOff> HitboxOnOffList;
}
[Serializable]
public class LeftRightHitboxOnOff
{
    public bool leftHitbox;
    public bool rightHitbox;
}