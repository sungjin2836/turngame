using System;
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

    public static bool isSceneMove;

    
    void Start()
    {
        player = GetComponentInParent<Player>();
        fieldCharDataManager = FindObjectOfType<FieldCharDataManager>();
    }

    private void Update()
    {
        
    }

    


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            isSceneMove = true;
            enemy = other.GetComponent<Enemy>();
            enemyIDData = other.GetComponent<CharacterData>();
            fieldCharDataManager.GetEnemyID(enemyIDData.CharacterID);
            if (enemy.ContainsElement(player.element)) 
            {
                fieldCharDataManager.isWeakElement = true;
                FieldUIManager.Instance.DisplayWeakness(player.element);
            }

            StartCoroutine(LoadSceneWaitForSeconds(1.0f, "TurnTestScene"));
        }
        if (other.CompareTag("Object"))
        {
            ObjectManager objectManager = other.gameObject.GetComponentInParent<ObjectManager>();
            other.GetComponent<BoxCollider>().enabled = false;
            other.GetComponent<MeshRenderer>().enabled = false;
            objectManager.checkObject();
            Debug.Log("아이템 획득");
        }
    }

    private IEnumerator LoadSceneWaitForSeconds(float duration, int sceneIndex)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(sceneIndex);
    }
    
    private IEnumerator LoadSceneWaitForSeconds(float duration, string sceneIndex)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(sceneIndex);
    }

    private void OnDestroy()
    {
        isSceneMove = false;
    }
}
