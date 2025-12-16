using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance { get; private set; }

    private string optionsMenuOpenedFrom;
    private Animator fade;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        fade = GameObject.Find("FadePanel").GetComponent<Animator>();
        fade.SetTrigger("Start");
    }

    public void SelectGame()
    {
        StartCoroutine(FadeAndLoad("GameSelector"));
    }

    public void StartGame()
    {
        StartCoroutine(FadeAndLoad("GameScene"));
    }

    public void OptionsMenu(string sceneName)
    {
        optionsMenuOpenedFrom = sceneName;
        StartCoroutine(FadeAndLoad("OptionsMenu"));
    }

    public void Back()
    {
        StartCoroutine(FadeAndLoad(optionsMenuOpenedFrom));
    }

    public void MainMenu()
    {
        StartCoroutine(FadeAndLoad("MainMenu"));
    }

    public void QuitGame()
    {
        fade.SetTrigger("End");

        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        fade.SetTrigger("End");

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(sceneName);

        yield return null;

        fade = GameObject.Find("FadePanel").GetComponent<Animator>();

<<<<<<< Updated upstream
        fade.SetTrigger("Start");
    }

    public void SetVolume(float newVolume)
    {
        GetComponent<AudioSource>().volume = newVolume;
    }

=======
        Debug.Log(sceneName + " " + fade);
        fade.SetTrigger("Start");
    }

>>>>>>> Stashed changes
}
