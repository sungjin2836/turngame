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
        //페이드인페이드아웃
        SceneManager.LoadScene("SampleScene");
    }


}
