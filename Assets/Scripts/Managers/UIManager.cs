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
    [SerializeField] private GameObject spellPanel;
    [SerializeField] private SpellButton spellButtonPrefab;

    [Header("Other")]
    [SerializeField] protected Player player;

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
        PlaceSpellButtonsWithOffset();
    }

    public void OnFinishTurnClicked()
    {
        StateMachine.Instance.ProcessEvent(Event.FinishPlayerTurn);
    }

    private void PlaceSpellButtonsWithOffset()
    {
        int i = 50;
        foreach (Spell spell in player.ListSpells)
        {
            SpellButton spellButton = Instantiate(spellButtonPrefab, spellPanel.transform);
            spellButton.name = spellButtonPrefab.name;
            float pos = spellButton.transform.localPosition.x;
            spellButton.transform.localPosition = new Vector2(pos + i, 0);
            spellButton.Spell = spell;
            spellButton.transform.Find("SpellArtwork").GetComponent<Image>().sprite = spell.artwork;
            i += 100;
        }
    }

}

