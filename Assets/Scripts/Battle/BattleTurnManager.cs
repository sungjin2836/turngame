using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnManager : MonoBehaviour
{
    private bool isplayer;
    public GameObject[] players; // ���� �÷��̾� ��
    public GameObject[] enemies; // ���� ���� ��

    public GameObject PlayerButton;

    List<int> playerCheck = new List<int>();
    int turnPlayer;

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


        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Renderer>();
        }
        turn = new Queue<int>();
        isplayer = false;
        //�켱����ť(); ȣ�� -> ���� ����

        PlayerButton.SetActive(false);

        playerCheck.Add(playerData1.speed);
        playerCheck.Add(playerData2.speed);
        playerCheck.Add(playerData3.speed);
        playerCheck.Add(playerData4.speed);


        turn.Enqueue(playerData1.speed);
        turn.Enqueue(playerData2.speed);
        turn.Enqueue(playerData3.speed);
        turn.Enqueue(playerData4.speed);
        turn.Enqueue(enemyData1.speed);
        turn.Enqueue(enemyData2.speed);
        turn.Enqueue(enemyData3.speed);
        turn.Enqueue(enemyData4.speed);
        Debug.Log((playerData1.speed));
        Debug.Log((playerData2.speed));
        Debug.Log((playerData3.speed));
        Debug.Log((playerData4.speed));
        Debug.Log((enemyData1.speed));
        Debug.Log((enemyData2.speed));
        Debug.Log((enemyData3.speed));
        Debug.Log((enemyData4.speed));

        Turn();
        
    }


    void Update()
    {
    }

    void MonsterAttack()
    {
        int playerCount = players.Length;

        int randomCount = Random.Range(0, playerCount);

        Debug.Log($"{randomCount+1} �� �÷��̾� ���ݹ���");
        StartCoroutine(HitDamage(randomCount));
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
        Debug.Log("�÷��̾� �Ϲ� ���� ��ư �̺�Ʈ");
        Turn();
    }

    public void onClickSkill_Attack()
    {
        Debug.Log($"�÷��̾� ��ų ��ư �̺�Ʈ");
        Turn();
    }

    void Turn()
    {
        //������ ����
        // �켱����ť.��ť 

        if (turn.Count > 0)
        {
            turnPlayer = turn.Dequeue();
            Debug.Log($"{turnPlayer} turn.Dequeue() ��");
        }
        else
        {
            Debug.Log("turn�� ����� �����Ͱ� ����");
            return;
        }

        //dequeue ������ ������
        for (int i = 0; i < playerCheck.Count; i++)
        {
            if (turnPlayer == playerCheck[i])
            {
                isplayer = true;
                Debug.Log($"isplayer�� {isplayer}");
                break;
            }
            else
            {
                isplayer = false;
            }
        }

        //�� ����
        if (isplayer)
        {
            PlayerButton.SetActive(true);
            //ui �ٲ��� ���� ui, �÷��̾� ui
            int turnPlayer = 0;
            for (int i = 0; i < playerCheck.Count; i++)
            {
                if(playerCheck[i] == this.turnPlayer)
                {
                    turnPlayer = i + 1;
                    break;
                }
            }

            Debug.Log($"{turnPlayer} �÷��̾� ���� ��ư setactive true");
        }
        else
        {
            PlayerButton.SetActive(false);
            Debug.Log($"���� ���� ��ư setactive false");
            MonsterAttack();
        }
    }

    

}
