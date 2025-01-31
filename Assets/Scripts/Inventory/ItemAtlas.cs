using UnityEditor;
using UnityEngine;

public class ItemAtlas : MonoBehaviour
{
    public static ItemAtlas instance;

    ItemAtlas()
    {
        instance = this;
    }


    #region Support

    [SerializeField] public SkillSO ImproveCooldownLvl1;
    [SerializeField] public SkillSO IncreaseDamageLvl1;
    [SerializeField] public SkillSO IncreaseManaLvl1;
    [SerializeField] public SkillSO IncreaseManaRegenLvl1;
    [SerializeField] public SkillSO increaseProjectileSpeedLvl1;
    [SerializeField] public SkillSO IncreaseRangeLvl1;

    #endregion Support

    #region Spell

    // [SerializeField] public SkillSO skull;

    #endregion Spell
}