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
        

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Renderer>();
        }
        turn = new Queue<int>();
        isplayer = false;
        //�켱����ť(); ȣ�� -> ���� ����

        PlayerButton.SetActive(false);

        playerCheck.Add(100);
        playerCheck.Add(90);
        playerCheck.Add(80);
        playerCheck.Add(60);


        turn.Enqueue(100);
        turn.Enqueue(95);
        turn.Enqueue(90);
        turn.Enqueue(80);
        turn.Enqueue(87);
        turn.Enqueue(83);
        turn.Enqueue(70);
        turn.Enqueue(60);

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
