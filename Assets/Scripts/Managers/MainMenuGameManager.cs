using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuGameManager : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
