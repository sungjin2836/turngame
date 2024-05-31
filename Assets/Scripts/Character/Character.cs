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
    public int _hp;

    
    public bool isDead { get; private set; }

    private Animator _animator;


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

    public virtual int NormalAttack(Character target, float value = 1f)
    {
        
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
            return finaldam;
        }
        else
        {
            hp -= damage;
        }
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

    public virtual void TurnEnd()
    {
        OnTurnEnd?.Invoke();
    }
}