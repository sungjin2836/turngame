using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DummyCharacter
{
    public enum CharacterType
    {
        Player,
        Enemy
    }
    public int speed = 0;
    public CharacterType type = CharacterType.Player;

    public DummyCharacter(CharacterType charType, int speed)
    {
        type = charType;
        this.speed = speed;
    }
}

public class BattleGameManager : MonoBehaviour
{
    DummyCharacter[] players = new DummyCharacter[4] { 
        new DummyCharacter(DummyCharacter.CharacterType.Player, 10),
        new DummyCharacter(DummyCharacter.CharacterType.Player, 20),
        new DummyCharacter(DummyCharacter.CharacterType.Player, 30),
        new DummyCharacter(DummyCharacter.CharacterType.Player, 40)
    };
    DummyCharacter[] enemies = new DummyCharacter[4] {
        new DummyCharacter(DummyCharacter.CharacterType.Enemy, 10),
        new DummyCharacter(DummyCharacter.CharacterType.Enemy, 20),
        new DummyCharacter(DummyCharacter.CharacterType.Enemy, 30),
        new DummyCharacter(DummyCharacter.CharacterType.Enemy, 40)
    };
    bool isPlayerFirst = true;
    Queue<DummyCharacter> turnQueue = new();

    void Start()
    {
        // 턴 우선순위 정렬
        if (isPlayerFirst)
        {
            foreach (DummyCharacter player in players)
            {
                turnQueue.Enqueue(player);
            }

            foreach (DummyCharacter enemy in enemies)
            {
                turnQueue.Enqueue(enemy);
            }
        }
        else
        {
            foreach (DummyCharacter enemy in enemies)
            {
                turnQueue.Enqueue(enemy);
            }

            foreach (DummyCharacter player in players)
            {
                turnQueue.Enqueue(player);
            }
        }

        // 턴 자동 실행
        StartCoroutine(StartTurn());
    }

    // 2초마다 턴 자동실행 처리
    IEnumerator StartTurn()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            Debug.Log($"{turnQueue.Peek().type}, {turnQueue.Peek().speed} 해당 캐릭터 타입의 턴 차례");
            turnQueue.Enqueue(turnQueue.Dequeue());
        }
    }
}
