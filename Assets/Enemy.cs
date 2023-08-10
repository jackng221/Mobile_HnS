using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ICharacter
{
    [SerializeField] EnemyData enemyData;
    [field: SerializeField] public float maxHealth { get; set; }
    [field: SerializeField] public float currentHealth { get; set; }
    [field: SerializeField] public float attackPt { get; set; }
    [field: SerializeField] public float defencePt { get; set; }
    [field: SerializeField] public float speedPt { get; set; }

    private void Start()
    {
        if (enemyData)
        {
            maxHealth = enemyData.baseMaxHp;
            currentHealth = maxHealth;
            attackPt = enemyData.baseAttackPt;
            defencePt = enemyData.baseDefencePt;
            speedPt = enemyData.baseSpeedPt;
        }
    }
    public void Damage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void AddAttack(float atk)
    {
        attackPt += atk;
    }

    public void AddDefence(float def)
    {
        defencePt += def;
    }

    public void AddSpeed(float spd)
    {
        speedPt += spd;
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Move()
    {
        throw new System.NotImplementedException();
    }

    public void Move(Vector2 input)
    {
        throw new System.NotImplementedException();
    }
}
