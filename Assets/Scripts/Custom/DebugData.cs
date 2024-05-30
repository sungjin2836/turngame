using System;
using UnityEngine;

public class DebugData : MonoBehaviour
{
    public int playerID = 1002;
    public int enemyID = 2002;

    private void Start()
    {
        var playerData = DataManager.Instance.GetPlayerData(playerID);
        Debug.Log(JsonUtility.ToJson(playerData));
        var enemyData = DataManager.Instance.GetEnemyData(enemyID);
        Debug.Log(JsonUtility.ToJson(enemyData));
    }
}