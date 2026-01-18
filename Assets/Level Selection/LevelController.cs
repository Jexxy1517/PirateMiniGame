using UnityEngine;
using UnityEngine.SceneManagement; 
public class LevelController : MonoBehaviour
{
    public void GoToMainMenu()
    {
        Debug.Log("Kembali ke Main Menu!");
        SceneManager.LoadScene("MainMenu"); 
    }
    public void StartLevel1()
    {
        Debug.Log("Memulai Level 1!");
        SceneManager.LoadScene("HowToPlay1"); 
    }
    public void StartLevel2()
    {
        Debug.Log("Memulai Level 2!");
        SceneManager.LoadScene("HowToPlay2");
    }

    public void StartLevel3()
    {
        Debug.Log("Memulai Level 3!");
        SceneManager.LoadScene("Level3"); 
    }
}