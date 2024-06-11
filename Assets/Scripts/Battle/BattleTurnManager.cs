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
    bool isplayer;
    int MaxRandom;
    [SerializeField]
    List<GameObject> players; // 현재 플레이어 수
    [SerializeField]
    List<GameObject> enemies; // 현재 몬스터 수
    Character turnPlayer;
    Character turnPlayer2;
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
        testPlayersData = new Player[players.Count];
        testEnemysData = new Enemy[enemies.Count];

        TurnPlayers = new List<Character>();

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
        isplayer = false;

        PlayerButton.SetActive(false);

        uIManager.InitTurnText(queue.Count());

        isCheckDie = new bool[enemies.Count];

        for (int i = 0; i < isCheckDie.Length; i++)
        {
            isCheckDie[i] = false;
        }
        for (int i = 0; i < 10; i++)
        {
            Deq();
            Debug.Log("-----------------");
            List<Character> toList = queue.ToList();
            Debug.Log("----------시작-------------");
            for (int j = 0; j < toList.Count; j++)
            {
                Debug.Log($"{toList[j].charName}은 {j}번 째이고 현재 행동게이지는 {toList[j].actionGauge - toList[j].currentActionGauge} / {toList[j].actionGauge}이다.");
            }
        }

        for (int i = 0; i < 10; i++)
        {
            Deq();
        }

        foreach (Character item in TurnPlayers)
        {
            Debug.Log(item.charName);
        }

        //Turn();
    }
    void Update()
    {
        TargetRayCast();
    }

    private void TargetRayCast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit rayHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //마우스 위치
            if (Physics.Raycast(ray, out rayHit) && rayHit.collider != null)
            {
                if (rayHit.collider.gameObject.CompareTag("Enemy") && basicTarget != null)
                {
                    Enemy prevTarget = basicTarget.GetComponent<Enemy>();
                    prevTarget.SetHealth();
                    prevTarget.SetShield();
                    prevTarget.ReturnPrevFinalSpeed();
                    basicTarget = rayHit.collider.gameObject;
                    CheckElementChose(basicTarget);
                    SetTurnOrder();
                }
                else if (rayHit.collider.gameObject.CompareTag("Player"))
                {
                    healTarget = rayHit.collider.gameObject;
                }
            }
        }
    }

    void CheckElementChose(GameObject basicTarget)
    {
        Enemy enemy = basicTarget.GetComponent<Enemy>();
        Player player = turnPlayer.GetComponent<Player>();

        if ((enemy.ContainsElement(player.element)))
        {
            enemy.SetPrevHpAndShield(30, player.PrevNormalSkill(true));
            enemy.SetPrevFinalSpeed();
        }
        else
        {
            enemy.SetPrevHpAndShield(0, player.PrevNormalSkill(false));
        }
    }

    void SetTurnOrder()
    {
        List<Character> toList = queue.ToList();
        Debug.Log("----------시작-------------");
        for (int i = 0; i < toList.Count; i++)
        {
            Debug.Log($"{toList[i].charName}은 {i}번 째이고 속도는 {toList[i].speed}이다.");
        }
        Debug.Log("-----------끝------------");
        for (int i = 0; i < toList.Count; i++)
        {
            if (toList[i].hp == 0)
            {
                toList.Remove(toList[i]);
            }
        
        }
        var sortedCharacters = toList.OrderByDescending(c => c.finalSpeed).ToList();

        uIManager.TurnTextClear();
        for (int i = 0; i < sortedCharacters.Count; i++)
        {

            uIManager.TurnTextPrint(i, sortedCharacters[i].charName);
        }
    }



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
        Player p = turnPlayer.GetComponent<Player>();
        Character charTarget = basicTarget.GetComponent<Character>();
        Enemy enemy = basicTarget.GetComponent<Enemy>();

        float finalAttack = turnPlayer.attackStat * p.normalAttack.damageAttr1[0];

        p.NormalAttack(charTarget, finalAttack);
        enemy.SetHealth();
        enemy.SetShield();
        if (enemy.hp == 0)
        {
            basicTarget.SetActive(false);
        }
        turnPlayer.speed -= 100;
        p.ReturnPrevFinalSpeed();
        queue.Enqueue(turnPlayer);
        SetTurnOrder();

        Debug.Log($"체력바 테스트 {enemy.name} 체력 : {enemy.hp} 실드 : {enemy.shield}");
        
        Turn();
    }

    public void OnClickSkillAttack()
    {
        Player p = turnPlayer.GetComponent<Player>();

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
                }
            }
            Debug.Log($"광역공격 {turnPlayer.charName}");
        }
        turnPlayer.speed -= 100;
        queue.Enqueue(turnPlayer);
        SetTurnOrder();
        p.ReturnPrevFinalSpeed();
        StartCoroutine(waitOneSec());
    }
    void MonsterAttack(Character turnMonster)
    {
        Enemy e = turnMonster.GetComponent<Enemy>();
        int playerCount = players.Count;
        int randomCount = Random.Range(0, playerCount);

        Player targetPlayerType = players[randomCount].GetComponent<Player>();

        Debug.Log($"{turnMonster.charName}가 {targetPlayerType.charName}을(를) 공격함");
        e.NormalAttack(targetPlayerType);

        targetPlayerType.SetHealth();
        if (targetPlayerType.hp == 0)
        {
            targetPlayerType.SetHealth();
        }
        turnMonster.speed -= 100;
        e.ReturnPrevFinalSpeed();
        queue.Enqueue(turnMonster);
        SetTurnOrder();
        StartCoroutine(waitOneSec());
    }

    private bool CheckDeadChar()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if(!enemies[i].activeSelf)
            {
                isCheckDie[i] = true;
                Debug.Log($"몬스터 죽은거 체크 {enemies[i].activeSelf}");
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
        if (CheckDeadChar())
        {
            uIManager.FinishGame();
            return;
        }

        SetTurnOrder();

        //turnPlayer = queue.Dequeue();
        if (queue.Count() > 0)
        {
            CompareSpeed();
        }
        else
        {
            Debug.Log("turn에 저장된 데이터가 없음");
            return;
        }
        if (turnPlayer is Player)
        {
            isplayer = true;
            Debug.Log($"{turnPlayer.charName}의 차례");

        }
        else
        {
            isplayer = false;
        }

        //턴 진행
        if (isplayer)
        {
            PlayerButton.SetActive(true);
            SetButtonName();
        }
        else
        {
            PlayerButton.SetActive(false);
            if(turnPlayer.hp > 0)
            {
            MonsterAttack(turnPlayer);
            }
            else
            {
                Turn();
            }
        }
    }

    IEnumerator waitOneSec()
    {
        yield return new WaitForSeconds(1);
        Turn();
    }

    public void SetButtonName()
    {
        Player playerButton = turnPlayer.GetComponent<Player>();
        PlayerButton.GetComponentsInChildren<Text>()[0].text = playerButton.normalAttack.skillName;
        PlayerButton.GetComponentsInChildren<Text>()[1].text = playerButton.battleSkill.skillName;
    }

    void Deq()
    {
        //turnPlayer1
        turnPlayer = queue.Dequeue();

        int needGauge = 0;

        Character character = turnPlayer.GetComponent<Character>();

        needGauge = character.actionGauge - character.currentActionGauge;

        for (int i = 0; i < testPlayersData.Length; i++)
        {
            testPlayersData[i].AddGauge(needGauge);
        }
        for (int i = 0; i < testEnemysData.Length; i++)
        {
            testEnemysData[i].AddGauge(needGauge);
        }
        turnPlayer.InitGauge();

        if (turnPlayer is Player)
        {
            TurnPlayers.Add(character);
            queue.Enqueue(turnPlayer); // 플레이어 행동 구현시 빠지고 플레이어 행동 후에 들어가게 됨
        }
        else
        {
            queue.Enqueue(turnPlayer);
        }

        // turnPlayer2
        turnPlayer2 = queue.Dequeue();

        Character character2 = turnPlayer2.GetComponent<Character>();

        needGauge = character2.actionGauge - character2.currentActionGauge;

        for (int i = 0; i < testPlayersData.Length; i++)
        {
            testPlayersData[i].AddGauge(needGauge);
        }
        for (int i = 0; i < testEnemysData.Length; i++)
        {
            testEnemysData[i].AddGauge(needGauge);
        }
        turnPlayer2.InitGauge();

        if (turnPlayer2 is Player)
        {
            TurnPlayers.Add(character2);
            queue.Enqueue(turnPlayer2);
        }
        else
        {
            queue.Enqueue(turnPlayer2);
        }

        Debug.Log($"deq1 {turnPlayer.charName} {turnPlayer.currentActionGauge}");
        Debug.Log($"deq2 {turnPlayer2.charName} {turnPlayer2.currentActionGauge}");
    }
}
