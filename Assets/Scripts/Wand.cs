using System;
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
    ManaCost,
    ManaRegen,
    CastTime,
}

public enum ModifierOperator
{
    Sum,
    Mult
}

[System.Serializable]
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
    private Dictionary<StatType, Stat> baseStats = new Dictionary<StatType, Stat> { };
    public List<Stat> initialStats;

    private StatTable statTable;
    private float attackTimer = Mathf.Infinity;
    private float currentMana = 0f;
    private List<ProjectileData> projectileDatas = new List<ProjectileData>();

    private int currentProjectileIndex = 0;
    private List<SkillSO> supportSpells = new List<SkillSO>();

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

    private void FixedUpdate()
    {
        currentMana += statTable.GetStat(StatType.ManaRegen).value * Time.fixedDeltaTime;
        currentMana = Mathf.Clamp(currentMana, 0, statTable.GetStat(StatType.Mana).value);
    }

    public void Initialize()
    {
        foreach (Stat stat in initialStats)
        {
            baseStats.Add(stat.type, stat);
        }
        statTable = new StatTable(baseStats);

        currentMana = statTable.GetStat(StatType.Mana).value;
    }

    /// <summary>
    /// Recreates the StatTable from base stats, then applies all current skills' modifiers.
    /// </summary>
    public void SkillListChanged(List<SkillSO> skillSOs)
    {
        projectileDatas = new List<ProjectileData>();
        supportSpells = skillSOs.Where(x => x.isSupportSpell).ToList();
        var activeSpells = skillSOs.Where(x => !x.isSupportSpell).ToList();

        var statModifier = skillSOs.SelectMany(x => x.modifiers).ToList();

        foreach (SkillSO activeSpell in activeSpells)
        {
            var projectileData = new ProjectileData(activeSpell, statModifier);
            projectileDatas.Add(projectileData);
        }

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
            SkillListChanged(new List<SkillSO>());
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
            SkillListChanged(new List<SkillSO>());
        }
    }

    public void Shoot(float distanceToEnemy)
    {
        if (projectileDatas == null || projectileDatas.Count == 0) return;
        var currentProjectileData = projectileDatas[currentProjectileIndex];

        bool isLastProjectile = currentProjectileIndex == 0;
        float cooldown = isLastProjectile ? statTable.GetStat(StatType.CooldownPeriod).value : statTable.GetStat(StatType.CastTime).value;
        bool cooldownPassed = attackTimer >= cooldown;
        bool hasMana = currentMana >= currentProjectileData.ManaCost;

        if (cooldownPassed && hasMana)
        {
            ShootProjectileAt(currentProjectileData);
            attackTimer = 0f;
            currentProjectileIndex = (currentProjectileIndex + 1) % projectileDatas.Count;
        }
    }

    private void ShootProjectileAt(ProjectileData projectileData)
    {
        if (projectileData == null || projectileData.prefab == null) return;
        var projectilePrefab = projectileData.prefab;

        // Instantiate the projectile at the wand's current position
        GameObject projObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            // Calculate direction from the wand to the enemy
            float scatterValue = Mathf.Clamp(projectileData.statTable.GetStat(StatType.Scatter).value, 0, 180);
            float scatterAngle = UnityEngine.Random.Range(-scatterValue, scatterValue);

            Quaternion myRotation = Quaternion.Euler(0f, 0f, scatterAngle);
            Vector3 direction = myRotation * transform.right;

            // Initialize the projectile with direction, damage, and speed
            projectile.Initialize(direction, projectileData);
            currentMana -= projectileData.ManaCost;
        }
    }
}