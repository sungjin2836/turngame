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
        if (other.CompareTag("Enemy"))
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (other.CompareTag("Object"))
        {
            ObjectManager objectManager = other.gameObject.GetComponentInParent<ObjectManager>();
            other.gameObject.SetActive(false);
            objectManager.checkObject();
            Debug.Log("æ∆¿Ã≈€ »πµÊ");
        }

        //∆‰¿ÃµÂ¿Œ∆‰¿ÃµÂæ∆øÙ
        
    }


}
