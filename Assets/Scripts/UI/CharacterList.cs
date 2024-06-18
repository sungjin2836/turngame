using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
public class CharacterList : MonoBehaviour
{
    private const int PANEL_COUNT = 2;

    private int _characterID;
    private static DataManager.Player _currentCharacter;

    private UserDataManager.User userData;
    private readonly Dictionary<DataManager.Player, UserDataManager.OwnedCharacter> _ownedCharacters = new();
    private readonly Dictionary<Button, Image> _panelPair = new();

    private int MaxExp => _currentCharacter.reachableLevels[_ownedCharacters[_currentCharacter].currentLevel];

    private static Action _onValueChanged;

    private PlayerInput _input;

    [Serializable]
    private struct UIObject
    {
        [Serializable]
        public struct ListView
        {
            public Button exitButton;
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
        _input = GetComponent<PlayerInput>();

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

    private void LoadCharacterList()
    {
        userData = UserDataManager.Instance.UserData;
        UserDataManager.OwnedCharacter[] characters = userData.ownedCharacter;
        foreach (UserDataManager.OwnedCharacter character in characters)
        {
            AddCharacterIcon(character.characterID);
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

        uiObjects.view.exitButton.onClick.AddListener(ExitToFieldScene);
        uiObjects.details.levelUpButton.onClick.AddListener(() => AddExp(1000));
    }

    private void OnDisable()
    {
        _onValueChanged -= UpdateUI;
        _onValueChanged -= WriteData;
        
        uiObjects.view.exitButton.onClick.RemoveAllListeners();
        uiObjects.details.levelUpButton.onClick.RemoveAllListeners();
    }

    private void AddCharacterIcon(int id)
    {
        CharacterIcon icon = new GameObject($"Icon_{id}").AddComponent<CharacterIcon>();
        icon.transform.SetParent(uiObjects.view.content);

        icon.GetCharacterData(id);
        _ownedCharacters.Add(icon.DefaultData, icon.UserData);

        if (_ownedCharacters.Count == 1) SelectCharacter(icon);
    }

    public static void SelectCharacter(CharacterIcon icon)
    {
        _currentCharacter = icon.DefaultData;
        _onValueChanged?.Invoke();
    }

    private void AddExp(int value)
    {
        int newExp = _ownedCharacters[_currentCharacter].currentExp + value;
        bool isMaxLevel = _currentCharacter.level >= _currentCharacter.reachableLevels.Length;
        if (isMaxLevel) return;

        while (newExp >= MaxExp)
        {
            newExp -= MaxExp;
            _ownedCharacters[_currentCharacter].currentLevel++;
        }

        _ownedCharacters[_currentCharacter].currentExp = newExp;

        _onValueChanged?.Invoke();
    }

    private void UpdateUI()
    {
        UIObject.ListDetail details = uiObjects.details;
        details.elementImage.color = Character.ElementColor(_currentCharacter.elem);
        details.nameText.text = _currentCharacter.charName;
        details.levelText.text = $"Lv.{_ownedCharacters[_currentCharacter].currentLevel}/20";
        details.expSlider.maxValue = MaxExp;
        details.expSlider.value = _ownedCharacters[_currentCharacter].currentExp;
        details.hpText.text = $"{_currentCharacter.hp + (_ownedCharacters[_currentCharacter].currentLevel - 1) * Character.STAT_HP}";
        details.atkText.text = $"{_currentCharacter.attackStat + (_ownedCharacters[_currentCharacter].currentLevel - 1) * Character.STAT_ATTACK}";
        details.speedText.text = $"{_currentCharacter.speed}";

        UIObject.ListEquipment equipments = uiObjects.equipments;
        equipments.hpText.text = $"{_currentCharacter.hp + (_ownedCharacters[_currentCharacter].currentLevel - 1) * Character.STAT_HP}";
        equipments.atkText.text = $"{_currentCharacter.attackStat + (_ownedCharacters[_currentCharacter].currentLevel - 1) * Character.STAT_ATTACK}";
        equipments.speedText.text = $"{_currentCharacter.speed}";
    }

    private void WriteData()
    {
        string jsonData = JsonUtility.ToJson(userData);
        string path = Path.Combine(Application.dataPath, "Resources/Data", "userdata.json");
        FileInfo file = new(path);
        file.Directory.Create();
        File.WriteAllText(file.FullName, jsonData);
    }

    private void Update()
    {
        if (_input.Cancel)
        {
            ExitToFieldScene();
        }
    }

    private void ExitToFieldScene()
    {
        SceneManager.LoadScene("FieldScene");
    }
}