using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter : IDamageable
{
    float attackPt { get; set; }
    float defencePt { get; set; }
    float speedPt { get; set; }

    void AddAttack(float atk);
    void AddDefence(float def);
    void AddSpeed(float spd);
}
