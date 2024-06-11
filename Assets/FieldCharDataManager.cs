using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[DefaultExecutionOrder(200)]
public class FieldCharDataManager : MonoBehaviour
{
    public static FieldCharDataManager instance;
    private CharacterData[] playerDataIDs;
    private int enemyDataID;
    [SerializeField]
    private GameObject players;
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
            Debug.Log($"���̵�� {playerDataIDs[i].CharacterID}");
        }
        Debug.Log($"���ݴ��� ������ ���̵�� {enemyDataID}");
    }

    void Update()
    {
        
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
