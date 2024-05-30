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

    public GameObject basicTarget;

    public int target;

    DataManager.Character[] playersData;
    DataManager.Character[] EnemysData;

    PriorityQueue<DataManager.Character> queue = new();
    PriorityQueue<DataManager.Character> queue2 = new();

    Dictionary<GameObject, DataManager.Character> MappingChar = new Dictionary<GameObject, DataManager.Character>();

    private Queue<int> turn;
    void Start()
    {
        playersData = new DataManager.Character[players.Length];
        EnemysData = new DataManager.Character[enemies.Length];
        var playerData1 = DataManager.Instance.GetPlayerData(1001);
        var playerData2 = DataManager.Instance.GetPlayerData(1002);
        var playerData3 = DataManager.Instance.GetPlayerData(1003);
        var playerData4 = DataManager.Instance.GetPlayerData(1004);
        var enemyData1 = DataManager.Instance.GetEnemyData(2001);
        var enemyData2 = DataManager.Instance.GetEnemyData(2002);
        var enemyData3 = DataManager.Instance.GetEnemyData(2003);
        var enemyData4 = DataManager.Instance.GetEnemyData(2004);

        int Num1 = 1001;
        int Num2 = 2001;
        for (int i = 0; i < 4; i++)
        {
            playersData[i] = DataManager.Instance.GetPlayerData(Num1);
            EnemysData[i] = DataManager.Instance.GetEnemyData(Num2);
            Num1++;
            Num2++;
        }

        for (int i = 0; i < 4; i++)
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

    void MonsterAttack(DataManager.Character turnMonster)
    {
        int playerCount = players.Length;
        int randomCount = Random.Range(0, playerCount);

        MappingChar.TryGetValue(players[randomCount], out DataManager.Character targetPlayer);

        Debug.Log($"{turnMonster.charName}가 {targetPlayer.charName}을(를) 공격함");
        StartCoroutine(HitDamage(randomCount));
        turnMonster.speed -= 10;
        queue.Enqueue(turnMonster);
        Turn();
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

        Debug.Log($"{turnPlayer.charName}이 {targetmonster.charName}을 대상으로 일반 공격");
        turnPlayer.speed -= 10;
        queue.Enqueue(turnPlayer);
        //basicTarget이 죽으면 다른 타겟 대상 설정해야함

        Turn();
    }



    public void OnClickSkillAttack()
    {
        MappingChar.TryGetValue(basicTarget, out DataManager.Character targetmonster);
        Debug.Log($"{turnPlayer.charName}이 {targetmonster.charName}을 대상으로 스킬 공격");

        turnPlayer.speed -= 10;
        queue.Enqueue(turnPlayer);
        //basicTarget이 죽으면 다른 타겟 대상 설정해야함
        Turn();
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
