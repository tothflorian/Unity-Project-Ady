using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Linker : MonoBehaviour
{
    public void SelectGame()
    {
        MenuController.Instance.SelectGame();
    }
    public void StartGame()
    {
        MenuController.Instance.StartGame();
    }

    public void OptionsMenu(string sceneName)
    {
        MenuController.Instance.OptionsMenu(sceneName);
    }

    public void Back()
    {
        MenuController.Instance.Back();
    }

    public void QuitGame()
    {
        MenuController.Instance.QuitGame();
    }

    public void MainMenu()
    {
        MenuController.Instance.MainMenu();
    }

    public void SetVolume()
    {
        SoundController.Instance.SetVolume(GameObject.Find("Slider").transform.GetComponent<Slider>().value,
                                           GameObject.Find("Toggle").GetComponent<Toggle>().isOn);
    }
    public void ClickSound()
    {
        SoundController.Instance.ClickSound();
    }
    public void SetMusic()
    {
        SoundController.Instance.SetMusic(GameObject.Find("Toggle").GetComponent<Toggle>().isOn);
        Debug.Log("setmusic");
    }
}
