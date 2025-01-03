using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;



public enum StatType
{
    Area,
    Vampire,
    SplitCount,
    Scatter,
    Duration,
    Range,
    Interval,
    Damage,
    TravelSpeed,
    DamageCooldown,
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
            stats[kvp.Key] = baseStats[kvp.Key];
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
        Stat stat = stats[mod.type];

        switch (mod.opp)
        {
            case ModifierOperator.Sum:
                stat.value += mod.value;
                break;
            case ModifierOperator.Mult:
                stat.value += stat.value * mod.value;
                break;
            default:
                break;
        }
    }

    public Stat GetStat(StatType type)
    {
        return stats[type];
    }

}

public class Wand : MonoBehaviour
{
    [Header("Skills")]
    [SerializeField]
    private List<SkillSO> initialSkills;  // Assigned in Inspector

    [HideInInspector]
    public Transform towerTransform;
    public GameObject projectilePrefab;

    // Store the base stats in a serializable dictionary or in code here:
    private Dictionary<StatType, Stat> baseStats = new Dictionary<StatType, Stat>
    {
        { StatType.Range,       new Stat(StatType.Range,       5) },
        { StatType.Interval,    new Stat(StatType.Interval,    0.1f) },
        { StatType.Damage,      new Stat(StatType.Damage,      10) },
        { StatType.TravelSpeed, new Stat(StatType.TravelSpeed, 2) },
    };

    private StatTable statTable;
    private float attackTimer = 0f;

    private void Start()
    {
        // 1) Initialize the StatTable once at startup.
        // 2) Immediately recalculate stats with the current skill list.
        RecalculateStats();
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= statTable.GetStat(StatType.Interval).value)
        {
            // Attempt to find and attack the closest enemy
            Enemy closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                float distance = Vector3.Distance(transform.position, closestEnemy.transform.position);
                if (distance <= statTable.GetStat(StatType.Range).value)
                {
                    ShootProjectileAt(closestEnemy);
                }
            }
            attackTimer = 0f;
        }
    }

    /// <summary>
    /// Recreates the StatTable from base stats, then applies all current skills' modifiers.
    /// </summary>
    public void RecalculateStats()
    {
        // Create a fresh StatTable from the base stats
        statTable = new StatTable(baseStats);

        // Apply each SkillSO in the list
        foreach (SkillSO skill in initialSkills)
        {
            foreach (StatModifier mod in skill.modifiers)
            {
                statTable.ApplyModifier(mod);
            }
        }
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
            RecalculateStats();
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
            RecalculateStats();
        }
    }

    private Enemy FindClosestEnemy()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Enemy closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            if (!enemy) continue;

            float distance = Vector3.Distance(towerTransform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    private void ShootProjectileAt(Enemy target)
    {
        if (projectilePrefab == null || target == null) return;

        // Instantiate the projectile at the wand's current position
        GameObject projObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            // Calculate direction from the wand to the enemy
            Vector3 direction = (target.transform.position - transform.position).normalized;

            // Initialize the projectile with direction, damage, and speed
            float damage = statTable.GetStat(StatType.Damage).value;
            float travelSpeed = statTable.GetStat(StatType.TravelSpeed).value;
            projectile.Initialize(direction, damage, travelSpeed);
        }
    }
}