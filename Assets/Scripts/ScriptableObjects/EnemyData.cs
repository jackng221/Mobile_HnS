using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy")]
public class EnemyData : ScriptableObject
{
    public GameObject prefabObj;
    public string displayName;
    public float baseMaxHp;
    public float baseAttackPt;
    public float baseDefencePt;
    public float baseSpeedPt;

}
