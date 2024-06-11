using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterList : MonoBehaviour
{
    private int _characterID;
    private static DataManager.Player _currentCharacter;
    private readonly Dictionary<DataManager.Player, int> _expPair = new();
    private static int maxExp => 150 * _currentCharacter.level;

    private static Action _onValueChanged;

    [SerializeField] private Transform content;
    [Space]
    [SerializeField] private Text nameText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text atkText;
    [SerializeField] private Text speedText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private Button levelUpButton;

    private void OnEnable()
    {
        _onValueChanged += UpdateUI;
        levelUpButton.onClick.AddListener(() => AddExp(1000));
    }

    private void OnDisable()
    {
        _onValueChanged -= UpdateUI;
        levelUpButton.onClick.RemoveAllListeners();
    }

    public void AddCharacter(int id)
    {
        CharacterIcon icon = new GameObject($"Icon_{id}").AddComponent<CharacterIcon>();
        icon.transform.SetParent(content);
        
        icon.GetCharacterData(id);
        _expPair.Add(icon.Data, 0);
        
        if (_expPair.Count == 1) SelectCharacter(icon);
    }

    public static void SelectCharacter(CharacterIcon icon)
    {
        _currentCharacter = icon.Data;
        _onValueChanged?.Invoke();
    }

    private void AddExp(int value)
    {
        int newExp = _expPair[_currentCharacter] + value;

        while (newExp >= maxExp)
        {
            newExp -= maxExp;
            LevelUp();
        }

        _expPair[_currentCharacter] = newExp;

        _onValueChanged?.Invoke();
    }

    private static void LevelUp()
    {
        _currentCharacter.level++;
        _currentCharacter.hp += 6;
        _currentCharacter.attackStat += 4;
    }

    private void UpdateUI()
    {
        nameText.text = _currentCharacter.charName;
        levelText.text = $"Lv.{_currentCharacter.level}/20";
        expSlider.maxValue = maxExp;
        expSlider.value = _expPair[_currentCharacter];
        hpText.text = $"{_currentCharacter.hp}";
        atkText.text = $"{_currentCharacter.attackStat}";
        speedText.text = $"{_currentCharacter.speed}";
    }
    
    public string WriteData()
    {
        return JsonUtility.ToJson(_currentCharacter);
    }
}