using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static UnityEditor.Rendering.FilterWindow;

public class GoInTurnGame : MonoBehaviour
{
    private Player player;
    private Enemy enemy;
    private CharacterData enemyIDData;
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
            enemyIDData = other.GetComponent<CharacterData>();
            fieldCharDataManager.GetEnemyID(enemyIDData.CharacterID);
            if (enemy.ContainsElement(player.element)) fieldCharDataManager.isWeakElement = true;
                SceneManager.LoadScene("TurnTestScene");
        }
        if (other.CompareTag("Object"))
        {
            ObjectManager objectManager = other.gameObject.GetComponentInParent<ObjectManager>();
            other.gameObject.SetActive(false);
            objectManager.checkObject();
            Debug.Log("아이템 획득");
        }

        //페이드인페이드아웃
        
    }


}
