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

public enum SkillEnum
{
    IncreaseArea,
    Vampire,
    Split,
    Scatter,
    IncreaseDuration,
    IncreaseRange,
    IncreaseAttackSpeed,
    IncreaseDamage,
    IncreaseProjectileSpeed,
    ReduceCooldown,
}

public abstract class Skill
{
    public string Name { get; protected set; }
    public SkillEnum SkillEnum { get; protected set; }
    public List<StatModifier> Modifiers { get; protected set; }

    protected Skill(string name, SkillEnum skillEnum, List<StatModifier> modifiers)
    {
        Name = name;
        SkillEnum = skillEnum;
        Modifiers = modifiers;
    }
}

public class IncreaseArea : Skill
{
    public IncreaseArea()
        : base(
            "Increase Area +10%",
            SkillEnum.IncreaseArea,
            new List<StatModifier>
            {
                new StatModifier(StatType.Area, ModifierOperator.Mult, 0.1f)
            }
        )
    {
    }
}

public class IncreaseDamage : Skill
{
    public IncreaseDamage()
        : base(
            "Increase Damage +50%",
            SkillEnum.IncreaseDamage,
            new List<StatModifier>
            {
                new StatModifier(StatType.Damage, ModifierOperator.Mult, 0.5f)
            }
        )
    {
    }
}

public class IncreaseProjectileSpeed : Skill
{
    public IncreaseProjectileSpeed()
        : base(
            "Increase Projectile Speed +50%",
            SkillEnum.IncreaseProjectileSpeed,
            new List<StatModifier>
            {
                new StatModifier(StatType.TravelSpeed, ModifierOperator.Mult, 0.5f)
            }
        )
    {
    }
}

public static class SkillFactory
{
    public static Skill CreateSkill(SkillEnum skillEnum)
    {
        switch (skillEnum)
        {
            case SkillEnum.IncreaseArea:
                return new IncreaseArea();
            case SkillEnum.IncreaseDamage:
                return new IncreaseDamage();
            case SkillEnum.IncreaseProjectileSpeed:
                return new IncreaseProjectileSpeed();
            // case SkillEnum.Vampire:
            //     return new VampireSkill(); // Example
            // case SkillEnum.Split:
            //     return new SplitSkill();   // Example
            // and so on...

            default:
                // If unrecognized, return null or throw
                return null;
        }
    }
}


public class Wand : MonoBehaviour
{
    [SerializeField]
    public List<SkillEnum> initialSkills = new List<SkillEnum>();
    private List<Skill> activeSkills = new List<Skill>();

    [HideInInspector]
    public Transform towerTransform;
    public GameObject projectilePrefab;

    private StatTable statTable;

    private float attackTimer = 0f;

    private void Start()
    {
        statTable = new StatTable(new Dictionary<StatType, Stat> {
            { StatType.Range, new Stat(StatType.Range, 5) },
            { StatType.Interval, new Stat(StatType.Interval, 0.1f) },
            { StatType.Damage, new Stat(StatType.Damage, 10) },
            { StatType.TravelSpeed, new Stat(StatType.TravelSpeed, 2) },
        });

        foreach (var skillEnum in initialSkills)
        {
            Skill newSkill = SkillFactory.CreateSkill(skillEnum);
            if (newSkill != null)
            {
                activeSkills.Add(newSkill);

                // For each modifier in the skill, apply to our stat table
                foreach (var modifier in newSkill.Modifiers)
                {
                    statTable.ApplyModifier(modifier);
                }
            }
        }
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

    private Enemy FindClosestEnemy()
    {
        // Using the new API:
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

            // Initialize the projectile with a direction, damage, and speed
            var damage = statTable.GetStat(StatType.Damage).value;
            projectile.Initialize(direction, damage, statTable.GetStat(StatType.TravelSpeed).value);
        }
    }
}
