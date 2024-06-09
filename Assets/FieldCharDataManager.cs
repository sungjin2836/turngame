using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[DefaultExecutionOrder(200)]
public class FieldCharDataManager : MonoBehaviour
{

    private GameObject[] characterDatas;
    private CharacterData[] playerDataIDs;
    [SerializeField]
    private GameObject players;
    private Enemy enemy;
    public bool isWeakElement;

    void Start()
    {
        playerDataIDs = players.GetComponentsInChildren<CharacterData>();
        //playerDataIDs = new CharacterData[players.transform.childCount];
    }

    public void DebugIdTest()
    {
        for (int i = 0; i < playerDataIDs.Length; i++)
        {
            Debug.Log($"아이디는 {playerDataIDs[i].CharacterID}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
