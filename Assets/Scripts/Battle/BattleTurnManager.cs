using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnManager : MonoBehaviour
{
    private bool isplayer;
    private int MaxRandom;
    public GameObject[] players; // ���� �÷��̾� ��
    public GameObject[] enemies; // ���� ���� ��
    DataManager.Character turnPlayer;
    DataManager.Character turnPlayer2;

    public GameObject PlayerButton;
    public GameObject basicTarget;

    DataManager.Character[] playersData;
    DataManager.Character[] EnemysData;

    PriorityQueue<DataManager.Character> queue = new();

    Dictionary<GameObject, DataManager.Character> MappingChar = new Dictionary<GameObject, DataManager.Character>();

    void Start()
    {
        Player player = GetComponent<Player>();
        playersData = new DataManager.Character[players.Length];
        EnemysData = new DataManager.Character[enemies.Length];

        int Num1 = 1001;
        int Num2 = 2001;
        MaxRandom = 100;
        for (int i = 0; i < players.Length; i++)
        {
            playersData[i] = DataManager.Instance.GetPlayerData(Num1);
            EnemysData[i] = DataManager.Instance.GetEnemyData(Num2);
            Num1++;
            Num2++;
        }


        for (int i = 0; i < players.Length; i++)
        {
            MappingChar.Add(players[i], playersData[i]);
            MappingChar.Add(enemies[i], EnemysData[i]);
            queue.Enqueue(playersData[i]);
            queue.Enqueue(EnemysData[i]);
        }

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

    void MonsterAttack(DataManager.Character turnMonster)
    {

        int playerCount = players.Length;
        int randomCount = Random.Range(0, playerCount);

        MappingChar.TryGetValue(players[randomCount], out DataManager.Character targetPlayer);

        Debug.Log($"{turnMonster.charName}�� {targetPlayer.charName}��(��) ������");
        StartCoroutine(HitDamage(randomCount));
        turnMonster.speed -= 100;
        queue.Enqueue(turnMonster);
        Turn();
    }
    void CompareSpeed()
    {
        DataManager.Character turnPlayer1 = queue.Dequeue();
        DataManager.Character turnPlayer2 = queue.Dequeue();

        int compSpeed = turnPlayer1.speed - turnPlayer2.speed;
        
        int randomResult = Random.Range(0, MaxRandom);
        int compareNum = 50 - (compSpeed * 5);
        if (randomResult > compareNum)
        {
            turnPlayer = turnPlayer1;
            Debug.Log($"�� �÷��̾� : {turnPlayer1.charName}, {turnPlayer2.charName}, ������ : {randomResult}, �ӵ����� :{turnPlayer1.speed} - {turnPlayer2.speed} = {compSpeed}, �и� �� �÷��̾� : {turnPlayer2.charName}, ���� ���� �÷��̾� {turnPlayer1.charName}");
            
            queue.Enqueue(turnPlayer2);
        }
        else
        {
            Debug.Log($"�� �÷��̾� : {turnPlayer1.charName}, {turnPlayer2.charName}, ������ : {randomResult}, �ӵ����� :{turnPlayer1.speed} - {turnPlayer2.speed} = {compSpeed}, �и� �� �÷��̾� : {turnPlayer1.charName}, ���� ���� �÷��̾� {turnPlayer2.charName}");
            turnPlayer = turnPlayer2;
            queue.Enqueue(turnPlayer1);
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
        MappingChar.TryGetValue(basicTarget, out DataManager.Character targetmonster);
            Debug.Log($"{turnPlayer.charName}�� {targetmonster.charName}�� ������� �Ϲ� ����");

            //player.NormalAttack();


            turnPlayer.speed -= 100;
            queue.Enqueue(turnPlayer);
            //basicTarget�� ������ �ٸ� Ÿ�� ��� �����ؾ���

            Turn();
        //else
        //{
        //    Debug.Log("�̹� ����� ĳ�����Դϴ�.");
        //}
        

    }



    public void OnClickSkillAttack()
    {
        if(MappingChar.TryGetValue(basicTarget, out DataManager.Character targetmonster))
        {
            Debug.Log($"{turnPlayer.charName}�� {targetmonster.charName}�� ������� ��ų ����");

            turnPlayer.speed -= 100;
            queue.Enqueue(turnPlayer);
            //basicTarget�� ������ �ٸ� Ÿ�� ��� �����ؾ���
            Turn();
        }
        else
        {
            Debug.Log("�̹� ����� ĳ�����Դϴ�.");
        }

    }

    void Turn()
    {
        if (queue.Count() > 0)
        {
            CompareSpeed();
            //turnPlayer = queue.Dequeue();
        }
        else
        {
            Debug.Log("turn�� ����� �����Ͱ� ����");
            return;
        }
        if (turnPlayer is DataManager.Player)
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
            Debug.Log($"���� ���� ��ư setactive false");
            //�̺�Ʈ �߻� -> �̺�Ʈ���� MonsterAttack ����
            
            MonsterAttack(turnPlayer);
        }
    }
}
