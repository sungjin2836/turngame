using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleTurnManager : MonoBehaviour
{
    private bool isplayer;
    private int MaxRandom;
    public GameObject[] players; // 현재 플레이어 수
    public GameObject[] enemies; // 현재 몬스터 수
    Character turnPlayer;
    public GameObject PlayerButton;
    public GameObject basicTarget;

    private bool[] isCheckDie;

    Player[] testPlayersData;
    Enemy[] testEnemysData;


    PriorityQueue<Character> queue2 = new();

    void Start()
    {
        testPlayersData = new Player[players.Length];
        testEnemysData = new Enemy[enemies.Length];

        isCheckDie = new bool[enemies.Length];
        MaxRandom = 100;
        for(int i = 0; i < players.Length; i++)
        {
            Player playerData = players[i].GetComponent<Player>();
            testPlayersData[i] = playerData;
            queue2.Enqueue(testPlayersData[i]);
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyData = enemies[i].GetComponent<Enemy>();
            testEnemysData[i] = enemyData;
            queue2.Enqueue(testEnemysData[i]);
        }

        Debug.Log(Mathf.FloorToInt(21 * 1));

        isplayer = false;

        PlayerButton.SetActive(false);

        Turn();

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit rayHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //마우스 위치
            if (Physics.Raycast(ray, out rayHit))
            {
                if (rayHit.collider != null)
                {
                    if (rayHit.collider.gameObject.CompareTag("Enemy"))
                    {
                        basicTarget = rayHit.collider.gameObject;
                    }
                }
            }
        }
    }

    void MonsterAttack(Character turnMonster)
    {

        int playerCount = players.Length;
        int randomCount = Random.Range(0, playerCount);

        Debug.Log($"{turnMonster.charName}가 {players[randomCount].name}을(를) 공격함");
        StartCoroutine(HitDamage(randomCount));
        turnMonster.speed -= 100;
        queue2.Enqueue(turnMonster);
        Turn();
    }
    void CompareSpeed()
    {
        Character turnPlayer1 = queue2.Dequeue();
        Character turnPlayer2 = queue2.Dequeue();

        int compSpeed = turnPlayer1.speed - turnPlayer2.speed;

        int randomResult = Random.Range(0, MaxRandom);
        int compareNum = 50 - (compSpeed * 5);
        if (randomResult > compareNum)
        {
            turnPlayer = turnPlayer1;
            Debug.Log($"턴 플레이어 : {turnPlayer1.charName}, {turnPlayer2.charName}, 랜덤값 : {randomResult}, 속도차이 :{turnPlayer1.speed} - {turnPlayer2.speed} = {compSpeed}, 밀린 턴 플레이어 : {turnPlayer2.charName}, 현재 턴의 플레이어 {turnPlayer1.charName}");

            queue2.Enqueue(turnPlayer2);
        }
        else
        {
            Debug.Log($"턴 플레이어 : {turnPlayer1.charName}, {turnPlayer2.charName}, 랜덤값 : {randomResult}, 속도차이 :{turnPlayer1.speed} - {turnPlayer2.speed} = {compSpeed}, 밀린 턴 플레이어 : {turnPlayer1.charName}, 현재 턴의 플레이어 {turnPlayer2.charName}");
            turnPlayer = turnPlayer2;
            queue2.Enqueue(turnPlayer1);
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
        UIManager uIManager = FindObjectOfType<UIManager>();
        Player p = new Player();

        Character charTarget = basicTarget.GetComponent<Character>();

        Enemy enemy = basicTarget.GetComponent<Enemy>();
        p.NormalAttack(charTarget, turnPlayer.attackStat);

        int _hp = enemy.hp;
        int _shield = enemy.shield;

        Debug.Log($"체력바 테스트 {enemy.name} 체력 : {enemy.hp} 실드 : {enemy.shield}");
        enemy.SetHealth();
        enemy.SetShield();

        turnPlayer.speed -= 100;
        queue2.Enqueue(turnPlayer);
        if (charTarget.hp == 0)
        {
            for (int i = 0; i < enemies.Length; i++)
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
        //basicTarget이 죽으면 다른 타겟 대상 설정해야함
        Turn();
    }



    public void OnClickSkillAttack()
    {
        Player p = new Player();
        Character charTarget = basicTarget.GetComponent<Character>();
        p.BattleSkill(charTarget, 2f);
        turnPlayer.speed -= 100;
        queue2.Enqueue(turnPlayer);
        //basicTarget이 죽으면 다른 타겟 대상 설정해야함
        Turn();

    }

    public static bool isAllFalse(bool[] array)
    {
        return array.All(item => !item);
    }


    void Turn()
    {
        if (queue2.Count() > 0)
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
