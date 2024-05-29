using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int hp;
    public int speed;
    public int attackStat;

    public virtual void NormalAttack(Character target)
    {
        target.GetDamage(attackStat);
    }

    public void GetDamage(int damage)
    {
        hp -= damage;
    }
}
