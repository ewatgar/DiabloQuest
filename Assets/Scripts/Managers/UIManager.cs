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
    [SerializeField] private float spellButtonsGap = 5;

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
        PlaceSpellButtonsWithOffset(spellPanel, spellButtonsGap);
    }

    public void OnFinishTurnClicked()
    {
        StateMachine.Instance.ProcessEvent(Event.FinishPlayerTurn);
    }

    private void PlaceSpellButtonsWithOffset(GameObject spellPanel, float gap)
    {
        Vector3 spellButtonSize = spellButtonPrefab.GetComponentInChildren<RectTransform>().sizeDelta;

        float spellButtonWidth = spellButtonSize.x;
        float gapX = gap;
        float nCols = player.ListSpells.Count;
        float colWidth = spellButtonWidth * nCols + gapX * (nCols - 1);
        float x0 = -(colWidth - spellButtonWidth) / 2;
        float offsetX = spellButtonWidth + gapX;

        int i = 0;
        foreach (Spell spell in player.ListSpells)
        {
            float coordsX = x0 + offsetX * i;
            SpellButton spellButton = Instantiate(spellButtonPrefab, spellPanel.transform);
            spellButton.name = spellButtonPrefab.name + i;
            spellButton.transform.localPosition = new Vector2(coordsX, 0);
            spellButton.Spell = spell;
            i++;
        }
    }



}

