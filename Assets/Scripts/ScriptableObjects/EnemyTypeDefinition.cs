using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/EnemyType", fileName = "NewEnemyType")]
public class EnemyTypeDefinition : ScriptableObject
{
    [Header("General")]
    public string enemyName;          // e.g., "Grunt", "Runner", "Tank"
    public GameObject enemyPrefab;    // Prefab that will be instantiated

    [Header("Base Stats")]
    public float baseHP = 10f;        // HP at waveStart
    public float baseSpeed = 2f;

    [Header("Growth Per Wave")]
    public float hpGrowthPerWave = 2f;
    public float speedGrowthPerWave = 0.2f;

    [Header("Stat Caps (Optional)")]
    public float maxHP = 200f;        // HP won't exceed this
    public float maxSpeed = 5f;

    [Header("Wave Range")]
    public int waveStart = 1;         // Minimum wave to appear
    // (Optionally add waveEnd if you want them to stop spawning after some wave)
}
