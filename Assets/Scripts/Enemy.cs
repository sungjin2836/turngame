using System.Linq;
using UnityEngine;

public class Enemy : Character
{
    [Header("적 캐릭터 정보")] public int maxShield;
    private int _shield;

    public int shield
    {
        get => _shield;
        private set => _shield = Mathf.Clamp(value, 0, maxShield);
    }

    public ElementType[] weakElements;

    public bool CheckElement(ElementType element)
    {
        return weakElements.Contains(element);
    }

    public bool CheckShield()
    {
        return shield > 0;
    }

    public void DamageToShield(int damage)
    {
        shield -= damage;
    }

    public void RegenShield()
    {
        shield = maxShield;
    }
}