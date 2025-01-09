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

    [SerializeField] public SkillSO increaseDamageLvl1;
    [SerializeField] public SkillSO increaseProjectileSpeedLvl1;

    #endregion Support

    #region Spell

    // [SerializeField] public SkillSO skull;

    #endregion Spell
}