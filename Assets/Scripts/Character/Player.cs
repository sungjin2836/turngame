using UnityEngine;
using static SkillDataManager;

public class Player : Character
{
    [Header("플레이어 캐릭터 정보")] public ElementType element;

    public Skill normalAttack;
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
        int dam = enemy.GetDamage(Mathf.FloorToInt(finalAttackStat * normalAttack.damageAttr1[0] * 0.9f), enemy.HasShield());
        Debug.Log($"{enemy.charName}의 체력은 {enemy.hp}/{enemy.maxHP}, 실드는 {enemy.shield}/{enemy.maxShield}");

        return dam;
    }


    public virtual void BattleSkill(Character target)
    {
        // 스킬
        var enemy = target as Enemy;
        if (enemy.ContainsElement(element)) enemy.DamageToShield(60);
        int dam = enemy.GetDamage(Mathf.FloorToInt(attackStat * battleSkill.damageAttr1[0]), enemy.HasShield());
        enemy.speed -= 10;
        Debug.Log($"스킬 사용 {enemy.charName}의 체력은 {enemy.hp}/{enemy.maxHP}, 실드는 {enemy.shield}/{enemy.maxShield}");


    }

    public virtual void BattleSkill(Character[] target)
    {
        // 스킬
        for (int i = 0; i < target.Length; i++)
        {
            var enemy = target[i] as Enemy;
            if (enemy.ContainsElement(element)) enemy.DamageToShield(60);
            int dam = enemy.GetDamage(Mathf.FloorToInt(attackStat * battleSkill.damageAttr1[0]), enemy.HasShield());
            Debug.Log($"광역스킬 사용 {enemy.charName}의 체력은 {enemy.hp}/{enemy.maxHP}, 실드는 {enemy.shield}/{enemy.maxShield}");
        }
    }

    public virtual void BattleSkill(Player target)
    {
        target.hp += attackStat;
        Debug.Log($"{charName}이 {target.charName}을 {attackStat}만큼 회복 시켜 {target.hp}가 됐다.");

    }


}