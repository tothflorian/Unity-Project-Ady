using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControllerLinker : MonoBehaviour
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

    public void SetVolume(float newVolume)
    {
        MenuController.Instance.SetVolume(newVolume);
    }
}
