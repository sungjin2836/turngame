using System.Linq;
using UnityEngine;

public class Enemy : Character
{
    [Header("적 캐릭터 정보")] public int maxShield;

    public ElementType[] weakElements;
    
    private int _shield;

    public int shield
    {
        get => _shield;
        private set => _shield = Mathf.Clamp(value, 0, maxShield);
    }

    public override void Initialize(int id)
    {
        var enemyData = DataManager.Instance.GetEnemyData(id);
        charName = enemyData.charName;
        level = enemyData.level;
        maxHP = enemyData.hp;
        speed = enemyData.speed;
        attackStat = enemyData.attackStat;
        weakElements = enemyData.elem;
        maxShield = enemyData.shield;

        hp = maxHP;
        finalSpeed = speed;
        finalAttackStat = attackStat;
        shield = maxShield;
        
        Debug.Log(JsonUtility.ToJson(this));
    }

    public bool ContainsElement(ElementType element)
    {
        return weakElements.Contains(element);
    }

    public bool HasShield()
    {
        return shield > 0;
    }

    public void DamageToShield(int damage)
    {
        shield -= damage;
    }

    public void RegenShield()
    {
        shield = maxShield;
    }
}