using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoInTurnGame : MonoBehaviour
{
    
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //���̵������̵�ƿ�
        SceneManager.LoadScene("SampleScene");
    }


}
