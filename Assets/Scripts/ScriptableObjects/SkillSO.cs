using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/Skill", fileName = "NewSkill")]
public class SkillSO : ScriptableObject
{
    [Header("Item Info")]
    [field: SerializeField] public int id { get; private set; } = 0;
    public string skillName;
    public Sprite itemIcon;
    public bool isStackable;
    public int stackSize;
    public GameObject projectilePrefab;
    public bool isSupportSpell => projectilePrefab == null;
    [TextArea]
    public string description;
    public List<Stat> initialStats;

    [Header("Stat Modifiers")]
    public List<StatModifier> modifiers;

    void OnValidate()
    {
        if (id <= 0)
        {
            Debug.LogWarning("Skill ID cannot be negative. Setting to 0.");
            id = 0;
        }
    }
}
