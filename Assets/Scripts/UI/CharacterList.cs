using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterList : MonoBehaviour
{
    private const int PANEL_COUNT = 2;
    private const int STAT_HP = 6;
    private const int STAT_ATTACK = 4;

    private int _characterID;
    private static DataManager.Player _currentCharacter;
    private readonly Dictionary<DataManager.Player, int> _expPair = new();
    private readonly Dictionary<Button, Image> _panelPair = new();

    private static int MaxExp => _currentCharacter.reachableLevels[_currentCharacter.level - 1];

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

    private void Start()
    {
        InitializeUI();
        LoadCharacterList();
    }

    private void InitializeUI()
    {
        if (!TryAdd(uiObjects.view)) return;

        foreach (Button button in uiObjects.view.pageButtons)
        {
            button.onClick.AddListener(() => { EnablePanel(button); });
        }

        foreach (Button button in uiObjects.view.pageButtons)
        {
            button.interactable = !_panelPair[button].gameObject.activeSelf;
        }
    }

    private UserDataManager.User userData;
    private UserDataManager.OwnedCharacter[] characters;

    private void LoadCharacterList()
    {
        userData = UserDataManager.Instance.UserData;
        UserDataManager.OwnedCharacter[] characters = userData.ownedCharacter;
        foreach (UserDataManager.OwnedCharacter character in characters)
        {
            AddCharacterIcon(character.characterID, character.currentLevel, character.currentExp);
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
        _onValueChanged += WriteData;
        uiObjects.details.levelUpButton.onClick.AddListener(() => AddExp(1000));
    }

    private void OnDisable()
    {
        _onValueChanged -= UpdateUI;
        _onValueChanged -= WriteData;
        uiObjects.details.levelUpButton.onClick.RemoveAllListeners();
    }

    private void AddCharacterIcon(int id, int level = 1, int exp = 0)
    {
        CharacterIcon icon = new GameObject($"Icon_{id}").AddComponent<CharacterIcon>();
        icon.transform.SetParent(uiObjects.view.content);

        icon.GetCharacterData(id);
        
        icon.Data.level = level;
        icon.Data.hp += level * STAT_HP;
        icon.Data.attackStat += level * STAT_ATTACK;
        
        _expPair.Add(icon.Data, exp);

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
        bool isMaxLevel = _currentCharacter.level >= _currentCharacter.reachableLevels.Length;
        if (isMaxLevel) return;

        while (newExp >= MaxExp)
        {
            newExp -= MaxExp;
            LevelUp();
        }

        _expPair[_currentCharacter] = newExp;

        _onValueChanged?.Invoke();
    }

    private static void LevelUp()
    {
        _currentCharacter.level++;
        _currentCharacter.hp += STAT_HP;
        _currentCharacter.attackStat += STAT_ATTACK;
    }

    private void UpdateUI()
    {
        UIObject.ListDetail details = uiObjects.details;
        /*details.elementImage.sprite = Resources.Load<Sprite>($"Images/DoNotShare/{(int)_currentCharacter.elem}");*/
        details.elementImage.color = ElementColor(_currentCharacter.elem);
        details.nameText.text = _currentCharacter.charName;
        details.levelText.text = $"Lv.{_currentCharacter.level}/20";
        details.expSlider.maxValue = MaxExp;
        details.expSlider.value = _expPair[_currentCharacter];
        details.hpText.text = $"{_currentCharacter.hp}";
        details.atkText.text = $"{_currentCharacter.attackStat}";
        details.speedText.text = $"{_currentCharacter.speed}";

        UIObject.ListEquipment equipments = uiObjects.equipments;
        equipments.hpText.text = $"{_currentCharacter.hp}";
        equipments.atkText.text = $"{_currentCharacter.attackStat}";
        equipments.speedText.text = $"{_currentCharacter.speed}";
    }

    private void WriteData()
    {
        userData.ownedCharacter.FirstOrDefault(x => x.characterID == _currentCharacter.id)!.currentLevel =
            _currentCharacter.level;
        userData.ownedCharacter.FirstOrDefault(x => x.characterID == _currentCharacter.id)!.currentExp =
            _expPair[_currentCharacter];

        string jsonData = JsonUtility.ToJson(userData);
        string path = Path.Combine(Application.dataPath, "Resources/Data", "userdata.json");
        FileInfo file = new(path);
        file.Directory.Create();
        File.WriteAllText(file.FullName, jsonData);
    }

    private static Color ElementColor(ElementType type)
    {
        return type switch
        {
            ElementType.Physical => Color.white,
            ElementType.Fire => Color.red,
            ElementType.Ice => Color.cyan,
            ElementType.Lightning => new Color(0.5f, 0, 1f),
            ElementType.Wind => Color.green,
            ElementType.Quantum => new Color(0.3f, 0.3f, 0.9f),
            ElementType.Imaginary => Color.yellow,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}