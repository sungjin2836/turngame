using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    
    private const string DATA_PATH = "Data";
    private const string PLAYER_JSON = "Player";
    private const string ENEMY_JSON = "Enemy";

    private Dictionary<int, Player> _players = new Dictionary<int, Player>();
    private Dictionary<int, Enemy> _enemies = new Dictionary<int, Enemy>();
    
    [System.Serializable]
    public abstract class Character
    {
        public int id;
        public string charName;
        public int level;
        public int hp;
        public int speed;
        public int attackStat;
    }
    
    [System.Serializable]
    public class Player : Character
    {
        public ElementType elem;

        public Player()
        {
        }
    }
    
    [System.Serializable]
    public class Enemy : Character
    {
        public int shield;
        public ElementType[] elem;
    }

    private struct PlayerData
    {
        public Player[] player;
    }

    private struct EnemyData
    {
        public Enemy[] enemy;
    }

    private void InitPlayerData()
    {
        TextAsset playerJson = Resources.Load<TextAsset>(Path.Combine(DATA_PATH, PLAYER_JSON));
        PlayerData playerList = JsonUtility.FromJson<PlayerData>(playerJson.text);

        foreach (var data in playerList.player)
        {
            var player = data as Character;
            _players.Add(data.id, data);
        }
    }

    private void InitEnemyData()
    {
        TextAsset enemyJson = Resources.Load<TextAsset>(Path.Combine(DATA_PATH, ENEMY_JSON));
        EnemyData enemyList = JsonUtility.FromJson<EnemyData>(enemyJson.text);

        foreach (var data in enemyList.enemy)
        {
            var enemy = data as Character;
            _enemies.Add(enemy.id, data);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        InitPlayerData();
        InitEnemyData();
    }

    public Player GetPlayerData(int id)
    {
        return _players[id];
    }

    public Enemy GetEnemyData(int id)
    {
        return _enemies[id];
    }
}