using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFinish : MonoBehaviour
{
    Animator window;
    Animator background;

    void Awake()
    {
        background = GameObject.Find("TutorialBackground").GetComponent<Animator>();
        window = GameObject.Find("TutorialWindow").GetComponent<Animator>();
    }

    public void Finish()
    {
        background.SetTrigger("Start");
        window.SetTrigger("Start");
        GameManager.Instance.newGame = false;
        
        StartCoroutine(DisableAfterAnimation(0.5f));
    }
    IEnumerator DisableAfterAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);
        GameObject.Find("Tutorial").SetActive(false);
    }
}
