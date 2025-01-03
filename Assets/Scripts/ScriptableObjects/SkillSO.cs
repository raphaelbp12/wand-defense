using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/Skill", fileName = "NewSkill")]
public class SkillSO : ScriptableObject
{
    [Header("Display")]
    public string skillName;
    [TextArea]
    public string description;

    [Header("Stat Modifiers")]
    public List<StatModifier> modifiers;
}
