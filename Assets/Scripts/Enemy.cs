using UnityEngine;

public class Enemy : Character
{
    private int _maxShield;

    public int shield
    {
        get => _maxShield;
        set => _maxShield = Mathf.Clamp(value, 0, _maxShield);
    }

    public ElementType[] weakElements;
}