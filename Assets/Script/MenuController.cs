using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance { get; private set; }

    private string optionsMenuOpenedFrom;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SelectGame()
    {
        SceneManager.LoadScene("GameSelector");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OptionsMenu(string sceneName)
    {
        SceneManager.LoadScene("OptionsMenu");
        optionsMenuOpenedFrom = sceneName;
    }

    public void Back()
    {
        SceneManager.LoadScene(optionsMenuOpenedFrom);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
