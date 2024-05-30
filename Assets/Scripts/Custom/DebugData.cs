using UnityEngine;

public class DebugData : MonoBehaviour
{
    public int characterID = 1002;
    public Character target;

    private void Start()
    {
        if (characterID < 2000)
        {
            DataManager.Player playerData = DataManager.Instance.GetPlayerData(characterID);
            Player player = GetComponent<Player>();
            player.charName = playerData.charName;
            player.level = playerData.level;
            player.maxHP = playerData.hp;
            player.speed = playerData.speed;
            player.attackStat = playerData.attackStat;
            player.element = playerData.elem;
            player.attackPower = 0.5f;
            player.HealSelf(10000);
            Debug.Log(JsonUtility.ToJson(player));
        }
        else
        {
            DataManager.Enemy enemyData = DataManager.Instance.GetEnemyData(characterID);
            Enemy enemy = GetComponent<Enemy>();
            enemy.charName = enemyData.charName;
            enemy.level = enemyData.level;
            enemy.maxHP = enemyData.hp;
            enemy.speed = enemyData.speed;
            enemy.attackStat = enemyData.attackStat;
            enemy.weakElements = enemyData.elem;
            enemy.maxShield = enemyData.shield;
            enemy.HealSelf(10000);
            enemy.RegenShield();
            Debug.Log(JsonUtility.ToJson(enemy));
        }
    }

    public void Attack()
    {
        if (GetComponent<Player>())
        {
            GetComponent<Player>().NormalAttack(target);
        }
        else if (GetComponent<Enemy>())
        {
            GetComponent<Enemy>().NormalAttack(target);
        }
    }
}