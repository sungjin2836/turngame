using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldToTurnDataManager : MonoBehaviour
{
    private FieldCharDataManager fieldCharDataManager;

    [SerializeField]
    private GameObject players;
    [SerializeField]
    private GameObject enemy;
    private CharacterData[] playerDataIDs;
    private CharacterData enemyDataID;

    private void Awake()
    {
        fieldCharDataManager = FindObjectOfType<FieldCharDataManager>();

        Debug.Log($" 약점 공격을 했는지?{fieldCharDataManager.isWeakElement}");
        fieldCharDataManager.DebugIdTest();

        playerDataIDs = players.GetComponentsInChildren<CharacterData>();
        enemyDataID = enemy.GetComponent<CharacterData>();

        for ( int i = 0; i < playerDataIDs.Length; i++)
        {
            playerDataIDs[i].CharacterID = fieldCharDataManager.GetCharacterID(i);
        }
        enemyDataID.CharacterID = fieldCharDataManager.SetEnemyID();
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
