using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;



enum StatType
{
    Area,
    Vampire,
    SplitCount,
    Scatter,
    Duration,
    Range,
    Interval,
    

}

enum ModifierOperator
{
    Sum,
    Mult
}

class Stat
{

    public Stat(StatType nType, float nValue)
    {
        type = nType;
        value = nValue;
    }

    public StatType type;
    public float value;
}

class StatModifier
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

class StatTable
{
    Dictionary<StatType, Stat> stats;

    public StatTable(Dictionary<StatType, Stat> baseStats)
    {
        this.Initialize();

        IDictionaryEnumerator myEnum =
                     baseStats.GetEnumerator();

        foreach (var kvp in baseStats) {
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

    void applyModifier(StatModifier mod)
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

    Stat GetStat(StatType type) {
        return stats[type];
    }

}


abstract class Skill {
    string name;
    List<StatModifier> modifiers;
}


class AreaIncrease : Skill {
    string name = "AreaIncrease";

    List<StatModifier> modifiers = new List<StatModifier> {
        new StatModifier(StatType.Area, ModifierOperator.Mult, 0.1f)
    };
        
}



public class Wand : MonoBehaviour
{
    [HideInInspector]
    public Transform towerTransform;
    public float attackInterval = 2f;
    public float attackRange = 10f;
    public int projectileDamage = 10;
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;

    private float attackTimer = 0f;

    private void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            // Attempt to find and attack the closest enemy
            Enemy closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                float distance = Vector3.Distance(transform.position, closestEnemy.transform.position);
                if (distance <= attackRange)
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
            projectile.Initialize(direction, projectileDamage, projectileSpeed);
        }
    }
}
