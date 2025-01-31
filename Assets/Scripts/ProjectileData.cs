using System.Collections.Generic;
using UnityEngine;

public class ProjectileData
{
    public GameObject prefab { get; private set; }
    public SkillSO activeSpell { get; private set; }
    public List<SkillSO> supportSpells { get; private set; }
    private Dictionary<StatType, Stat> baseStats = new Dictionary<StatType, Stat> { };
    public StatTable statTable { get; private set; }
    public float ManaCost { get { return statTable.GetStat(StatType.ManaCost).value; } }
    public float Range { get { return statTable.GetStat(StatType.Range).value; } }
    public ProjectileData(SkillSO activeSpell, List<SkillSO> spells)
    {
        foreach (Stat stat in activeSpell.initialStats)
        {
            baseStats.Add(stat.type, stat);
        }
        this.statTable = new StatTable(baseStats);
        this.prefab = activeSpell.projectilePrefab;
        this.activeSpell = activeSpell;
        this.supportSpells = spells;

        foreach (SkillSO skill in spells)
        {
            foreach (StatModifier mod in skill.modifiers)
            {
                statTable.ApplyModifier(mod);
            }
        }
    }
}
