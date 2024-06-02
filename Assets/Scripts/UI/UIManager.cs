using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Slider enemyShieldBar;
    [SerializeField]
    private Slider enemyHpBar;



    public int _hp;
    public int _shield;
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
        _hp = 1000;
        _shield = 60;
        SetMaxHealth(_hp);
        SetMaxShield(_shield);

        Debug.Log(_hp);
        Debug.Log(_shield);
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
