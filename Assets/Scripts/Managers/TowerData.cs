using UnityEngine;

public class TowerData
{
    public float MaxHP { get; private set; }
    public float CurrentHP { get; private set; }
    public TowerData(float maxHP)
    {
        MaxHP = maxHP;
        CurrentHP = maxHP;
    }
    public float AddMaxHP(float amount)
    {
        MaxHP += amount;
        CurrentHP += amount;
        return MaxHP;
    }

    public void SetCurrentHP(float amount)
    {
        CurrentHP = amount;
    }

    public void SetMaxHP(float amount)
    {
        MaxHP = amount;
    }

    public void Reset()
    {
        CurrentHP = MaxHP;
    }
}
