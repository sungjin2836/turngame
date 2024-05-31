using System;
using UnityEngine;

public abstract class Character : MonoBehaviour, IComparable<Character>
{
    public Action OnTurnEnd;
    
    [Header("기본 캐릭터 정보")] public string charName;
    public int level;
    public int maxHP;
    public int speed;
    public int attackStat;
    
    public bool isDead { get; private set; }

    private Animator _animator;

    private int _hp;

    public int hp
    {
        get => _hp;
        protected set => _hp = Mathf.Clamp(value, 0, maxHP);
    }

    public int finalSpeed { get; protected set; }
    public int finalAttackStat { get; protected set; }

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public abstract void Initialize(int id);

    public int CompareTo(Character other)
    {
        return other.speed - speed;
    }

    public virtual void NormalAttack(Character target, float value = 1f)
    {
        target.GetDamage(Mathf.FloorToInt(finalAttackStat * value));
        Debug.Log($"{target.charName}의 체력은 {target.hp}/{target.maxHP}");
    }

    public void GetDamage(int damage, bool hasShield = false)
    {
        if (hasShield)
            hp -= Mathf.FloorToInt(damage * 0.9f);
        else
            hp -= damage;

        if (hp == 0) Die();
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

    public virtual void TurnEnd()
    {
        OnTurnEnd?.Invoke();
    }
}