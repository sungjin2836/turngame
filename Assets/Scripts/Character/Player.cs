using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static CooperativeSkillDataManager;
using static SkillDataManager;

public class Player : Character
{
    [Header("플레이어 캐릭터 정보")] public ElementType element;

    public Skill normalAttack;
    public Skill battleSkill;

    public CooperativeSkill cooperativeSkill1;
    public CooperativeSkill cooperativeSkill2;
    public CooperativeSkill cooperativeSkill3;


    public Slider playerHpBar;

    private Camera mainCamera;

    private UserDataManager.OwnedCharacter LoadUserOwnedCharacter(int id)
    {
        var characters = UserDataManager.Instance.UserData.ownedCharacter;
        var character = characters.FirstOrDefault(x => x.characterID == id);
        return character;
    }

    public override void Initialize(int id)
    {
        var defaultPlayerData = DataManager.Instance.GetPlayerData(id);
        var userOwnedCharacter = LoadUserOwnedCharacter(id);
        charName = defaultPlayerData.charName;
        level = userOwnedCharacter.currentLevel;
        maxHP = defaultPlayerData.hp + level * 6;
        speed = defaultPlayerData.speed;
        attackStat = defaultPlayerData.attackStat + level * 4;
        element = defaultPlayerData.elem;
        actionGauge = Mathf.FloorToInt(10000 / defaultPlayerData.speed);

        Debug.Log("여기까지 되나1");

        SkillDataManager.Skill normalAttackData = SkillDataManager.Instance.GetSkillData($"{id}001");
        SkillDataManager.Skill battleSkillData = SkillDataManager.Instance.GetSkillData($"{id}002");

        CooperativeSkillDataManager.CooperativeSkill cooperativeSkillData1 = CooperativeSkillDataManager.Instance.GetCoSkillData($"{id}1001");
        CooperativeSkillDataManager.CooperativeSkill cooperativeSkillData2 = CooperativeSkillDataManager.Instance.GetCoSkillData($"{id}1002");
        CooperativeSkillDataManager.CooperativeSkill cooperativeSkillData3 = CooperativeSkillDataManager.Instance.GetCoSkillData($"{id}1003");

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

        Debug.Log("여기까지 되나2");

        cooperativeSkill1 = InitCooperativeSkillData(cooperativeSkillData1);
        cooperativeSkill2 = InitCooperativeSkillData(cooperativeSkillData2);
        cooperativeSkill3 = InitCooperativeSkillData(cooperativeSkillData3);

        hp = maxHP;
        finalSpeed = speed;
        finalAttackStat = attackStat;
        currentActionGauge = 1;
        //Debug.Log("파이널 어택"+finalAttackStat);

        mainCamera = Camera.main;

        SetMaxHealth();
        SetHpBarPosition();
        Debug.Log($"{currentActionGauge} / {actionGauge}");

    }

    private CooperativeSkill InitCooperativeSkillData(CooperativeSkillDataManager.CooperativeSkill cooperativeSkillData1)
    {
        CooperativeSkill _cooperativeSkill = gameObject.AddComponent<CooperativeSkill>();
        _cooperativeSkill.cooperativeSkillName = cooperativeSkillData1.name;
        _cooperativeSkill.cooperativeId = cooperativeSkillData1.cooperativeId;
        _cooperativeSkill.range1 = cooperativeSkillData1.range1;
        _cooperativeSkill.range2 = cooperativeSkillData1.range2;
        _cooperativeSkill.damageAttr1 = cooperativeSkillData1.damageAttr1;
        _cooperativeSkill.damageAttr2 = cooperativeSkillData1.damageAttr2;
        _cooperativeSkill.damageAttr1Type = cooperativeSkillData1.damageAttr1Type;
        _cooperativeSkill.damageAttr2Type = cooperativeSkillData1.damageAttr2Type;
        _cooperativeSkill.distance1 = cooperativeSkillData1.distance1;
        _cooperativeSkill.distance2 = cooperativeSkillData1.distance2;

        return _cooperativeSkill;
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
        //Debug.Log($"{enemy.charName}의 체력은 {enemy.hp}/{enemy.maxHP}, 실드는 {enemy.shield}/{enemy.maxShield}");

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

    public void CooperativeSkillAttack(List<Character> _players, Character _charTarget, List<GameObject> enemies)
    {
        int partyMemberId = _players[1].gameObject.GetComponent<CharacterData>().CharacterID;
        
        CooperativeSkill currentCooperativeSkill = new CooperativeSkill();

        currentCooperativeSkill = CheckCooperativeID(partyMemberId);

        if(currentCooperativeSkill == null)
        {
            Debug.Log($"현재 협동스킬이 널값임 {currentCooperativeSkill}");
            return;
        }

        switch (currentCooperativeSkill.damageAttr1Type)
        {
            case (CooperativeSkillDataManager.DamageType.attack):
                switch (currentCooperativeSkill.damageAttr2Type)
                {
                    case (CooperativeSkillDataManager.DamageType.attack):
                        CoSkillDoubleAttack(currentCooperativeSkill);
                        break;
                    case (CooperativeSkillDataManager.DamageType.heal):
                        CoSkillAttackAndHeal(currentCooperativeSkill);
                        break;
                }
                break;
            case (CooperativeSkillDataManager.DamageType.heal):
                switch (currentCooperativeSkill.damageAttr2Type)
                {
                    case (CooperativeSkillDataManager.DamageType.attack):
                        CoSkillHealAndAttack(currentCooperativeSkill);
                        break;
                    case (CooperativeSkillDataManager.DamageType.heal):
                        CoSkillDoubleHeal(currentCooperativeSkill);
                        break;
                }
                break;
            
        }
        // 광역 딜, 광역 딜
        // 광역 딜, 광역 힐
        // 광역 딜, 단일 딜
        // 단일 딜, 광역 딜
        // 단일 딜, 단일 힐
        // 단일 딜, 단일 딜
        // 단일 힐, 광역 딜
        // 단일 힐, 단일 딜
    }

    private void CoSkillDoubleHeal(CooperativeSkill currentCooperativeSkill)
    {
        switch (currentCooperativeSkill.range1)
        {
            case (CooperativeSkillDataManager.Range.single):
                CoSkillDoubleSingleHeal(currentCooperativeSkill);
                break;
            case (CooperativeSkillDataManager.Range.all):
                CoSkillDoubleAllHeal(currentCooperativeSkill);
                break;
        }
    }

    private static void CoSkillDoubleAllHeal(CooperativeSkill currentCooperativeSkill)
    {
        switch (currentCooperativeSkill.range2)
        {
            case (CooperativeSkillDataManager.Range.single):
                break;
            case (CooperativeSkillDataManager.Range.all):
                break;
        }
    }

    private static void CoSkillDoubleSingleHeal(CooperativeSkill currentCooperativeSkill)
    {
        switch (currentCooperativeSkill.range2)
        {
            case (CooperativeSkillDataManager.Range.single):

                break;
            case (CooperativeSkillDataManager.Range.all):
                break;
        }
    }

    private void CoSkillHealAndAttack(CooperativeSkill currentCooperativeSkill)
    {
        switch (currentCooperativeSkill.range1)
        {
            case (CooperativeSkillDataManager.Range.single):
                switch (currentCooperativeSkill.range2)
                {
                    case (CooperativeSkillDataManager.Range.single):
                        break;
                    case (CooperativeSkillDataManager.Range.all):
                        break;
                }
                break;
            case (CooperativeSkillDataManager.Range.all):
                switch (currentCooperativeSkill.range2)
                {
                    case (CooperativeSkillDataManager.Range.single):
                        break;
                    case (CooperativeSkillDataManager.Range.all):
                        break;
                }
                break;
        }
    }

    private void CoSkillAttackAndHeal(CooperativeSkill currentCooperativeSkill)
    {
        switch (currentCooperativeSkill.range1)
        {
            case (CooperativeSkillDataManager.Range.single):
                switch (currentCooperativeSkill.range2)
                {
                    case (CooperativeSkillDataManager.Range.single):
                        break;
                    case (CooperativeSkillDataManager.Range.all):
                        break;
                }
                break;
            case (CooperativeSkillDataManager.Range.all):
                switch (currentCooperativeSkill.range2)
                {
                    case (CooperativeSkillDataManager.Range.single):
                        break;
                    case (CooperativeSkillDataManager.Range.all):
                        break;
                }
                break;
        }
    }

    private static void CoSkillDoubleAttack(CooperativeSkill currentCooperativeSkill)
    {
        switch (currentCooperativeSkill.range1)
        {
            case (CooperativeSkillDataManager.Range.single):
                switch (currentCooperativeSkill.range2)
                {
                    case (CooperativeSkillDataManager.Range.single):
                        // 
                        break;
                    case (CooperativeSkillDataManager.Range.all):
                        break;
                }
                break;
            case (CooperativeSkillDataManager.Range.all):
                switch (currentCooperativeSkill.range2)
                {
                    case (CooperativeSkillDataManager.Range.single):
                        break;
                    case (CooperativeSkillDataManager.Range.all):
                        break;
                }
                break;
        }
    }

    private CooperativeSkill CheckCooperativeID(int partyMemberId)
    {
        if (partyMemberId == cooperativeSkill1.cooperativeId)
        {
            return cooperativeSkill1;
        }
        else if (partyMemberId == cooperativeSkill2.cooperativeId)
        {
            return cooperativeSkill2;
        }
        else if (partyMemberId == cooperativeSkill3.cooperativeId)
        {
            return cooperativeSkill3;
        }
        return null;
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
            gameObject.SetActive(false);
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