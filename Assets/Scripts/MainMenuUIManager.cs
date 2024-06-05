using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        SoundManager.Instance.PlayMainMenuMusic();
        newGameButton.GetComponent<Button>().onClick.AddListener(() => OnNewGameButtonListener());
        continueButton.GetComponent<Button>().onClick.AddListener(() => OnContinueButtonListener());
        quitButton.GetComponent<Button>().onClick.AddListener(() => OnQuitButtonListener());
        _clickedNewGameTwice = false;
        ShowNewGameWarning(false);
    }

    private void OnNewGameButtonListener()
    {
        SoundManager.Instance.PlayUIButtonSFX();
        if (!_clickedNewGameTwice)
        {
            ShowNewGameWarning(true);
            print("Warning delete old save file");
            _clickedNewGameTwice = true;
        }
        else
        {
            SceneManager.LoadScene("LevelSelectionScene");
            string path = SaveManager.SaveNewGame();
            print($"Savefile in path {path}");
        }
    }

    private void OnContinueButtonListener()
    {
        SoundManager.Instance.PlayUIButtonSFX();
        if (SaveManager.SaveExists()) SceneManager.LoadScene("LevelSelectionScene");
        else
        {
            TextMeshProUGUI errorText = newGameWarning.GetComponent<TextMeshProUGUI>();
            errorText.text = "Partida no encontrada. Debes crear una nueva partida";
            ShowNewGameWarning(true);
            _clickedNewGameTwice = true;
        }
    }

    private void OnQuitButtonListener()
    {
        SoundManager.Instance.PlayUIButtonSFX();
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
