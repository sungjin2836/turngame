using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldToTurnDataManager : MonoBehaviour
{
    private FieldCharDataManager fieldCharDataManager;

    [SerializeField]
    GameObject players;
    [SerializeField]
    GameObject enemy;
    CharacterData[] playerDataIDs;
    CharacterData enemyDataID;

    private void Awake()
    {
        fieldCharDataManager = FindObjectOfType<FieldCharDataManager>();
        if(fieldCharDataManager != null)
        {
            Debug.Log($" 약점 공격을 했는지?{fieldCharDataManager.isWeakElement}");
            fieldCharDataManager.DebugIdTest();
            playerDataIDs = players.GetComponentsInChildren<CharacterData>();
            enemyDataID = enemy.GetComponent<CharacterData>();

            for (int i = 0; i < playerDataIDs.Length; i++)
            {
                playerDataIDs[i].CharacterID = fieldCharDataManager.GetCharacterID(i);
            }
            enemyDataID.CharacterID = fieldCharDataManager.SetEnemyID();
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
