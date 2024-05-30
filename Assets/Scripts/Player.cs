using UnityEngine;

public class Player : Character
{
    [Header("플레이어 캐릭터 정보")] public ElementType element;
    public float skillPower = 1f;

    public override void NormalAttack(Character target)
    {
        var enemy = target as Enemy;
        if (enemy.CheckElement(element)) enemy.DamageToShield(30);
        enemy.GetDamage(Mathf.FloorToInt(attackStat * attackPower), enemy.CheckShield());
        Debug.Log($"{enemy.charName}의 체력은 {enemy.HP}/{enemy.maxHP}, 실드는 {enemy.shield}/{enemy.maxShield}");
    }

    public virtual void BattleSkill(Character[] target)
    {
        // 스킬
    }
}