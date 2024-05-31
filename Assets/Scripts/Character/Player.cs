using UnityEngine;

public class Player : Character
{
    [Header("플레이어 캐릭터 정보")] public ElementType element;

    public override void Initialize(int id)
    {
        var playerData = DataManager.Instance.GetPlayerData(id);
        charName = playerData.charName;
        level = playerData.level;
        maxHP = playerData.hp;
        speed = playerData.speed;
        attackStat = playerData.attackStat;
        element = playerData.elem;

        hp = maxHP;
        finalSpeed = speed;
        finalAttackStat = attackStat;
        
        Debug.Log(JsonUtility.ToJson(this));
    }

    public override void NormalAttack(Character target, float value = 0.5f)
    {
        var enemy = target as Enemy;
        if (enemy.ContainsElement(element)) enemy.DamageToShield(30);
        enemy.GetDamage(Mathf.FloorToInt(finalAttackStat * value), enemy.HasShield());
        Debug.Log($"{enemy.charName}의 체력은 {enemy.hp}/{enemy.maxHP}, 실드는 {enemy.shield}/{enemy.maxShield}");
    }

    public virtual void BattleSkill(Character target, float value)
    {
        // 스킬
    }

    public virtual void BattleSkill(Character[] target, float value)
    {
        // 스킬
    }
}