using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static CooperativeSkillDataManager;
using static SkillDataManager;

public class Player : Character
{
    [Header("플레이어 캐릭터 정보")]
    public ElementType element;

    public Skill normalAttack;
    public Skill battleSkill;

    public CooperativeSkill cooperativeSkill1;
    public CooperativeSkill cooperativeSkill2;
    public CooperativeSkill cooperativeSkill3;


    public Slider playerHpBar;
    public Image playerImage;

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
        maxHP = defaultPlayerData.hp + level * STAT_HP;
        speed = defaultPlayerData.speed;
        attackStat = defaultPlayerData.attackStat + level * STAT_ATTACK;
        element = defaultPlayerData.elem;
        actionGauge = Mathf.FloorToInt(10000 / defaultPlayerData.speed);

        Debug.Log("해치웠나? 1");
        //Debug.Log("여기까지 되나1");

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

        Debug.Log("해치웠나? 2");
        //Debug.Log("여기까지 되나2");

        cooperativeSkill1 = InitCooperativeSkillData(cooperativeSkillData1);
        cooperativeSkill2 = InitCooperativeSkillData(cooperativeSkillData2);
        cooperativeSkill3 = InitCooperativeSkillData(cooperativeSkillData3);

        hp = maxHP;
        finalSpeed = speed;
        finalAttackStat = attackStat;
        currentActionGauge = 1;

        mainCamera = Camera.main;

        SetMaxHealth();
        //SetHpBarPosition();
        Debug.Log($"{currentActionGauge} / {actionGauge}");

        playerImage.color = Color.white;

        SetColor(playerImage);

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
        BattleCamera.MoveTo("Attack Camera", transform, target.transform);
        TargetPos = target.startPos + target.transform.forward;
        Debug.Log("NormalAttack의 공격력" + finalAttackStat);
        var enemy = target as Enemy;
        if (enemy.ContainsElement(element)) enemy.DamageToShield(30);
        int dam = enemy.GetDamage(Mathf.FloorToInt(finalAttackStat * normalAttack.damageAttr1[0] * 0.9f), enemy.HasShield());
        enemy.SetDamageText(dam.ToString());
        enemy.SetHealth();
        //Debug.Log($"{enemy.charName}의 체력은 {enemy.hp}/{enemy.maxHP}, 실드는 {enemy.shield}/{enemy.maxShield}");

        return dam;
    }


    public virtual void BattleSkill(Character target)
    {
        BattleCamera.MoveTo("Attack Camera", transform, target.transform);
        TargetPos = target.startPos + target.transform.forward;
        // 스킬
        var enemy = target as Enemy;
        if (enemy.ContainsElement(element)) enemy.DamageToShield(60);
        int dam = enemy.GetDamage(Mathf.FloorToInt(attackStat * battleSkill.damageAttr1[0]), enemy.HasShield());
        enemy.SetDamageText(dam.ToString());
        Debug.Log($"스킬 사용 {enemy.charName}의 체력은 {enemy.hp}/{enemy.maxHP}, 실드는 {enemy.shield}/{enemy.maxShield} 죽음 여부{enemy.isDead}");
    }

    public virtual void BattleSkill(Character[] target)
    {
        // 스킬
        for (int i = 0; i < target.Length; i++)
        {
            var enemy = target[i] as Enemy;
            if (enemy.ContainsElement(element)) enemy.DamageToShield(60);
            int dam = enemy.GetDamage(Mathf.FloorToInt(attackStat * battleSkill.damageAttr1[0]), enemy.HasShield());
            enemy.SetDamageText(dam.ToString());
            enemy.SetHealth();
            enemy.SetShield();
            Debug.Log($"광역스킬 사용 {enemy.charName}의 체력은 {enemy.hp}/{enemy.maxHP}, 실드는 {enemy.shield}/{enemy.maxShield} 죽음 여부 {enemy.isDead}");
        }
    }

    public int PrevNormalSkill(bool isSameElement)
    {
        int damage;
        if (isSameElement)
        {
            damage = Mathf.FloorToInt(attackStat * normalAttack.damageAttr1[0]);
        }
        else
        {
            damage = Mathf.FloorToInt(attackStat * normalAttack.damageAttr1[0] * 0.9f);
        }

        return damage;
    }

    public int PrevBattleSkill()
    {
        int dam = Mathf.FloorToInt(attackStat * battleSkill.damageAttr1[0] * 0.9f);
        return dam;
    }

    public virtual void BattleSkill(Player target)
    {
        BattleCamera.MoveTo("Heal Camera", target.transform, target.transform);
        TargetPos = transform.position + transform.up;
        int healamount = Mathf.FloorToInt(maxHP * battleSkill.damageAttr1[0]);

        target.hp += Mathf.FloorToInt(healamount);
        target.SetHealth();
        //Debug.Log($"{charName}이 {target.charName}을 {healamount}만큼 회복 시켜 {target.hp}가 됐다.");
    }

    public CooperativeSkill CooperativeSkillAttack(List<Character> _turnplayers, Character _charTarget, Character[] targets, Player[] _players, Player _healCharTarget)
    {
        int partyMemberId = _turnplayers[1].gameObject.GetComponent<CharacterData>().CharacterID;

        CooperativeSkill currentCooperativeSkill = gameObject.GetComponent<CooperativeSkill>();

        currentCooperativeSkill = CheckCooperativeID(partyMemberId);

        if (currentCooperativeSkill == null)
        {
            Debug.Log($"현재 협동스킬이 널값임 {currentCooperativeSkill}");
            return null;
        }

        if (currentCooperativeSkill.damageAttr1Type == CooperativeSkillDataManager.DamageType.attack)
        {
            CooperativeSkillAttack(_charTarget, targets, currentCooperativeSkill);
        }
        else if (currentCooperativeSkill.damageAttr1Type == CooperativeSkillDataManager.DamageType.heal)
        {
            if (currentCooperativeSkill.range1 == CooperativeSkillDataManager.Range.all)
            {
                CooperativeSkillHeal(_players, currentCooperativeSkill);
            }
            else if(currentCooperativeSkill.range1 == CooperativeSkillDataManager.Range.single)
            {
                Player[] healCharTarget = { _healCharTarget};
                CooperativeSkillHeal(healCharTarget, currentCooperativeSkill);
            }
        }

        return currentCooperativeSkill;



        //switch (currentCooperativeSkill.damageAttr1Type)
        //{
        //    case (CooperativeSkillDataManager.DamageType.attack):
        //        switch (currentCooperativeSkill.damageAttr2Type)
        //        {
        //            case (CooperativeSkillDataManager.DamageType.attack):
        //                CoSkillDoubleAttack(currentCooperativeSkill);
        //                break;
        //            case (CooperativeSkillDataManager.DamageType.heal):
        //                CoSkillAttackAndHeal(currentCooperativeSkill);
        //                break;
        //        }
        //        break;
        //    case (CooperativeSkillDataManager.DamageType.heal):
        //        switch (currentCooperativeSkill.damageAttr2Type)
        //        {
        //            case (CooperativeSkillDataManager.DamageType.attack):
        //                CoSkillHealAndAttack(currentCooperativeSkill);
        //                break;
        //            case (CooperativeSkillDataManager.DamageType.heal):
        //                CoSkillDoubleHeal(currentCooperativeSkill);
        //                break;
        //        }
        //        break;

        //}
        // 광역 딜, 광역 딜
        // 광역 딜, 광역 힐
        // 광역 딜, 단일 딜
        // 단일 딜, 광역 딜
        // 단일 딜, 단일 힐
        // 단일 딜, 단일 딜
        // 단일 힐, 광역 딜
        // 단일 힐, 단일 딜
    }

    private void CooperativeSkillHeal(Player[] _players, CooperativeSkill currentCooperativeSkill)
    {
        int healamount = Mathf.FloorToInt(maxHP * currentCooperativeSkill.damageAttr1[0]);

        for (int i = 0; i < _players.Length; i++)
        {
            _players[i].hp += Mathf.FloorToInt(healamount);
            _players[i].SetHealth();
            Debug.Log($"{charName}이 {_players[i].charName}을 {healamount}만큼 회복 시켜 {_players[i].hp}가 됐다.");
        }
    }

    private void CooperativeSkillAttack(Character _charTarget, Character[] targets, CooperativeSkill currentCooperativeSkill)
    {
        if (currentCooperativeSkill.range1 == CooperativeSkillDataManager.Range.all)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                var enemy = targets[i] as Enemy;
                if (enemy.ContainsElement(element)) enemy.DamageToShield(60);
                int dam = enemy.GetDamage(Mathf.FloorToInt(attackStat * currentCooperativeSkill.damageAttr1[0]), enemy.HasShield());
                enemy.SetDamageText(dam.ToString());
                enemy.SetHealth();
                enemy.SetShield();
                Debug.Log($"협동스킬 사용 {enemy.charName}의 체력은 {enemy.hp}/{enemy.maxHP}, 실드는 {enemy.shield}/{enemy.maxShield}이고 데미지는 {dam}이다.");
            }
        }
        else if (currentCooperativeSkill.range1 == CooperativeSkillDataManager.Range.single)
        {
            TargetPos = _charTarget.startPos + _charTarget.transform.forward;

            // 스킬
            var enemy = _charTarget as Enemy;
            if (enemy.ContainsElement(element)) enemy.DamageToShield(60);
            int dam = enemy.GetDamage(Mathf.FloorToInt(attackStat * currentCooperativeSkill.damageAttr1[0]), enemy.HasShield());
            enemy.SetDamageText(dam.ToString());
            Debug.Log($"스킬 사용 {enemy.charName}의 체력은 {enemy.hp}/{enemy.maxHP}, 실드는 {enemy.shield}/{enemy.maxShield}");
        }
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
        //Debug.Log($"sethealth 매개변수 : {hp}, 실제 체력 {_hp}");
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

    protected override void TurnEnd()
    {
        base.TurnEnd();
        BattleCamera.MoveTo("Ready Camera");
    }

    private void SetColor(Image _image)
    {
        switch (charName)
        {
            case "개척자":
                _image.color = Color.gray;
                break;
            case "삼칠이":
                Color _color = new Vector4(0, 213, 255, 255);
                _image.color = _color;
                break;
            case "단항":
                _image.color = Color.green;
                break;
            case "히메코":
                _image.color = Color.red;
                break;
            default: break;
        }
    }

    public void SelectHealTarget()
    {
        BattleTurnManager battleTurnManager = GameObject.FindObjectOfType<BattleTurnManager>();
        // 배틀매니저의 기존 인스턴스를 가져옴

        battleTurnManager.SelectHealTarget(this.gameObject);
    }


}