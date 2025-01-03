using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/Skill", fileName = "NewSkill")]
public class SkillSO : ScriptableObject
{
    [Header("Item Info")]
    public string skillName;
    public Sprite itemIcon;
    public bool isStackable;
    public int stackSize;
    [TextArea]
    public string description;

    [Header("Stat Modifiers")]
    public List<StatModifier> modifiers;
}
