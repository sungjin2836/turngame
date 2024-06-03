using UnityEngine;
using static SkillDataManager;

public class Player : Character
{
    [Header("플레이어 캐릭터 정보")] public ElementType element;

    [SerializeField]
    public Skill normalAttack;
    [SerializeField]
    public Skill battleSkill;

    public override void Initialize(int id)
    {
        var playerData = DataManager.Instance.GetPlayerData(id);
        charName = playerData.charName;
        level = playerData.level;
        maxHP = playerData.hp;
        speed = playerData.speed;
        attackStat = playerData.attackStat;
        element = playerData.elem;

        SkillDataManager.Skill normalAttackData = SkillDataManager.Instance.GetSkillData($"{id}001");
        SkillDataManager.Skill battleSkillData = SkillDataManager.Instance.GetSkillData($"{id}002");

        normalAttack = gameObject.AddComponent<Skill>();
        normalAttack.skillName = normalAttackData.name;
        normalAttack.range = normalAttackData.range;
        normalAttack.damageAttr1 = normalAttackData.damageAttr1;
        normalAttack.damageAttr2 = normalAttackData.damageAttr2;
        normalAttack.damageAttr1Type = normalAttackData.damageAttr1Type;
        normalAttack.damageAttr2Type = normalAttackData.damageAttr2Type;

        battleSkill = gameObject.AddComponent<Skill>();
        battleSkill.skillName = battleSkillData.name;
        battleSkill.range = battleSkillData.range;
        battleSkill.damageAttr1 = battleSkillData.damageAttr1;
        battleSkill.damageAttr2 = battleSkillData.damageAttr2;
        battleSkill.damageAttr1Type = battleSkillData.damageAttr1Type;
        battleSkill.damageAttr2Type = battleSkillData.damageAttr2Type;

        hp = maxHP;
        finalSpeed = speed;
        finalAttackStat = attackStat;
        Debug.Log("파이널 어택"+finalAttackStat);
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