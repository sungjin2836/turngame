using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Game Manager Example
public class BattleGameManager : MonoBehaviour
{
    class PriorityQueue<T>
    {
        private List<T> queueList = new();

        public void Enqueue(T element)
        {
            queueList.Add(element);
            queueList.Sort();
        }

        public T Dequeue()
        {
            if (queueList.Count == 0)
            {
                throw new System.ArgumentOutOfRangeException("You cannot Dequeue at empty Queue. Please check Queue Count.");
            }

            T returnValue = queueList[0];
            queueList.RemoveAt(0);
            return returnValue;
        }

        public int Count()
        {
            return queueList.Count;
        }

        public T Peek()
        {
            if (queueList.Count == 0)
            {
                throw new System.ArgumentOutOfRangeException("You cannot Peek at empty Queue. Please check Queue count.");
            }

            return queueList[0];
        }

        public List<T> InspectList()
        {
            return queueList;
        }
    }

    // �׽�Ʈ�� �� GameManager //
    // �����ϰ� ǥ���� ���� ĳ���� Ŭ����
    class DummyCharacter : IComparable<DummyCharacter>
    {
        public enum CharacterType
        {
            Player,
            Enemy
        }
        public int speed = 0;
        public CharacterType type = CharacterType.Player;
        public string name = "";

        public DummyCharacter(CharacterType charType, int speed, string name)
        {
            type = charType;
            this.speed = speed;
            this.name = name;
        }

        // �켱���� ť �񱳸� ���� ���� ������ Ŭ������ �߰��ؾߵ� (����Ʈ�� ����)
        public int CompareTo(DummyCharacter other)
        {
            return other.speed - speed;
        }
    }

    DummyCharacter[] players = new DummyCharacter[4] { 
        new DummyCharacter(DummyCharacter.CharacterType.Player, 30, "�׽�Ʈ3"),
        new DummyCharacter(DummyCharacter.CharacterType.Player, 20, "�׽�Ʈ2"),
        new DummyCharacter(DummyCharacter.CharacterType.Player, 10, "�׽�Ʈ1"),
        new DummyCharacter(DummyCharacter.CharacterType.Player, 40, "�׽�Ʈ4")
    };
    DummyCharacter[] enemies = new DummyCharacter[4] {
        new DummyCharacter(DummyCharacter.CharacterType.Enemy, 20, "�� �׽�Ʈ2"),
        new DummyCharacter(DummyCharacter.CharacterType.Enemy, 40, "�� �׽�Ʈ4"),
        new DummyCharacter(DummyCharacter.CharacterType.Enemy, 30, "�� �׽�Ʈ3"),
        new DummyCharacter(DummyCharacter.CharacterType.Enemy, 10, "�� �׽�Ʈ1"),
    };
    bool isPlayerFirst = true;
    PriorityQueue<DummyCharacter> turnQueue = new();

    void Start()
    {
        // �켱���� ť�� �÷��̾�� �� Ŭ���� �߰�
        foreach (DummyCharacter player in players)
        {
            turnQueue.Enqueue(player);
        }

        foreach (DummyCharacter enemy in enemies)
        {
            turnQueue.Enqueue(enemy);
        }

        Debug.Log("==== Initialized Data ====");
        foreach (var charObj in turnQueue.InspectList())
        {
            Debug.Log($"{charObj.type} {charObj.name} = {charObj.speed}");
        }
        Debug.Log("==== End of Initialized Data ====");

        // ������ �� �ڵ� ����
        StartCoroutine(StartTurn());
    }

    // 2�ʸ��� �� �ڵ����� ó��
    IEnumerator StartTurn()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            if (turnQueue.Peek().type == DummyCharacter.CharacterType.Player)
            {
                int targetPosition = new System.Random().Next(0, enemies.Count() - 1);
                turnQueue.Peek().speed -= 5;
                Debug.Log($"{turnQueue.Peek().name} -> {enemies[targetPosition].name}");
                turnQueue.Enqueue(turnQueue.Dequeue());
            }
            else
            {
                int targetPosition = new System.Random().Next(0, players.Count() - 1);
                turnQueue.Peek().speed -= 5;
                Debug.Log($"{turnQueue.Peek().name} -> {players[targetPosition].name}");
                turnQueue.Enqueue(turnQueue.Dequeue());
            }

            Debug.Log("==== Turn Status ====");
            foreach (var charObj in turnQueue.InspectList())
            {
                Debug.Log($"{charObj.type} {charObj.name} = {charObj.speed}");
            }
            Debug.Log("==== End of Turn Status ====");
        }
    }
}
