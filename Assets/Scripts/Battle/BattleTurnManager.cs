using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DataManager;

public class BattleTurnManager : MonoBehaviour
{
    private bool isplayer;
    private int MaxRandom;
    public GameObject[] players; // 현재 플레이어 수
    public GameObject[] enemies; // 현재 몬스터 수
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
    
    private List<string> CharNames;

    private Character[] targets;

    //PriorityQueue<Character> queue1 = new(); 
    PriorityQueue<Character> queue = new(); 
    // 큐 -> 리스트 

    void Start()
    {
        CharNames = new List<string>();
        testPlayersData = new Player[players.Length];
        testEnemysData = new Enemy[enemies.Length];

        isCheckDie = new bool[enemies.Length];
        MaxRandom = 100;
        for (int i = 0; i < players.Length; i++)
        {
            Player playerData = players[i].GetComponent<Player>();
            testPlayersData[i] = playerData;
            queue.Enqueue(testPlayersData[i]);
        }
        for (int i = 0; i < enemies.Length; i++)
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

        SetTurnOrder();
    }

    private void TargetRayCast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit rayHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //마우스 위치
            if (Physics.Raycast(ray, out rayHit) && rayHit.collider != null)
            {
                if (rayHit.collider.gameObject.CompareTag("Enemy"))
                {
                    List<string> textName = new List<string>();

                    basicTarget = rayHit.collider.gameObject;

                    SetTurnOrder();
                }
                else if (rayHit.collider.gameObject.CompareTag("Player"))
                {
                    healTarget = rayHit.collider.gameObject;
                }
            }
        }
    }

    void SetTurnOrder()
    {
        List<Character> toList = queue.ToList();

        for (int i = 0; i < toList.Count; i++)
        {
            uIManager.TurnTextPrint(i, toList[i].charName);
        }
    }

    void MonsterAttack(Character turnMonster)
    {

        int playerCount = players.Length;
        int randomCount = Random.Range(0, playerCount);

        Debug.Log($"{turnMonster.charName}가 {players[randomCount].name}을(를) 공격함");
        StartCoroutine(HitDamage(randomCount));
        turnMonster.speed -= 100;
        queue.Enqueue(turnMonster);
        Turn();
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
            //Debug.Log($"턴 플레이어 : {turnPlayer1.charName}, {turnPlayer2.charName}, 랜덤값 : {randomResult}, 속도차이 :{turnPlayer1.speed} - {turnPlayer2.speed} = {compSpeed}, 밀린 턴 플레이어 : {turnPlayer2.charName}, 현재 턴의 플레이어 {turnPlayer1.charName}");

            queue.Enqueue(turnPlayer2);
        }
        else
        {
            //Debug.Log($"턴 플레이어 : {turnPlayer1.charName}, {turnPlayer2.charName}, 랜덤값 : {randomResult}, 속도차이 :{turnPlayer1.speed} - {turnPlayer2.speed} = {compSpeed}, 밀린 턴 플레이어 : {turnPlayer1.charName}, 현재 턴의 플레이어 {turnPlayer2.charName}");
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



    IEnumerator HitDamage(int playerCount)
    {
        int count = 0;
        while (count < 5)
        {
            yield return new WaitForSeconds(1f);
            players[playerCount].GetComponent<Renderer>().enabled = !players[playerCount].GetComponent<Renderer>().enabled;
            count++;
        }
        players[playerCount].GetComponent<Renderer>().enabled = true;
    }

    public void OnClickNormalAttack()
    {
        Player p = turnPlayer.GetComponent<Player>();
        Character charTarget = basicTarget.GetComponent<Character>();
        Enemy enemy = basicTarget.GetComponent<Enemy>();

        float finalAttack = turnPlayer.attackStat * p.normalAttack.damageAttr1[0];

        p.NormalAttack(charTarget, finalAttack);

        Debug.Log($"체력바 테스트 {enemy.name} 체력 : {enemy.hp} 실드 : {enemy.shield}");
        enemy.SetHealth();
        enemy.SetShield();
        turnPlayer.speed -= 100;
        queue.Enqueue(turnPlayer);
        if (charTarget.hp == 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] == basicTarget)
                {
                    isCheckDie[i] = false;
                }
            }

            //CharNames.Add(charTarget.charName);
            basicTarget.SetActive(false);
            
            if (isAllFalse(isCheckDie))
            {
                Debug.Log("모든 몬스터가 죽었습니다.");
                return;
            }

        }
        //basicTarget이 죽으면 다른 타겟 대상 설정해야함
        Turn();
    }



    public void OnClickSkillAttack()
    {
        Player p = turnPlayer.GetComponent<Player>();

        Character charTarget = basicTarget.GetComponent<Character>();
        Enemy EnemyTarget = basicTarget.GetComponent<Enemy>();

        Debug.Log(p.battleSkill.skillName);
        targets = new Character[enemies.Length];

        if(p.battleSkill.damageAttr1Type == SkillDataManager.DamageType.heal && healTarget != null)
        {
            Player healCharTarget = healTarget.GetComponent<Player>();
            p.BattleSkill(healCharTarget);
        }else if (p.battleSkill.range == SkillDataManager.Range.single)
        {
            p.BattleSkill(charTarget);
            EnemyTarget.SetHealth();
            EnemyTarget.SetShield();
        }
        else if (p.battleSkill.range == SkillDataManager.Range.all)
        {
            for(int i = 0;i < enemies.Length; i++)
            {
                targets[i] = enemies[i].GetComponent<Character>();
            }
            p.BattleSkill(targets);

            Debug.Log($"광역공격 {turnPlayer.charName}");
        }

        turnPlayer.speed -= 100;
        queue.Enqueue(turnPlayer);
        //basicTarget이 죽으면 다른 타겟 대상 설정해야함
        Turn();

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
            //ui 바꿔줌 몬스터 ui, 플레이어 ui
        }
        else
        {
            PlayerButton.SetActive(false);
            //이벤트 발생 -> 이벤트에서 MonsterAttack 실행

            MonsterAttack(turnPlayer);
        }
    }
}
