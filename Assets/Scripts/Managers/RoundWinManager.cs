using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundWinManager : MonoBehaviour
{
    public void OnMaxHPButtonClicked()
    {
        GlobalData.Instance.TowerData.AddMaxHP(10);
        // Load the gameplay scene again
        SceneManager.LoadScene("GameScene");
    }
    public void OnGoldCoinsButtonClicked()
    {
        CurrencyManager.Instance.AddGold(30);
        // Load the gameplay scene again
        SceneManager.LoadScene("GameScene");
    }
    public void OnSpellsButtonClicked()
    {
        // Load the gameplay scene again
        SceneManager.LoadScene("ChooseSpellScene");
    }
}
