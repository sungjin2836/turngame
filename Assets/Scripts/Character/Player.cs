using UnityEngine;
using UnityEngine.UI;
using static SkillDataManager;

public class Player : Character
{
    [Header("플레이어 캐릭터 정보")] public ElementType element;

    public Skill normalAttack;
    public Skill battleSkill;

    public Slider playerHpBar;

    private Camera mainCamera;

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
        //Debug.Log("파이널 어택"+finalAttackStat);

        mainCamera = Camera.main;

        SetMaxHealth();
        SetHpBarPosition();

    }

    public int hp
    {
        get => _hp;

        private set => _hp = Mathf.Clamp(value, 0, maxHP);
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
            enemy.SetHealth();
            enemy.SetShield();
            Debug.Log($"광역스킬 사용 {enemy.charName}의 체력은 {enemy.hp}/{enemy.maxHP}, 실드는 {enemy.shield}/{enemy.maxShield}");
        }
    }

    public int PrevNormalSkill(bool isSameElement)
    {
        int dam;
        if (isSameElement)
        {
            dam = Mathf.FloorToInt(attackStat * normalAttack.damageAttr1[0]);
            
        }
        else
        {
            dam = Mathf.FloorToInt(attackStat * normalAttack.damageAttr1[0] * 0.9f);
        }

        return dam;
    }

    public int PrevBattleSkill()
    {
        int dam = Mathf.FloorToInt(attackStat * battleSkill.damageAttr1[0] * 0.9f);
        return dam;
    }

    public virtual void BattleSkill(Player target)
    {
        int healamount = Mathf.FloorToInt(maxHP * battleSkill.damageAttr1[0]);

        target.hp += Mathf.FloorToInt(healamount);
        target.SetHealth();
        Debug.Log($"{charName}이 {target.charName}을 {healamount}만큼 회복 시켜 {target.hp}가 됐다.");
    }

    public void SetMaxHealth()
    {
        playerHpBar.maxValue = maxHP;
        playerHpBar.value = hp;
    }
    public void SetHealth()
    {
        Debug.Log($"sethealth 매개변수 : {hp}, 실제 체력 {_hp}");
        playerHpBar.value = hp;
        if (hp == 0)
        {
            playerHpBar.gameObject.SetActive(false);
            playerHpBar.gameObject.SetActive(false);
        }
    }

    private void SetHpBarPosition()
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(transform.position + Vector3.up * 1.2f);
        playerHpBar.transform.position = screenPosition;
    }

    public void ReturnPrevFinalSpeed()
    {
        finalSpeed = speed;
    }

}