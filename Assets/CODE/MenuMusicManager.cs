using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusicManager : MonoBehaviour
{
    private static MenuMusicManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("No AudioSource found on MenuMusicManager!");
            }
            else
            {
                audioSource.loop = true;
                audioSource.Play();
                Debug.Log("Menu music started!");
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (audioSource == null) return;

        if (scene.name == "level1" || scene.name == "level2")
        {
            audioSource.Stop();
            Debug.Log("Stopped menu music in gameplay scene.");
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                Debug.Log("Resumed menu music in menu scene.");
            }
        }
    }

    public void MuteMusic()
    {
        if (audioSource == null) return;
        audioSource.mute = true;
        PlayerPrefs.SetInt("MenuMusicMuted", 1);
        PlayerPrefs.Save();
        Debug.Log("Music muted!");
    }

    public void UnmuteMusic()
    {
        if (audioSource == null) return;
        audioSource.mute = false;
        PlayerPrefs.SetInt("MenuMusicMuted", 0);
        PlayerPrefs.Save();
        Debug.Log("Music unmuted!");
    }


    void Start()
    {
        if (audioSource != null && PlayerPrefs.HasKey("MenuMusicMuted"))
        {
            audioSource.mute = PlayerPrefs.GetInt("MenuMusicMuted") == 1;
        }
    }
}
