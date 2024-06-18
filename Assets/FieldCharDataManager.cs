using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[DefaultExecutionOrder(200)]
public class FieldCharDataManager : MonoBehaviour
{
    public static FieldCharDataManager instance = null;
    CharacterData[] playerDataIDs;
    int enemyDataID;
    [SerializeField]
    GameObject players;
    public bool isWeakElement;


    
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        playerDataIDs = players.GetComponentsInChildren<CharacterData>();
    }

    public void DebugIdTest()
    {
        for (int i = 0; i < playerDataIDs.Length; i++)
        {
            Debug.Log($"아이디는 {playerDataIDs[i].CharacterID}");
        }
        Debug.Log($"공격당한 몬스터의 아이디는 {enemyDataID}");
    }

    public int GetCharacterID(int num)
    {
        return playerDataIDs[num].CharacterID;
    }

    public void GetEnemyID(int num)
    {
        enemyDataID = num;
    }

    public int SetEnemyID()
    {
        return enemyDataID;
    }

}
