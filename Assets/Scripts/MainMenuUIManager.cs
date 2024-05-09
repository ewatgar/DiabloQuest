using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject newGameWarning;

    private bool _clickedNewGameTwice;

    private void Start()
    {
        newGameButton.GetComponent<Button>().onClick.AddListener(() => OnNewGameButtonListener());
        continueButton.GetComponent<Button>().onClick.AddListener(() => OnContinueButtonListener());
        quitButton.GetComponent<Button>().onClick.AddListener(() => OnQuitButtonListener());
        _clickedNewGameTwice = false;
        ShowNewGameWarning(false);
    }

    private void OnNewGameButtonListener()
    {
        if (!_clickedNewGameTwice)
        {
            ShowNewGameWarning(true);
            print("Warning delete old save file");
            _clickedNewGameTwice = true;
        }
        else
        {
            SceneManager.LoadScene("LevelSelectionScene");
            print($"Savefile in path {SaveManager.SaveNewGame()}");
        }
    }

    private void OnContinueButtonListener()
    {
        throw new NotImplementedException();
    }

    private void OnQuitButtonListener()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ShowNewGameWarning(bool value)
    {
        newGameWarning.SetActive(value);
    }
}
