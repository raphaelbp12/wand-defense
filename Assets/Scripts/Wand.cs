using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StatType
{
    Area,
    Vampire,
    SplitCount,
    Scatter,
    Duration,
    Range,
    CooldownPeriod,
    Damage,
    TravelSpeed,
    DamageCooldown,
    Mana,
    ManaRegen,
}

public enum ModifierOperator
{
    Sum,
    Mult
}

public class Stat
{

    public Stat(StatType nType, float nValue)
    {
        type = nType;
        value = nValue;
    }

    public StatType type;
    public float value;

    public Stat Copy()
    {
        return new Stat(type, value);
    }
}

[System.Serializable]
public class StatModifier
{
    public StatType type;
    public ModifierOperator opp;
    public float value;

    public StatModifier(StatType nType, ModifierOperator nOpp, float nValue)
    {
        type = nType;
        opp = nOpp;
        value = nValue;
    }

    // A parameterless constructor is handy for Unity's serialization
    public StatModifier() { }
}

public class StatTable
{
    Dictionary<StatType, Stat> stats;

    public StatTable(Dictionary<StatType, Stat> baseStats)
    {
        this.Initialize();

        foreach (var kvp in baseStats)
        {
            stats[kvp.Key] = baseStats[kvp.Key].Copy();
        }
    }

    void Initialize()
    {
        stats = new Dictionary<StatType, Stat>();

        foreach (StatType type in System.Enum.GetValues(typeof(StatType)))
        {
            stats.Add(type, new Stat(type, 0));
        }
    }

    public void ApplyModifier(StatModifier mod)
    {
        Stat existingStat = stats[mod.type];

        switch (mod.opp)
        {
            case ModifierOperator.Sum:
                existingStat.value += mod.value;
                break;
            case ModifierOperator.Mult:
                existingStat.value += existingStat.value * mod.value;
                break;
            default:
                break;
        }
    }

    public Stat GetStat(StatType type)
    {
        return stats[type].Copy();
    }

}

public class Wand : MonoBehaviour
{
    public BarController manaBarController;
    [Header("Skills")]
    [SerializeField]
    public List<SkillSO> initialSkills;  // Assigned in Inspector

    [HideInInspector]
    public Transform towerTransform;

    // Store the base stats in a serializable dictionary or in code here:
    private Dictionary<StatType, Stat> baseStats = new Dictionary<StatType, Stat>
    {
        { StatType.Range,           new Stat(StatType.Range,       5) },
        { StatType.CooldownPeriod,  new Stat(StatType.CooldownPeriod,    0.5f) },
        { StatType.Damage,          new Stat(StatType.Damage,      10) },
        { StatType.TravelSpeed,     new Stat(StatType.TravelSpeed, 2) },
        { StatType.Mana,            new Stat(StatType.Mana, 50) },
        { StatType.ManaRegen,       new Stat(StatType.ManaRegen, 10) },
    };

    private StatTable statTable;
    private float attackTimer = 0f;
    private float currentMana = 0f;
    private float manaPerProjectile = 9f;

    private List<GameObject> projectilesGO = new List<GameObject>();

    private void Start()
    {
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;

        if (manaBarController != null)
        {
            var manaPercent = currentMana / statTable.GetStat(StatType.Mana).value;
            manaBarController.SetPercentage(manaPercent);
            manaBarController.SetText($"{currentMana}/{statTable.GetStat(StatType.Mana).value}");
        }
    }

    public void Shoot(float distanceToEnemy)
    {
        if (attackTimer >= statTable.GetStat(StatType.CooldownPeriod).value && currentMana >= manaPerProjectile)
        {
            if (distanceToEnemy <= statTable.GetStat(StatType.Range).value)
            {
                ShootProjectileAt();
                attackTimer = 0f;
                currentMana -= manaPerProjectile;
            }
        }
    }

    private void FixedUpdate()
    {
        currentMana += statTable.GetStat(StatType.ManaRegen).value * Time.fixedDeltaTime;
        currentMana = Mathf.Clamp(currentMana, 0, statTable.GetStat(StatType.Mana).value);
    }

    public void Initialize()
    {
        currentMana = statTable.GetStat(StatType.Mana).value;
    }

    /// <summary>
    /// Recreates the StatTable from base stats, then applies all current skills' modifiers.
    /// </summary>
    public void RecalculateStats(List<SkillSO> skillSOs)
    {
        // Create a fresh StatTable from the base stats
        statTable = new StatTable(baseStats);

        // Apply each SkillSO in the list
        foreach (SkillSO skill in skillSOs)
        {
            foreach (StatModifier mod in skill.modifiers)
            {
                statTable.ApplyModifier(mod);
            }
        }
    }

    public void SetActiveSpells(List<SkillSO> activeSpells)
    {
        projectilesGO = activeSpells.Select(x => x.projectilePrefab).ToList();
    }

    /// <summary>
    /// Call this if the skill list changes at runtime.
    /// e.g. "AddSkill(myNewSkillSO)" then "RecalculateStats()"
    /// </summary>
    /// <param name="skill">The skill to add.</param>
    public void AddSkill(SkillSO skill)
    {
        if (!initialSkills.Contains(skill))
        {
            initialSkills.Add(skill);
            RecalculateStats(new List<SkillSO>());
        }
    }

    /// <summary>
    /// Removes a skill from the list, if present, then recalculates.
    /// </summary>
    /// <param name="skill">The skill to remove.</param>
    public void RemoveSkill(SkillSO skill)
    {
        if (initialSkills.Contains(skill))
        {
            initialSkills.Remove(skill);
            RecalculateStats(new List<SkillSO>());
        }
    }

    private void ShootProjectileAt()
    {
        if (projectilesGO == null || projectilesGO.Count == 0) return;

        var projectilePrefab = projectilesGO.First();

        // Instantiate the projectile at the wand's current position
        GameObject projObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            // Calculate direction from the wand to the enemy
            Vector3 direction = transform.right;

            // Initialize the projectile with direction, damage, and speed
            float damage = statTable.GetStat(StatType.Damage).value;
            float travelSpeed = statTable.GetStat(StatType.TravelSpeed).value;
            projectile.Initialize(direction, damage, travelSpeed);
        }
    }
}