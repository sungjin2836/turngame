using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQueueManager : MonoBehaviour
{
    PriorityQueue<Character> queue = new();

    private Character[] characters;
    private Character player1;
    private Character player2;
    
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    //void Deq()
    //{
    //    player1 = queue.Dequeue();
    //    for (int i = 0; i < characters.Length; i++)
    //    {
    //        characters[i].현재행동게이지 += (10000 / player1.맥스행동게이지) - player1.현재행동게이지; //플레이어2의 행동게이지 총량 - 현재량만큼 더해줌
    //    }
    //    player1.현재행동게이지 = 0;
    //    queue.Enqueue(player1);
    //    player2 = queue.Dequeue();
        
    //    for (int i = 0; i < characters.Length; i++)
    //    {
    //        characters[i].현재행동게이지 += (10000 / player2.맥스행동게이지) - player2.현재행동게이지; //플레이어2의 행동게이지 총량 - 현재량만큼 더해줌
    //    }
    //    player2.현재행동게이지 = 0;
    //    queue.Enqueue(player2);
    //}

    //void Turn()
    //{
    //    Deq();

    //    if(player1.type == 'player' && player2.type == 'player')
    //    {
    //        협공
    //    }
    //    else if ()
    //    {

    //    }
        

    //}





}
