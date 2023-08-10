using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Stance", menuName = "ScriptableObjects/Stance")]
public class StanceData : ScriptableObject
{
    public AnimatorOverrideController stanceAnimator;
    public List<float> comboMultiplier;
}
