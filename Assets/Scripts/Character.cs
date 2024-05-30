using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public string charName;
    public int level;
    public int maxHP;
    private int _hp;
    public int speed;
    public int attackStat;

    public int HP
    {
        get => _hp;
        set => _hp = Mathf.Clamp(value, 0, maxHP);
    }

    public virtual void NormalAttack(Character target)
    {
        target.GetDamage(attackStat);
    }

    public void GetDamage(int damage)
    {
        HP -= damage;
    }
}
