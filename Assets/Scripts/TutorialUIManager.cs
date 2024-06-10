using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject pageOne;
    [SerializeField] private GameObject pageTwo;
    [SerializeField] private Button firstPageNextButton;
    [SerializeField] private Button secondPagePreviousButton;
    [SerializeField] private Button secondPageFinishButton;

    void Start()
    {
        tutorialPanel.SetActive(true);
        pageOne.SetActive(true);
        pageTwo.SetActive(false);
        firstPageNextButton.onClick.AddListener(() => OnFirstPageNextButtonListener());
        secondPagePreviousButton.onClick.AddListener(() => OnSecondPagePreviousButtonListener());
        secondPageFinishButton.onClick.AddListener(() => OnSecondPageFinishButtonListener());
    }

    private void OnFirstPageNextButtonListener()
    {
        SoundManager.Instance.PlayUIButtonSFX();
        pageOne.SetActive(false);
        pageTwo.SetActive(true);
    }

    private void OnSecondPagePreviousButtonListener()
    {
        SoundManager.Instance.PlayUIButtonSFX();
        pageOne.SetActive(true);
        pageTwo.SetActive(false);
    }

    private void OnSecondPageFinishButtonListener()
    {
        SoundManager.Instance.PlayUIButtonSFX();
        tutorialPanel.SetActive(false);
    }
}
