using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseSpellManager : MonoBehaviour
{

    public int numberOfSpells = 3;
    public GameObject spellButtonContainer;
    public GameObject spellButtonPrefab;
    List<Button> spellButtons = new List<Button>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // load all spells from scriptable objects folder
        var spells = Resources.LoadAll<SkillSO>("Spells");

        for (int i = 0; i < numberOfSpells; i++)
        {
            SkillSO randomSpell = spells[Random.Range(0, spells.Length)];
            var newButtonGO = Instantiate(spellButtonPrefab, spellButtonContainer.transform);
            var newButton = newButtonGO.GetComponent<Button>();
            var buttonText = newButtonGO.GetComponentInChildren<TMPro.TMP_Text>();
            buttonText.text = randomSpell.skillName;
            spellButtons.Add(newButton);
            newButton.onClick.AddListener(() => { LogSpellName(randomSpell); });
        }


    }

    void LogSpellName(SkillSO spell)
    {
        Debug.Log(spell.skillName);
        GlobalData.Instance.PlayerInventory.AddItems(new ItemStack(spell, 1));
        SceneManager.LoadScene("GameScene");
    }

}
