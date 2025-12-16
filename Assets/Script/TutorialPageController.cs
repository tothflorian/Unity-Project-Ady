using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButtonController : MonoBehaviour
{

    private GameObject dots;
    private GameObject previousButton, nextButton, finishButton;
    private int currentPage = 0;

    void Start()
    {
        dots = GameObject.Find("Dots");
        previousButton = GameObject.Find("PreviousButton");
        nextButton = GameObject.Find("NextButton");
        finishButton = GameObject.Find("FinishButton");

        CurrentPage = 0;
        previousButton.SetActive(false);
        finishButton.SetActive(false);
    }
    private int CurrentPage
    {
        get => currentPage;
        set
        {

            transform.GetChild(currentPage).gameObject.SetActive(false);
            dots.transform.GetChild(currentPage).gameObject.GetComponent<Image>().color = Color.gray;
            currentPage = value;
            transform.GetChild(currentPage).gameObject.SetActive(true);
            dots.transform.GetChild(currentPage).gameObject.GetComponent<Image>().color = Color.white;
        }
    }

    public void NextButtonClick()
    {
        if (CurrentPage + 1 < 5)
        {
            CurrentPage++;
            previousButton.SetActive(true);
        }
        if (CurrentPage == 4)
        {
            nextButton.SetActive(false);
            finishButton.SetActive(true);
        }
            
    }
    public void PreviousButtonClick()
    {
        if (CurrentPage - 1 > -1)
        {
            CurrentPage--;
            nextButton.SetActive(true);
            finishButton.SetActive(false);
        }
        if (CurrentPage == 0)
            previousButton.SetActive(false);
    }
}
