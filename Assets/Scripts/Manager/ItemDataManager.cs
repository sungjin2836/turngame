using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    static public ItemDataManager Instance { get; private set; }
    public Dictionary<string, Item> ItemDataTable { get; private set; }

    private const string DATA_PATH = "Data";
    private const string ITEM_JSON = "Items";

    public enum ItemType
    {
        heal,
        exp
    }

    public enum Scope
    {
        single,
        all
    }

    public enum ChangeType
    {
        point,
        percent
    }

    [System.Serializable]
    public class Attribute
    {
        public ItemType itemType;
        public Scope scope;
        public ChangeType changeType;
        public int value;
    }


    [System.Serializable]
    public class Item
    {
        public string id;
        public string name;
        public List<Attribute> attributes;
    }

    public class ItemData
    {
        public Item[] items;
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
        ItemDataTable = new();

        TextAsset itemJson = Resources.Load<TextAsset>(Path.Combine(DATA_PATH, ITEM_JSON));
        ItemData itemList = JsonUtility.FromJson<ItemData>(itemJson.text);


        foreach (var data in itemList.items)
        {
            ItemDataTable.Add(data.id, data);
        }
    }

    public Item GetItemData(string id)
    {
        return ItemDataTable[id];
    }
}
