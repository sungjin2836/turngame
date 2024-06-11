using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(150)]
public class FieldUIController : MonoBehaviour
{
    public Player player;
    public Text text;
    public Text enemyName;
    public Text enemyElements;
    private string elementsData;
    // Start is called before the first frame update
    void Start()
    {
        if (player != null)
        {
            text = GetComponent<Text>();
            text.text = player.charName;
        }
    }

    public void DetectEnemy(Enemy hit)
    {
        Debug.Log("DetectEnemy");
        Enemy enemy = hit.GetComponent<Enemy>();
        Debug.Log(enemy.charName);
        if (hit != null && enemyName != null)
        {
            if(enemyName != null)
            {
                enemyName = enemyName.GetComponent<Text>();
                Debug.Log(enemyName);
            }
            if (enemyElements != null)
            {
                enemyElements = enemyElements.GetComponent<Text>();
                Debug.Log(enemyElements);
            }
            enemyName.text = enemy.charName;
            elementsData = "";
            for (int i = 0; i < enemy.weakElements.Length; i++)
            {
                elementsData += enemy.weakElements[i];
                elementsData += " ";
                Debug.Log(enemy.weakElements[i]);
            }
            Debug.Log(elementsData);
            enemyElements.text = elementsData;
        }
    }
}
