using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject turnOrderPanel;


    [SerializeField]
    private Slider enemyShieldBar;
    [SerializeField]
    private Slider enemyHpBar;
    [SerializeField]
    private Text turnOrderText;

    Text[] turnTexts;
    private int _hp;
    private int _shield;
    public int Hp
    {
        get => _hp;

        private set => _hp = Math.Clamp(value, 0, _hp);
    }

    public int Shield
    {
        get => _shield;

        private set => _shield = Math.Clamp(value, 0, _shield);
    }


    void Start()
    {
        
    }

    public void InitTurnText(int num)
    {
        turnTexts = new Text[num];

        for (int i = 0; i < num; i++)
        {
            turnTexts[i] = Instantiate(turnOrderText, turnOrderPanel.transform.position, Quaternion.identity);
            turnTexts[i].transform.SetParent(turnOrderPanel.transform);
        }
    }

    public void TurnTextPrint(int num, string Name)
    {
        turnTexts[num].text = Name;
    }

    public void SetMaxHealth(int health)
    {
        enemyHpBar.maxValue = health;
        enemyHpBar.value = health;
    }

    public void SetHealth(int health)
    {
        enemyHpBar.value = health;
    }

    public void SetMaxShield(int shield)
    {
        enemyShieldBar.maxValue = shield;
        enemyShieldBar.value = shield;
    }
    public void SetShield(int shield)
    {
        enemyShieldBar.value = shield;
    }

}
