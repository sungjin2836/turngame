using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CooperativeSkillDataManager : MonoBehaviour
{
    static public CooperativeSkillDataManager Instance { get; private set; }
    public Dictionary<string, CooperativeSkill> CoSkills { get; private set; }

    private const string DATA_PATH = "Data";
    private const string COSKILL_JSON = "cooperativeSkill";

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
    public class CooperativeSkill
    {
        public string id;
        public string name;
        public int cooperativeId;
        public float[] damageAttr1;
        public DamageType damageAttr1Type;
        public float[] damageAttr2;
        public DamageType damageAttr2Type;
        public Range range1;
        public Range range2;
        public Distance distance1;
        public Distance distance2;
    }

    public class CooperativeSkillData
    {
        public CooperativeSkill[] cooperativeSkill;
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
        InitCoSkillData();

    }

    private void InitCoSkillData()
    {
        CoSkills = new();

        TextAsset cooperativeSkillJson = Resources.Load<TextAsset>(Path.Combine(DATA_PATH, COSKILL_JSON));
        CooperativeSkillData cooperativeSkillList = JsonUtility.FromJson<CooperativeSkillData>(cooperativeSkillJson.text);
        
        foreach (var data in cooperativeSkillList.cooperativeSkill)
        {
            CoSkills.Add(data.id, data);
        }
    }

    public CooperativeSkill GetCoSkillData(string id)
    {
        return CoSkills[id];
    }

    
}
    
