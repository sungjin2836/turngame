using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnManager : MonoBehaviour
{
    private bool isplayer;
    public GameObject[] players; // ���� �÷��̾� ��
    public GameObject[] enemies; // ���� ���� ��
    DataManager.Character turnPlayer;
    
    public GameObject PlayerButton;

    PriorityQueue<DataManager.Character> queue = new();

    private Queue<int> turn;
    void Start()
    {
        var playerData1 = DataManager.Instance.GetPlayerData(1001);
        var playerData2 = DataManager.Instance.GetPlayerData(1002);
        var playerData3 = DataManager.Instance.GetPlayerData(1003);
        var playerData4 = DataManager.Instance.GetPlayerData(1004);

        var enemyData1 = DataManager.Instance.GetEnemyData(2001);
        var enemyData2 = DataManager.Instance.GetEnemyData(2002);
        var enemyData3 = DataManager.Instance.GetEnemyData(2003);
        var enemyData4 = DataManager.Instance.GetEnemyData(2004);


        queue.Enqueue(playerData1);
        queue.Enqueue(playerData2);
        queue.Enqueue(playerData3);
        queue.Enqueue(playerData4);
        queue.Enqueue(enemyData1);
        queue.Enqueue(enemyData2);
        queue.Enqueue(enemyData3);
        queue.Enqueue(enemyData4);


        turn = new Queue<int>();
        isplayer = false;
        //�켱����ť(); ȣ�� -> ���� ����

        PlayerButton.SetActive(false);

        Turn();
        
    }


    void Update()
    {

    }

    void MonsterAttack(DataManager.Character turnMonster)
    {
        int playerCount = players.Length;

        int randomCount = Random.Range(0, playerCount);

        Debug.Log($"{turnMonster.charName}�� {randomCount+1} �� �÷��̾� ������");
        StartCoroutine(HitDamage(randomCount));
        turnMonster.speed -= 10;
        queue.Enqueue(turnMonster);
        Turn();
    }

    IEnumerator HitDamage(int playerCount)
    {
        int count = 0;
        while(count < 5)
        {
            yield return new WaitForSeconds(1f);
            players[playerCount].GetComponent<Renderer>().enabled = !players[playerCount].GetComponent<Renderer>().enabled;
            Debug.Log($"{playerCount+1}�� �÷��̾� ������");
            count++;
        }
        players[playerCount].GetComponent<Renderer>().enabled = true;
    }

    public void onClickNormal_Attack()
    {
        Debug.Log($"{turnPlayer.charName}�� �Ϲ� ���� ��ư �̺�Ʈ");
        turnPlayer.speed -= 10;
        queue.Enqueue(turnPlayer);
        Turn();
    }

    public void onClickSkill_Attack()
    {
        Debug.Log($"{turnPlayer.charName}�� ��ų ��ư �̺�Ʈ");
        turnPlayer.speed -= 10;
        queue.Enqueue(turnPlayer);
        Turn();
    }

    void PlayerTurn()
    {
        if (true)
        {
            onClickSkill_Attack();
            onClickNormal_Attack();

        }
    }

    private void OnMouseDown()
    {
        if()
        {
            Debug.Log
        }
    }


    void Turn()
    {
        if (queue.Count() > 0)
        {
            turnPlayer = queue.Dequeue();
        }
        else
        {
            Debug.Log("turn�� ����� �����Ͱ� ����");
            return;
        }

        //�÷��̾� üũ ������ ������ Ÿ���� �÷��̾������� Ȯ��
        
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
            MonsterAttack(turnPlayer);
        }
    }

    

}
