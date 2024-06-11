using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    static public UserDataManager Instance { get; private set; }
    public User UserData { get; private set; }

    private const string DATA_PATH = "Data";
    private const string USERDATA_JSON = "userdata";

    [System.Serializable]
    public class OwnedCharacter
    {
        public int characterID;
        public int currentHP;
        public int currentLevel;
        public int[] skillLevel;
    }

    [System.Serializable]
    public class Inventory
    {
        public int id;
        public int count;
    }

    [System.Serializable]
    public class PartyPreset
    {
        public string partyName;
        public int[] preset;
    }

    [System.Serializable]
    public class User
    {
        public string username;
        public OwnedCharacter[] ownedCharacter;
        public PartyPreset[] partyPreset;
        public int currentPartyIndex;
        public Inventory[] inventory;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitItemData();
    }

    private void InitItemData()
    {
        TextAsset userJson = Resources.Load<TextAsset>(Path.Combine(DATA_PATH, USERDATA_JSON));
        UserData = JsonUtility.FromJson<User>(userJson.text);
    }
}
