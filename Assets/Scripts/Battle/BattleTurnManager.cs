using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleTurnManager : MonoBehaviour
{
    private bool isplayer;
    private int MaxRandom;
    public GameObject[] players; // ���� �÷��̾� ��
    public GameObject[] enemies; // ���� ���� ��
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //���콺 ��ġ
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

        Debug.Log($"{turnMonster.charName}�� {players[randomCount].name}��(��) ������");
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
            Debug.Log($"�� �÷��̾� : {turnPlayer1.charName}, {turnPlayer2.charName}, ������ : {randomResult}, �ӵ����� :{turnPlayer1.speed} - {turnPlayer2.speed} = {compSpeed}, �и� �� �÷��̾� : {turnPlayer2.charName}, ���� ���� �÷��̾� {turnPlayer1.charName}");

            queue2.Enqueue(turnPlayer2);
        }
        else
        {
            Debug.Log($"�� �÷��̾� : {turnPlayer1.charName}, {turnPlayer2.charName}, ������ : {randomResult}, �ӵ����� :{turnPlayer1.speed} - {turnPlayer2.speed} = {compSpeed}, �и� �� �÷��̾� : {turnPlayer1.charName}, ���� ���� �÷��̾� {turnPlayer2.charName}");
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

        Debug.Log($"ü�¹� �׽�Ʈ {enemy.name} ü�� : {enemy.hp} �ǵ� : {enemy.shield}");
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
                Debug.Log("��� ���Ͱ� �׾����ϴ�.");
                return;
            }

        }
        //basicTarget�� ������ �ٸ� Ÿ�� ��� �����ؾ���
        Turn();
    }



    public void OnClickSkillAttack()
    {
        Player p = new Player();
        Character charTarget = basicTarget.GetComponent<Character>();
        p.BattleSkill(charTarget, 2f);
        turnPlayer.speed -= 100;
        queue2.Enqueue(turnPlayer);
        //basicTarget�� ������ �ٸ� Ÿ�� ��� �����ؾ���
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
            Debug.Log("turn�� ����� �����Ͱ� ����");
            return;
        }
        if (turnPlayer is Player)
        {
            isplayer = true;
            Debug.Log($"{turnPlayer.charName}�� ����");
        }
        else
        {
            isplayer = false;
        }

        //�� ����
        if (isplayer)
        {
            PlayerButton.SetActive(true);
            //ui �ٲ��� ���� ui, �÷��̾� ui
        }
        else
        {
            PlayerButton.SetActive(false);
            //�̺�Ʈ �߻� -> �̺�Ʈ���� MonsterAttack ����

            MonsterAttack(turnPlayer);
        }
    }
}
