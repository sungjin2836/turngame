using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DataManager;

public class BattleTurnManager : MonoBehaviour
{
    private bool isplayer;
    private int MaxRandom;
    public GameObject[] players; // ���� �÷��̾� ��
    public GameObject[] enemies; // ���� ���� ��
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
    // ť -> ����Ʈ 

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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //���콺 ��ġ
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

        Debug.Log($"{turnMonster.charName}�� {players[randomCount].name}��(��) ������");
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
            //Debug.Log($"�� �÷��̾� : {turnPlayer1.charName}, {turnPlayer2.charName}, ������ : {randomResult}, �ӵ����� :{turnPlayer1.speed} - {turnPlayer2.speed} = {compSpeed}, �и� �� �÷��̾� : {turnPlayer2.charName}, ���� ���� �÷��̾� {turnPlayer1.charName}");

            queue.Enqueue(turnPlayer2);
        }
        else
        {
            //Debug.Log($"�� �÷��̾� : {turnPlayer1.charName}, {turnPlayer2.charName}, ������ : {randomResult}, �ӵ����� :{turnPlayer1.speed} - {turnPlayer2.speed} = {compSpeed}, �и� �� �÷��̾� : {turnPlayer1.charName}, ���� ���� �÷��̾� {turnPlayer2.charName}");
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

        Debug.Log($"ü�¹� �׽�Ʈ {enemy.name} ü�� : {enemy.hp} �ǵ� : {enemy.shield}");
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
                Debug.Log("��� ���Ͱ� �׾����ϴ�.");
                return;
            }

        }
        //basicTarget�� ������ �ٸ� Ÿ�� ��� �����ؾ���
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

            Debug.Log($"�������� {turnPlayer.charName}");
        }

        turnPlayer.speed -= 100;
        queue.Enqueue(turnPlayer);
        //basicTarget�� ������ �ٸ� Ÿ�� ��� �����ؾ���
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
