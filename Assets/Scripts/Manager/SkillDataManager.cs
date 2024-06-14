using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SkillDataManager : MonoBehaviour
{
    static public SkillDataManager Instance { get; private set; }
    public Dictionary<string, Skill> Skills { get; private set; }

    private const string DATA_PATH = "Data";
    private const string SKILL_JSON = "Skill";

    public enum Range
    {
        single,
        all
    }

    public enum DamageType
    {
        none,
        attack,
        heal
    }

    public enum Distance
    {
        close,
        far
    }
    
    [System.Serializable]
    public class Skill 
    {
        public string id;
        public string name;
        public float[] damageAttr1;
        public DamageType damageAttr1Type;
        public float[] damageAttr2;
        public DamageType damageAttr2Type;
        public Range range;
        public Distance distance;
    }

    public class SkillData
    {
        public Skill[] skill;
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
        InitSkillData();
    }

    private void InitSkillData()
    {
        Skills = new();

        TextAsset skillJson = Resources.Load<TextAsset>(Path.Combine(DATA_PATH, SKILL_JSON));
        SkillData skillList = JsonUtility.FromJson<SkillData>(skillJson.text);

        foreach (var data in skillList.skill)
        {
            Skills.Add(data.id, data);
        }
    }

    public Skill GetSkillData(string id)
    {
        return Skills[id];
    }
}
