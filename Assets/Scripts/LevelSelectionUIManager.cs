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
        blockedLevelError.SetActive(false);
        levelOneButton.GetComponent<Button>().onClick.AddListener(() => OnLevelSelectionButtonListener(1));
        levelTwoButton.GetComponent<Button>().onClick.AddListener(() => OnLevelSelectionButtonListener(2));
        levelThreeButton.GetComponent<Button>().onClick.AddListener(() => OnLevelSelectionButtonListener(3));
        quitButton.GetComponent<Button>().onClick.AddListener(() => OnQuitButtonListener());
        gameData = SaveManager.Load();
    }
    private void OnLevelSelectionButtonListener(int level)
    {
        switch (level)
        {
            case 1:
                SceneManager.LoadScene("BattleOneScene");
                break;
            case 2:
                if (gameData.numCompletedLevels >= 1) SceneManager.LoadScene("BattleTwoScene");
                else
                {
                    TextMeshProUGUI errorText = blockedLevelError.GetComponent<TextMeshProUGUI>();
                    errorText.text = "Nivel 2 bloqueado";
                    blockedLevelError.SetActive(true);
                    print("You need to finish level 1");
                }
                break;
            case 3:
                if (gameData.numCompletedLevels >= 2) SceneManager.LoadScene("BattleThreeScene");
                else
                {
                    TextMeshProUGUI errorText = blockedLevelError.GetComponent<TextMeshProUGUI>();
                    errorText.text = "Nivel 3 bloqueado";
                    blockedLevelError.SetActive(true);
                    print("You need to finish level 2");
                }
                break;
        }
    }

    private void OnQuitButtonListener()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
