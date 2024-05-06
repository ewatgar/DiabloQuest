using System.Collections;
using System.Collections.Generic;
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


    private void Start()
    {
        levelOneButton.GetComponent<Button>().onClick.AddListener(() => OnLevelSelectionButtonListener(1));
        levelTwoButton.GetComponent<Button>().onClick.AddListener(() => OnLevelSelectionButtonListener(2));
        levelThreeButton.GetComponent<Button>().onClick.AddListener(() => OnLevelSelectionButtonListener(3));
    }

    private void OnLevelSelectionButtonListener(int level)
    {
        switch (level)
        {
            case 1:
                SceneManager.LoadScene("BattleOneScene");
                break;
            case 2:
                print("Level 2 not implemented");
                break;
            case 3:
                print("Level 3 not implemented");
                break;
        }

    }
}
