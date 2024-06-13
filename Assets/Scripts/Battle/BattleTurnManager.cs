using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static DataManager;

[DefaultExecutionOrder(100)]
public class BattleTurnManager : MonoBehaviour
{
    bool isPlayer;
    bool isPlayer2;
    bool IsFinishGame;
    bool ISAllPlayersDead;
    int MaxRandom;
    [SerializeField]
    private List<GameObject> players; // ���� �÷��̾� ��
    [SerializeField]
    List<GameObject> enemies; // ���� ���� ��
    Character turnPlayer;
    Character turnPlayer2;
    List<Character> tempPlayers;
    [SerializeField]
    GameObject PlayerButton;
    [SerializeField]
    GameObject basicTarget;
    [SerializeField]
    GameObject healTarget;

    [SerializeField]
    Camera Camera;

    UIManager uIManager;

    bool[] isCheckDie;

    FieldCharDataManager fieldCharDataManager;

    Player[] testPlayersData;
    Enemy[] testEnemysData;

    List<Character> TurnPlayers;

    private Character[] targets;

    PriorityQueue<Character> queue = new();

    void Start()
    {
        IsFinishGame = false;
        ISAllPlayersDead = false;
        testPlayersData = new Player[players.Count];
        testEnemysData = new Enemy[enemies.Count];

        TurnPlayers = new List<Character>();
        tempPlayers = new List<Character> ();
        isCheckDie = new bool[enemies.Count];

        MaxRandom = 100;

        for (int i = 0; i < players.Count; i++)
        {
            Player playerData = players[i].GetComponent<Player>();
            testPlayersData[i] = playerData;
            queue.Enqueue(testPlayersData[i]);
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            Enemy enemyData = enemies[i].GetComponent<Enemy>();
            testEnemysData[i] = enemyData;
            queue.Enqueue(testEnemysData[i]);
        }

        uIManager = FindObjectOfType<UIManager>();
        isPlayer = false;

        //PlayerButton.SetActive(false);

        uIManager.InitTurnText(queue.Count());
        uIManager.InitTurnPlayerText(2);

        isCheckDie = new bool[enemies.Count];

        for (int i = 0; i < isCheckDie.Length; i++)
        {
            isCheckDie[i] = false;
        }
        //for (int i = 0; i < 10; i++)
        //{
        //    Deq();
        //    Debug.Log("-----------------");
        //    List<Character> toList = queue.ToList();
        //    Debug.Log("----------����-------------");
        //    for (int j = 0; j < toList.Count; j++)
        //    {
        //        Debug.Log($"{toList[j].charName}�� {j}�� °�̰� ���� �ൿ�������� {toList[j].actionGauge - toList[j].currentActionGauge} / {toList[j].actionGauge}�̴�.");
        //    }
        //}

        
        StartCoroutine(AutoTurn());
    }
    void Update()
    {
        TargetRayCast();

        if(TurnPlayers.Count >= 2)
        {
            PlayerButton.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            PlayerButton.transform.GetChild(2).gameObject.SetActive(false);
        }

        
    }

    private void TargetRayCast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit rayHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //���콺 ��ġ
            if (Physics.Raycast(ray, out rayHit) && rayHit.collider != null)
            {
                if (rayHit.collider.gameObject.CompareTag("Enemy") && basicTarget != null)
                {
                    Enemy prevTarget = basicTarget.GetComponent<Enemy>();
                    prevTarget.SetHealth();
                    prevTarget.SetShield();
                    prevTarget.ReturnPrevActionGauge();
                    basicTarget = rayHit.collider.gameObject;
                    CheckElementChose(basicTarget, TurnPlayers[0]);
                    SetTurnOrder();
                }
                else if (rayHit.collider.gameObject.CompareTag("Player"))
                {
                    healTarget = rayHit.collider.gameObject;
                }
            }
        }
    }

    void CheckElementChose(GameObject basicTarget, Character _turnPlayer)
    {
        Enemy enemy = basicTarget.GetComponent<Enemy>();
        Player player = _turnPlayer.GetComponent<Player>();

        if ((enemy.ContainsElement(player.element)))
        {
            enemy.SetPrevHpAndShield(30, player.PrevNormalSkill(true));
            enemy.SetPrevActionGauge();
        }
        else
        {
            enemy.SetPrevHpAndShield(0, player.PrevNormalSkill(false));
        }
    }

    void SetTurnOrder()
    {
        List<Character> toList = queue.ToList();
        Debug.Log("----------����-------------");
        for (int i = 0; i < toList.Count; i++)
        {
            Debug.Log($"{toList[i].charName}�� {i}�� °�̰� ���� �ൿ �������� {toList[i].actionGauge - toList[i].currentActionGauge}�̴�.");
        }
        Debug.Log("-----------��------------");
        for (int i = 0; i < toList.Count; i++)
        {
            if (toList[i].hp == 0)
            {
                toList.Remove(toList[i]);
            }
        
        }
        var sortedCharacters = toList.OrderByDescending(c => (c.currentActionGauge - c.actionGauge)).ToList();

        uIManager.TurnTextClear();
        for (int i = 0; i < sortedCharacters.Count; i++)
        {
            int actionPoint = sortedCharacters[i].actionGauge - sortedCharacters[i].currentActionGauge;
            uIManager.TurnTextPrint(i, sortedCharacters[i].charName, actionPoint);
        }
    }

    void SetTurnPlayerGroup()
    {
        uIManager.TurnPlayerTextClear();

        for (int i = 0; i < TurnPlayers.Count; i++)
        {
            uIManager.TurnPlayerTextPrint(i, TurnPlayers[i].charName);
        }
    }

    void CheckDeadCharacter(Character turnPlayer)
    {
        if (turnPlayer.hp == 0)
        {
            turnPlayer = queue.Dequeue();
            CheckDeadCharacter(turnPlayer);
        }
    }

    public void OnClickNormalAttack()
    {
        if(TurnPlayers.Count <= 0)
        {
            Debug.Log("�÷��̾� ���ʰ� �ƴմϴ�.");
            return;
        }
        Player p = TurnPlayers[0].GetComponent<Player>();
        
        Character charTarget = basicTarget.GetComponent<Character>();
        Enemy enemy = basicTarget.GetComponent<Enemy>();

        float finalAttack = TurnPlayers[0].attackStat * p.normalAttack.damageAttr1[0];

        p.NormalAttack(charTarget, finalAttack);
        if (TurnPlayers[1] != null)
        {
            int ran = Random.Range(0, 100);
            if (ran > 90)
            {
                Player p2 = TurnPlayers[1].GetComponent<Player>();
                finalAttack = TurnPlayers[1].attackStat * p2.normalAttack.damageAttr1[0] * 0.5f;
                p2.NormalAttack(charTarget, finalAttack);
            }
        }
        enemy.SetHealth();
        enemy.SetShield();
        if (enemy.hp == 0)
        {
            basicTarget.SetActive(false);
            SetTurnOrder();
            SetTurnPlayerGroup();
        }
        //TurnPlayers[0].speed -= 100;
        //p.ReturnPrevActionGauge();
        Debug.Log($"{TurnPlayers[0].charName}�� �ൿ������ {TurnPlayers[0].currentActionGauge} / {TurnPlayers[0].actionGauge}");
        queue.Enqueue(TurnPlayers[0]);
        TurnPlayers.Remove(TurnPlayers[0]);
        SetButtonName();
        SetTurnOrder();
        SetTurnPlayerGroup();

        Debug.Log($"ü�¹� �׽�Ʈ {enemy.name} ü�� : {enemy.hp} �ǵ� : {enemy.shield}");
        
        //Turn();
    }

    public void OnClickSkillAttack()
    {
        Player p = TurnPlayers[0].GetComponent<Player>();

        Character charTarget = basicTarget.GetComponent<Character>();
        Enemy EnemyTarget = basicTarget.GetComponent<Enemy>();

        Debug.Log(p.battleSkill.skillName);
        targets = new Character[enemies.Count];

        if (p.battleSkill.damageAttr1Type == SkillDataManager.DamageType.heal && healTarget != null)
        {
            Player healCharTarget = healTarget.GetComponent<Player>();
            p.BattleSkill(healCharTarget);
        }
        else if (p.battleSkill.range == SkillDataManager.Range.single)
        {
            p.BattleSkill(charTarget);
            EnemyTarget.SetHealth();
            EnemyTarget.SetShield();
            if (EnemyTarget.hp == 0)
            {
                basicTarget.SetActive(false);
                SetTurnOrder();
                SetTurnPlayerGroup();
            }
        }
        else if (p.battleSkill.range == SkillDataManager.Range.all)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                targets[i] = enemies[i].GetComponent<Character>();
                p.BattleSkill(targets[i]);
                EnemyTarget = enemies[i].GetComponent<Enemy>();
                EnemyTarget.SetHealth();
                EnemyTarget.SetShield();
                if(EnemyTarget.hp == 0)
                {
                    enemies[i].SetActive(false);
                    SetTurnOrder();
                    SetTurnPlayerGroup();
                }
            }
            Debug.Log($"�������� {TurnPlayers[0].charName}");
        }
        queue.Enqueue(TurnPlayers[0]);
        TurnPlayers.Remove(TurnPlayers[0]);
        SetButtonName();
        SetTurnOrder();
        SetTurnPlayerGroup();
    }

    public void OnCooperativeSkill()
    {
        Debug.Log($"{TurnPlayers[0].charName}�� {TurnPlayers[1].charName}�� ������ų �ߵ�");

        for (int i = 0; i < TurnPlayers.Count; i++)
        {
            queue.Enqueue(TurnPlayers[i]);
        }

        TurnPlayers.Clear();
        SetTurnPlayerGroup();
    }

    void MonsterAttack(Character turnMonster)
    {
        Enemy e = turnMonster.GetComponent<Enemy>();
        int playerCount = players.Count;
        
        int randomCount = Random.Range(0, playerCount);

        Player targetPlayerType = players[randomCount].GetComponent<Player>();
        Debug.Log($"{e.charName}�� {targetPlayerType.charName}��(��) ������");
        Debug.Log($"{e.charName}, {targetPlayerType.hp}");
        e.NormalAttack(targetPlayerType);

        targetPlayerType.SetHealth();
        if (targetPlayerType.hp == 0)
        {
            Debug.Log($"{targetPlayerType.charName}�� �׾� ����Ʈ���� �����մϴ�.");

            if (TurnPlayers.Contains(targetPlayerType))
            {
                TurnPlayers.Remove(targetPlayerType);
            }

            targetPlayerType.SetHealth();
            Debug.Log($"{players[randomCount]}");
            players.Remove(players[randomCount]);
            if(players.Count <= 0)
            {
                ISAllPlayersDead = true;
                Debug.Log($"player�� ��� �׾� �������� �й��߽��ϴ�.");
                StopCoroutine(AutoTurn());
            }
        }
    }

    private bool CheckDeadChar()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if(!enemies[i].activeSelf)
            {
                isCheckDie[i] = true;
                Debug.Log($"���� ������ üũ {enemies[i].activeSelf}");
            }
        }

        return isCheckDie.All(x => x);
    }

    public static bool isAllFalse(bool[] array)
    {
        return array.All(item => !item);
    }


    void Turn()
    {
        if (ISAllPlayersDead)
        {
            Debug.Log($"{ISAllPlayersDead} ���� �й�");
            uIManager.GameOver();
            StopAllCoroutines();
        }
        if (CheckDeadChar())
        {
            IsFinishGame = true;
            uIManager.FinishGame();
            return;
        }
        turnPlayer = Deq();
        turnPlayer2 = Deq();

        SetTurnPlayerGroup();
        CheckPlayer(turnPlayer, isPlayer);
        CheckPlayer(turnPlayer2, isPlayer2);
        // turnPlayers ����Ʈ�� ������ Ȯ���Ͽ� 2���� ������ ��ư Ȱ��ȭ
        if (isPlayer || isPlayer2)
        {
            SetButtonName();
        }

        if (turnPlayer is Enemy)
        {
            Debug.Log($"�ϸ��� ��� �� null Ȯ�� {turnPlayer.charName}");
            MonsterTurn(turnPlayer);
        }
        if(turnPlayer2 is Enemy)
        {
            Debug.Log($"�ϸ���2 ��� �� null Ȯ�� {turnPlayer2.charName}");
            MonsterTurn(turnPlayer2);
        }
        
        SetTurnOrder();
    }

    private void MonsterTurn(Character _turnPlayer)
    {
        if (ISAllPlayersDead) return;
        Debug.Log($"���� �� {_turnPlayer.charName}");
        if (_turnPlayer.hp > 0)
        {
            Debug.Log($"������ ü���� 0 �̻��� {_turnPlayer.hp}");
            MonsterAttack(_turnPlayer);
        }
    }

    private void CheckPlayer(Character _turnPlayer, bool _isPlayer)
    {
        if (_turnPlayer is Player)
        {
            _isPlayer = true;
            Debug.Log($"{_turnPlayer.charName}�� ���� {_isPlayer}");
        }
        else
        {
            _isPlayer = false;
            Debug.Log($"{_turnPlayer.charName}�� ���� {_isPlayer}");
        }
    }

    public void SetButtonName()
    {
        if(TurnPlayers.Count <= 0)
        {
            PlayerButton.GetComponentsInChildren<Text>()[0].text = "�Ϲ� ����";
            PlayerButton.GetComponentsInChildren<Text>()[1].text = "��ų ����";
            return;
        }
        Player playerButton = TurnPlayers[0].GetComponent<Player>();
        PlayerButton.GetComponentsInChildren<Text>()[0].text = playerButton.normalAttack.skillName;
        PlayerButton.GetComponentsInChildren<Text>()[1].text = playerButton.battleSkill.skillName;
    }

    private Character Deq()
    {
        Character _turnPlayer = queue.Dequeue();
        if (_turnPlayer.hp <= 0)
        {
            _turnPlayer = queue.Dequeue();
        }
        if (TurnPlayers.Count >= 2)
        {
            while (_turnPlayer is Player)
            {
                tempPlayers.Add(_turnPlayer);
                _turnPlayer = queue.Dequeue();
            }
        }

        Debug.Log($"{_turnPlayer.charName} ĳ���� ������Ʈ���� hp Ȯ�� �Ǵ��� �׽�Ʈ {_turnPlayer.hp}");

        int needGauge = 0;

        Character character = _turnPlayer.GetComponent<Character>();

        needGauge = character.actionGauge - character.currentActionGauge;
        ChargeGaue(needGauge);
        SetTurnOrder();
        if (TurnPlayers.Count < 2)
        {
            if (_turnPlayer is Player && !TurnPlayers.Contains(_turnPlayer))
            {
                _turnPlayer.InitGauge();
                TurnPlayers.Add(character);
                SetButtonName();
            }
        }

        if (_turnPlayer is Enemy)
        {
            _turnPlayer.InitGauge();
            queue.Enqueue(_turnPlayer);
        }

        for (int i = 0; i < tempPlayers.Count; i++)
        {
            queue.Enqueue(tempPlayers[i]);
        }
        tempPlayers.Clear();

        Debug.Log($"deq1 {_turnPlayer.charName} {_turnPlayer.currentActionGauge}");
        return _turnPlayer;
    }

    private void ChargeGaue(int needGauge)
    {
        if (TurnPlayers.Count < 2)
        {
            for (int i = 0; i < testPlayersData.Length; i++)
            {
                testPlayersData[i].AddGauge(needGauge);
            }
        }

        for (int i = 0; i < testEnemysData.Length; i++)
        {
            testEnemysData[i].AddGauge(needGauge);
        }
    }

    IEnumerator AutoTurn()
    {
        while (!IsFinishGame)
        {
            Debug.Log($"���ο� ��");
            Turn();
            yield return new WaitForSeconds(3.0f);
        }
    }

    //--------------------------���� ��� �ǻ�� ���ϴ� �޼���-------------------------

    void CompareSpeed()
    {
        Character turnPlayer1 = queue.Dequeue();

        //CheckDeadCharacter(turnPlayer1);

        Character turnPlayer2 = queue.Dequeue();

        //CheckDeadCharacter(turnPlayer2);

        int compSpeed = turnPlayer1.speed - turnPlayer2.speed;

        int randomResult = Random.Range(0, MaxRandom);
        int compareNum = 50 - (compSpeed * 5);
        if (randomResult > compareNum)
        {
            turnPlayer = turnPlayer1;
            queue.Enqueue(turnPlayer2);
        }
        else
        {
            turnPlayer = turnPlayer2;
            queue.Enqueue(turnPlayer1);
        }
    }








}
