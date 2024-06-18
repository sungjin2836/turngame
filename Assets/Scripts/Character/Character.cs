using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Character : MonoBehaviour, IComparable<Character>
{
    public const int STAT_HP = 6;
    public const int STAT_ATTACK = 4;

    public Action OnTurnEnd;

    [Header("기본 캐릭터 정보")] public string charName;
    public int level;
    public int maxHP;
    public int speed;
    public int attackStat;
    public int _hp;
    public int actionGauge;
    public int _currentActionGauge;

    [HideInInspector] public Vector3 startPos;
    protected Vector3 TargetPos;

    public bool isDead { get; private set; }

    private Animator _animator;

    public int hp
    {
        get => _hp;

        private set => _hp = Mathf.Clamp(value, 0, maxHP);
    }

    public int finalSpeed { get; protected set; }

    public int currentActionGauge
    {
        get => _currentActionGauge;
        protected set => _currentActionGauge = Mathf.Clamp(value, 0, 1000);
    }

    public int finalAttackStat { get; protected set; }

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().name != "FieldScene")
        {
            MoveTowards();
        }
    }

    public abstract void Initialize(int id);

    public int CompareTo(Character other)
    {
        return (actionGauge - currentActionGauge) - (other.actionGauge - other.currentActionGauge);
    }

    public virtual void GetActionGauge()
    {
        actionGauge = Mathf.FloorToInt(10000 / speed);
    }

    public virtual int NormalAttack(Character target, float value = 1f)
    {
        TargetPos = target.transform.position + target.transform.forward;
        int dam = target.GetDamage(Mathf.FloorToInt(finalAttackStat * value));
        Debug.Log($"{target.charName}의 체력은 {target.hp}/{target.maxHP}");
        return dam;
    }

    public int GetDamage(int damage, bool hasShield = false)
    {
        int finaldam = Mathf.FloorToInt(damage * 0.9f);
        if (hasShield)
        {
            hp -= finaldam;
            if (hp == 0) Die();
            return finaldam;
        }
        else
        {
            hp -= damage;
            
        }

        Debug.Log($"{hasShield}, {damage}, {finaldam}, {hp}, {_hp}, {name}");

        if (hp == 0) Die();
        return damage;
    }

    public void HealSelf(int value)
    {
        hp += value;
    }

    private void Die()
    {
        isDead = true;
        Debug.Log($"{charName}은 죽었다!");
    }

    protected virtual void TurnEnd()
    {
        OnTurnEnd?.Invoke();
    }

    private void CorrectionSpeed()
    {
        int CorrectionValue = UnityEngine.Random.Range(0, 5);
    }

    public void AddGauge(int num)
    {
        currentActionGauge += num;
    }

    public void InitGauge()
    {
        currentActionGauge = currentActionGauge - actionGauge;
    }
    public static Color ElementColor(ElementType type)
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
    protected virtual void MoveTowards()
    {
        if (TargetPos == Vector3.zero) return;
        transform.position = Vector3.MoveTowards(transform.position, TargetPos, .2f);
        if (Vector3.Distance(transform.position, TargetPos) < 1e-2f) TargetPos = startPos;
        if (Vector3.Distance(transform.position, startPos) < 1e-2f)
        {
            TargetPos = Vector3.zero;
            TurnEnd();
        }
    }
}