using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnManager : MonoBehaviour
{
    private bool isplayer;
    public GameObject[] players; // 현재 플레이어 수
    public GameObject[] enemies; // 현재 몬스터 수

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
        //우선순위큐(); 호출 -> 순위 정렬

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

        Debug.Log($"{randomCount+1} 번 플레이어 공격받음");
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
            Debug.Log($"{playerCount+1}번 플레이어 깜빡임");
            count++;
        }
        players[playerCount].GetComponent<Renderer>().enabled = true;
    }

    public void onClickNormal_Attack()
    {
        Debug.Log("플레이어 일반 공격 버튼 이벤트");
        Turn();
    }

    public void onClickSkill_Attack()
    {
        Debug.Log($"플레이어 스킬 버튼 이벤트");
        Turn();
    }

    void Turn()
    {
        //조건을 만듬
        // 우선순위큐.디큐 

        if (turn.Count > 0)
        {
            turnPlayer = turn.Dequeue();
            Debug.Log($"{turnPlayer} turn.Dequeue() 값");
        }
        else
        {
            Debug.Log("turn에 저장된 데이터가 없음");
            return;
        }

        //dequeue 데이터 가져옴
        for (int i = 0; i < playerCheck.Count; i++)
        {
            if (turnPlayer == playerCheck[i])
            {
                isplayer = true;
                Debug.Log($"isplayer는 {isplayer}");
                break;
            }
            else
            {
                isplayer = false;
            }
        }

        //턴 진행
        if (isplayer)
        {
            PlayerButton.SetActive(true);
            //ui 바꿔줌 몬스터 ui, 플레이어 ui
            int turnPlayer = 0;
            for (int i = 0; i < playerCheck.Count; i++)
            {
                if(playerCheck[i] == this.turnPlayer)
                {
                    turnPlayer = i + 1;
                    break;
                }
            }

            Debug.Log($"{turnPlayer} 플레이어 차례 버튼 setactive true");
        }
        else
        {
            PlayerButton.SetActive(false);
            Debug.Log($"몬스터 차례 버튼 setactive false");
            MonsterAttack();
        }
    }

    

}
