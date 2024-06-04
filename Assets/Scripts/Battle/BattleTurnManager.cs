using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static DataManager;

[DefaultExecutionOrder(100)]
public class BattleTurnManager : MonoBehaviour
{
    private bool isplayer;
    private int MaxRandom;
    public List<GameObject> players; // 현재 플레이어 수
    public List<GameObject> enemies; // 현재 몬스터 수
    Character turnPlayer;
    public GameObject PlayerButton;
    public GameObject basicTarget;
    public GameObject healTarget;

    [SerializeField]
    private Camera Camera;

    private UIManager uIManager;

    private bool[] isCheckDie;

    Player[] testPlayersData;
    Enemy[] testEnemysData;

    private Character[] targets;

    PriorityQueue<Character> queue = new();

    void Start()
    {
        testPlayersData = new Player[players.Count];
        testEnemysData = new Enemy[enemies.Count];

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

        Turn();
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
        var sortedCharacters = toList.OrderByDescending(c => c.finalSpeed).ToList();

        uIManager.TurnTextClear();
        for (int i = 0; i < sortedCharacters.Count; i++)
        {

            uIManager.TurnTextPrint(i, sortedCharacters[i].charName);
            Debug.Log($"{sortedCharacters[i].charName}의 속도는 {sortedCharacters[i].finalSpeed}");
        }
        Debug.Log("------------------------");
    }



    void CompareSpeed()
    {
        Character turnPlayer1 = queue.Dequeue();

        CheckDeadCharacter(turnPlayer1);

        Character turnPlayer2 = queue.Dequeue();

        CheckDeadCharacter(turnPlayer2);

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
        turnPlayer.speed -= 100;
        p.ReturnPrevFinalSpeed();
        queue.Enqueue(turnPlayer);
        SetTurnOrder();

        Debug.Log($"체력바 테스트 {enemy.name} 체력 : {enemy.hp} 실드 : {enemy.shield}");

        enemy.SetHealth();
        enemy.SetShield();


        CheckDeadChar(charTarget);
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
        }
        else if (p.battleSkill.range == SkillDataManager.Range.all)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                targets[i] = enemies[i].GetComponent<Character>();
                if (targets[i].isDead)
                {
                    //enemies[i].SetActive(false);
                }
            }
            p.BattleSkill(targets);
            turnPlayer.speed -= 100;
            p.ReturnPrevFinalSpeed();
            queue.Enqueue(turnPlayer);
            SetTurnOrder();

            Debug.Log($"광역공격 {turnPlayer.charName}");
        }

        for (int i = 0; i < targets.Length; i++)
        {
            //CheckDeadChar(targets[i]);
        }
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

    private void CheckDeadChar(Character charTarget)
    {
        if (charTarget.hp == 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == basicTarget)
                {
                    isCheckDie[i] = false;
                }
            }
            basicTarget.SetActive(false);

            if (isAllFalse(isCheckDie))
            {
                Debug.Log("모든 몬스터가 죽었습니다.");
                return;
            }

        }
    }

    public static bool isAllFalse(bool[] array)
    {
        return array.All(item => !item);
    }


    void Turn()
    {
        SetTurnOrder();

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
        }
        else
        {
            PlayerButton.SetActive(false);
            MonsterAttack(turnPlayer);
        }
    }

    IEnumerator waitOneSec()
    {
        yield return new WaitForSeconds(1);
        Turn();
    }

}
