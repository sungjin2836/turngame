using UnityEngine;

public abstract class Character : MonoBehaviour, System.IComparable<Character>
{
    [Header("기본 캐릭터 정보")] public string charName;
    public int level;
    public int maxHP;
    private int _hp;

    public int HP
    {
        get => _hp;
        private set => _hp = Mathf.Clamp(value, 0, maxHP);
    }

    public int speed;
    public int attackStat;
    public float attackPower = 1f;

    [Header("캐릭터 상황")] public bool isDead;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public virtual void NormalAttack(Character target)
    {
        target.GetDamage(Mathf.FloorToInt(attackStat * attackPower));
        Debug.Log($"{target.charName}의 체력은 {target.HP}/{target.maxHP}");
    }

    public void GetDamage(int damage, bool isShield = false)
    {
        if (isShield)
        {
            HP -= Mathf.FloorToInt(damage * 0.9f);
        }
        else
        {
            HP -= damage;
        }

        if (HP == 0)
        {
            Die();
        }
    }

    public void HealSelf(int value)
    {
        HP += value;
    }

    private void Die()
    {
        isDead = true;
        Debug.Log($"{charName}은 죽었다!");
    }

    public int CompareTo(Character other)
    {
        return other.speed - speed;
    }

    public void TurnEnd()
    {
        // 애니메이션이 종료되었을 때 다음 순서로 넘어감 (애니메이션 이벤트로 사용)
    }
}