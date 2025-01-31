using System.Collections.Generic;
using UnityEngine;

public class ProjectileData
{
    public GameObject prefab { get; private set; }
    public SkillSO activeSpell { get; private set; }
    private Dictionary<StatType, Stat> baseStats = new Dictionary<StatType, Stat> { };
    public StatTable statTable { get; private set; }
    public float ManaCost { get { return statTable.GetStat(StatType.ManaCost).value; } }
    public ProjectileData(SkillSO activeSpell, List<StatModifier> statModifiers)
    {
        foreach (Stat stat in activeSpell.initialStats)
        {
            baseStats.Add(stat.type, stat);
        }
        this.statTable = new StatTable(baseStats);
        this.prefab = activeSpell.projectilePrefab;
        this.activeSpell = activeSpell;

        foreach (StatModifier statModifier in statModifiers)
        {
            statTable.ApplyModifier(statModifier);
        }
    }
}
