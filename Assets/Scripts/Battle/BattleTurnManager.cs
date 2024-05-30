using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnManager : MonoBehaviour
{
    private bool isplayer;
    public GameObject[] players; // 현재 플레이어 수
    public GameObject[] enemies; // 현재 몬스터 수
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
        //우선순위큐(); 호출 -> 순위 정렬

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

        Debug.Log($"{turnMonster.charName}가 {randomCount+1} 번 플레이어 공격함");
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
            Debug.Log($"{playerCount+1}번 플레이어 깜빡임");
            count++;
        }
        players[playerCount].GetComponent<Renderer>().enabled = true;
    }

    public void onClickNormal_Attack()
    {
        Debug.Log($"{turnPlayer.charName}의 일반 공격 버튼 이벤트");
        turnPlayer.speed -= 10;
        queue.Enqueue(turnPlayer);
        Turn();
    }

    public void onClickSkill_Attack()
    {
        Debug.Log($"{turnPlayer.charName}의 스킬 버튼 이벤트");
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
            Debug.Log("turn에 저장된 데이터가 없음");
            return;
        }

        //플레이어 체크 데이터 들어오면 타입이 플레이어인지만 확인
        
            if (turnPlayer is DataManager.Player)
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
            Debug.Log($"몬스터 차례 버튼 setactive false");
            MonsterAttack(turnPlayer);
        }
    }

    

}
