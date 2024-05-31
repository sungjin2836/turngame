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
        Debug.Log("파이널 어택"+finalAttackStat);
        //Debug.Log(JsonUtility.ToJson(this));
    }

    public override int NormalAttack(Character target, float value = 0.5f)
    {
        Debug.Log("NormalAttack의 공격력" + finalAttackStat);
        var enemy = target as Enemy;
        if (enemy.ContainsElement(element)) enemy.DamageToShield(30);
        int dam = enemy.GetDamage(Mathf.FloorToInt(finalAttackStat * value), enemy.HasShield());
        Debug.Log($"{enemy.charName}의 체력은 {enemy.hp}/{enemy.maxHP}, 실드는 {enemy.shield}/{enemy.maxShield}");

        return dam;
    }

    public int NormalAttack(Character target, int _attack, float value = 0.5f)
    {
        Debug.Log("NormalAttack의 공격력" + finalAttackStat);
        Debug.Log(_attack);
        var enemy = target as Enemy;
        if (enemy.ContainsElement(element)) enemy.DamageToShield(30);
        int dam = enemy.GetDamage(Mathf.FloorToInt(_attack * value), enemy.HasShield());
        Debug.Log($"{enemy.charName}의 체력은 {enemy.hp}/{enemy.maxHP}, 실드는 {enemy.shield}/{enemy.maxShield}");

        return dam;
    }


    public virtual void BattleSkill(Character target, float value)
    {
        // 스킬
        Debug.Log($"{target.charName}에게 배틀스킬 사용");
    }

    public virtual void BattleSkill(Character[] target, float value)
    {
        // 스킬
    }
}