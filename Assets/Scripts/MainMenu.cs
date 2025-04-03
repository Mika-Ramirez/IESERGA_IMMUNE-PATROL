using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;
    public GameObject settingsPanel;

    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("New Scene"); 
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
