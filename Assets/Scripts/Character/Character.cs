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
    public int actionGauge;
    public int _currentActionGauge;

    private bool _go;
    private Vector3 _startPos;
    private Vector3 _targetPos;
    private float _lastTime;
    protected float Duration = .7f;

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
        _startPos = transform.position;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        float currentTime = Time.time - _lastTime;

        if (_go)
        {
            transform.position = Vector3.Lerp(_startPos, _targetPos, currentTime / Duration);
            if (Vector3.Distance(transform.position, _targetPos) < 1e-2f) return;

            _go = false;
            _lastTime = Time.time;

            //체력바 최신화
            /*if (!_targetPos.CompareTag("Enemy")) return;
            Enemy targetEnemy = _targetPos.gameObject.GetComponent<Enemy>();
            targetEnemy.SetHealth();
            targetEnemy.SetShield();*/
        }
        else if (!_go && currentTime > 0.5f)
        {
            transform.position = _startPos;
            _lastTime = 0f;
            TurnEnd();
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

    protected virtual void MoveToTarget(Character target)
    {
        _go = true;
        _lastTime = Time.time;
        _targetPos = target.transform.position;
    }
}