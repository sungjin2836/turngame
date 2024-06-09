using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Rendering.FilterWindow;

public class GoInTurnGame : MonoBehaviour
{
    private Player player;
    private Enemy enemy;
    private FieldCharDataManager fieldCharDataManager;
    
    void Start()
    {
        player = GetComponentInParent<Player>();
        fieldCharDataManager = FindObjectOfType<FieldCharDataManager>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemy = other.GetComponent<Enemy>();
            if (enemy.ContainsElement(player.element)) fieldCharDataManager.isWeakElement = true;
                SceneManager.LoadScene("TurnTestScene");
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
