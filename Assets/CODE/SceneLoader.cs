using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadLevelOne()
    {
        SceneManager.LoadScene("level1");
    }

    public void LoadLevelTwo()
    {
        SceneManager.LoadScene("level2");
    }
}
