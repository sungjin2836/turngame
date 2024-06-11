using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterList : MonoBehaviour
{
    private const int PANEL_COUNT = 2;
    
    private int _characterID;
    private static DataManager.Player _currentCharacter;
    private readonly Dictionary<DataManager.Player, int> _expPair = new();
    private readonly Dictionary<Button, Image> _panelPair = new();
    
    // TODO - Fix maxExp to return max experience from data
    private static int maxExp => 200 * _currentCharacter.level;

    private static Action _onValueChanged;

    [Serializable]
    private struct UIObject
    {
        [Serializable]
        public struct ListView
        {
            public Transform content;
            public Button[] pageButtons;
            public Image[] panels;
        }

        [Serializable]
        public struct ListDetail
        {
            public Image elementImage;
            public Text nameText;
            public Text levelText;
            public Text hpText;
            public Text atkText;
            public Text speedText;
            public Slider expSlider;
            public Button levelUpButton;
        }

        [Serializable]
        public struct ListEquipment
        {
            public Button headSlot;
            public Button armsSlot;
            public Button legsSlot;
            public Button chestSlot;
            public Text hpText;
            public Text atkText;
            public Text speedText;
        }

        public ListView view;
        public ListDetail details;
        public ListEquipment equipments;
    }

    [SerializeField] private UIObject uiObjects;

    private void Awake()
    {
        Initialize();

        foreach (var button in uiObjects.view.pageButtons)
        {
            button.interactable = !_panelPair[button].gameObject.activeSelf;
        }
    }

    private void Initialize()
    {
        if (!TryAdd(uiObjects.view)) return;

        foreach (var button in uiObjects.view.pageButtons)
        {
            button.onClick.AddListener(() => { EnablePanel(button); });
        }
    }

    private void EnablePanel(Button button)
    {
        foreach (var b in uiObjects.view.pageButtons)
        {
            b.interactable = true;
            _panelPair[b].gameObject.SetActive(false);
        }
                
        button.interactable = false;
        _panelPair[button].gameObject.SetActive(true);
    }

    private bool TryAdd(UIObject.ListView view)
    {
        if (view.pageButtons.Length != PANEL_COUNT || view.panels.Length != PANEL_COUNT)
        {
            Debug.LogError("Panels or buttons amount are not equal");
            return false;
        }

        for (int i = 0; i < PANEL_COUNT; i++)
        {
            _panelPair.Add(view.pageButtons[i], view.panels[i]);
        }
        return true;
    }

    private void OnEnable()
    {
        _onValueChanged += UpdateUI;
        uiObjects.details.levelUpButton.onClick.AddListener(() => AddExp(1000));
    }

    private void OnDisable()
    {
        _onValueChanged -= UpdateUI;
        uiObjects.details.levelUpButton.onClick.RemoveAllListeners();
    }

    public void AddCharacter(int id)
    {
        CharacterIcon icon = new GameObject($"Icon_{id}").AddComponent<CharacterIcon>();
        icon.transform.SetParent(uiObjects.view.content);

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
        UIObject.ListDetail details = uiObjects.details;
        // TODO - Use Dictionary instead of Resource.Load() 
        details.elementImage.sprite = Resources.Load<Sprite>($"Images/DoNotShare/{(int)_currentCharacter.elem}");
        details.nameText.text = _currentCharacter.charName;
        details.levelText.text = $"Lv.{_currentCharacter.level}/20";
        details.expSlider.maxValue = maxExp;
        details.expSlider.value = _expPair[_currentCharacter];
        details.hpText.text = $"{_currentCharacter.hp}";
        details.atkText.text = $"{_currentCharacter.attackStat}";
        details.speedText.text = $"{_currentCharacter.speed}";

        UIObject.ListEquipment equipments = uiObjects.equipments;
        equipments.hpText.text = $"{_currentCharacter.hp}";
        equipments.atkText.text = $"{_currentCharacter.attackStat}";
        equipments.speedText.text = $"{_currentCharacter.speed}";
    }

    public string WriteData()
    {
        return JsonUtility.ToJson(_currentCharacter);
    }
}