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
    //        characters[i].�����ൿ������ += (10000 / player1.�ƽ��ൿ������) - player1.�����ൿ������; //�÷��̾�2�� �ൿ������ �ѷ� - ���緮��ŭ ������
    //    }
    //    player1.�����ൿ������ = 0;
    //    queue.Enqueue(player1);
    //    player2 = queue.Dequeue();
        
    //    for (int i = 0; i < characters.Length; i++)
    //    {
    //        characters[i].�����ൿ������ += (10000 / player2.�ƽ��ൿ������) - player2.�����ൿ������; //�÷��̾�2�� �ൿ������ �ѷ� - ���緮��ŭ ������
    //    }
    //    player2.�����ൿ������ = 0;
    //    queue.Enqueue(player2);
    //}

    //void Turn()
    //{
    //    Deq();

    //    if(player1.type == 'player' && player2.type == 'player')
    //    {
    //        ����
    //    }
    //    else if ()
    //    {

    //    }
        

    //}





}
