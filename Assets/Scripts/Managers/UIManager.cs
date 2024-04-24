using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get => _instance; }

    [Header("UI Elements")]
    [SerializeField] private Canvas canvas;
    //[SerializeField] private GameObject spellZone;
    [SerializeField] private SpellButton spellButtonPrefab;
    //[SerializeField] private GameObject otherButtonsZone;
    [SerializeField] private GameObject mainBar;
    [SerializeField] private GameObject charInfo;

    private Player player;
    private List<Enemy> enemiesList;

    //private bool _canUseMainButtons;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = Utils.GetPlayer();
        enemiesList = Utils.GetEnemies();
        AddAsObserverToAllCharacters();
        PlaceSpellButtonsWithOffset();
    }

    public void OnFinishTurnClicked()
    {
        StateMachine.Instance.ProcessEvent(Event.FinishPlayerTurn);
    }

    private void PlaceSpellButtonsWithOffset()
    {
        GameObject spellZone = mainBar.transform.Find("SpellZone").gameObject;

        int i = 50;
        foreach (Spell spell in player.ListSpells)
        {
            SpellButton spellButton = Instantiate(spellButtonPrefab, spellZone.transform);
            spellButton.name = spellButtonPrefab.name;
            float pos = spellButton.transform.localPosition.x;
            spellButton.transform.localPosition = new Vector2(pos + i, 0);
            spellButton.Spell = spell;
            spellButton.transform.Find("SpellArtwork").GetComponent<Image>().sprite = spell.artwork;
            i += 100;
        }
    }

    private void UpdateCharInfoText(Character character)
    {
        //charInf.
        print("char clicked");
    }

    public void AddAsObserverToAllCharacters()
    {
        player.OnCharClicked += HandleCharClicked;
        foreach (Enemy enemy in enemiesList)
        {
            enemy.OnCharClicked += HandleCharClicked;
        }
    }

    // EVENTS ---------------------------------

    private void HandleCharClicked(Character character)
    {
        UpdateCharInfoText(character);
    }



}

