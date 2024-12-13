using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void OnRestartButtonClicked()
    {
        // Load the gameplay scene again
        SceneManager.LoadScene("GameScene");
    }

    public void OnMainMenuButtonClicked()
    {
        // Return to Main Menu
        SceneManager.LoadScene("MainMenuScene");
    }
}
