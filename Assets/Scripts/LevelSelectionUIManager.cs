using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject levelOneButton;
    [SerializeField] private GameObject levelTwoButton;
    [SerializeField] private GameObject levelThreeButton;
    [SerializeField] private GameObject blockedLevelError;
    [SerializeField] private GameObject quitButton;
    private GameData gameData;


    private void Start()
    {
        SoundManager.Instance.PlayLevelSelectionMusic();
        blockedLevelError.SetActive(false);
        levelOneButton.GetComponent<Button>().onClick.AddListener(() => OnLevelSelectionButtonListener(1));
        levelTwoButton.GetComponent<Button>().onClick.AddListener(() => OnLevelSelectionButtonListener(2));
        levelThreeButton.GetComponent<Button>().onClick.AddListener(() => OnLevelSelectionButtonListener(3));
        quitButton.GetComponent<Button>().onClick.AddListener(() => OnQuitButtonListener());
        gameData = SaveManager.Load();
    }
    private void OnLevelSelectionButtonListener(int level)
    {
        SoundManager.Instance.PlayUIButtonSFX();
        TextMeshProUGUI errorText = blockedLevelError.GetComponent<TextMeshProUGUI>();
        switch (level)
        {
            case 1:
                if (gameData.numCompletedLevels > 0)
                {
                    errorText.text = "Nivel 1 ya se ha completado";
                    blockedLevelError.SetActive(true);
                }
                else SceneManager.LoadScene("BattleOneScene");
                break;
            case 2:
                if (gameData.numCompletedLevels < 1)
                {
                    errorText.text = "Nivel 2 bloqueado, debes terminar el nivel 1";
                    blockedLevelError.SetActive(true);
                }
                else if (gameData.numCompletedLevels > 1)
                {
                    errorText.text = "Nivel 2 ya se ha completado";
                    blockedLevelError.SetActive(true);
                }
                else SceneManager.LoadScene("BattleTwoScene");
                break;
            case 3:
                if (gameData.numCompletedLevels < 2)
                {
                    errorText.text = "Nivel 3 bloqueado, debes terminar el nivel 2";
                    blockedLevelError.SetActive(true);
                }
                else if (gameData.numCompletedLevels > 2)
                {
                    errorText.text = "Nivel 3 ya se ha completado";
                    blockedLevelError.SetActive(true);
                }
                else SceneManager.LoadScene("BattleThreeScene");
                break;
        }
    }

    private void OnQuitButtonListener()
    {
        SoundManager.Instance.PlayUIButtonSFX();
        SceneManager.LoadScene("MainMenu");
    }
}
