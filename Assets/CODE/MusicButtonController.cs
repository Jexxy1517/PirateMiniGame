using UnityEngine;
using UnityEngine.UI;

public class MusicButtonController : MonoBehaviour
{
    public Button muteButton;
    public Button unmuteButton;

    void Start()
    {
        MenuMusicManager musicManager = FindObjectOfType<MenuMusicManager>();

        if (musicManager == null)
        {
            Debug.LogError("MenuMusicManager not found in scene!");
            return;
        }

        muteButton.onClick.AddListener(musicManager.MuteMusic);
        unmuteButton.onClick.AddListener(musicManager.UnmuteMusic);
    }
}
