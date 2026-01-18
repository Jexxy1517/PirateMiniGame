using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("level2");
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("LevelMenu");
    }
}
