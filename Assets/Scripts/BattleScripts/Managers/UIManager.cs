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

    private Player _player;
    private List<Enemy> _enemiesList;

    public event Action OnFinishTurnButtonClicked;
    public event Action<Spell> OnSpellButtonClicked;

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
        _player = Utils.GetPlayer();
        _enemiesList = Utils.GetEnemies();
        AddAsObserverToAllCharacters();
        PlaceSpellButtonsWithOffset();
    }

    private void PlaceSpellButtonsWithOffset()
    {
        GameObject spellZone = mainBar.transform.Find("SpellZone").gameObject;

        int i = 50;
        foreach (Spell spell in _player.ListSpells)
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
        TextMeshProUGUI charName = charInfo.transform.Find("CharName").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI hpText = charInfo.transform.Find("HPText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI apText = charInfo.transform.Find("APText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI mpText = charInfo.transform.Find("MPText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI resPercText = charInfo.transform.Find("ResPercText").GetComponent<TextMeshProUGUI>();

        charName.text = character.gameObject.name;
        hpText.text = character.HealthPoints.ToString();
        apText.text = character.ActionPoints.ToString();
        mpText.text = character.MovementPoints.ToString();
        resPercText.text = (character.MovementPoints * 10).ToString() + "%";
    }

    public void AddAsObserverToAllCharacters()
    {
        _player.OnCharClicked += HandleCharClicked;
        foreach (Enemy enemy in _enemiesList)
        {
            enemy.OnCharClicked += HandleCharClicked;
        }
    }

    public void AddAsObserverUI(Action HandleFinishTurnButtonClicked, Action<Spell> HandleSpellButtonClicked)
    {
        OnFinishTurnButtonClicked += HandleFinishTurnButtonClicked;
        OnSpellButtonClicked += HandleSpellButtonClicked;
    }

    // EVENTS ---------------------------------

    private void HandleCharClicked(Character character)
    {
        if (StateMachine.Instance.CurrectState != State.PlayerSelectingSpell) UpdateCharInfoText(character);
    }

    // BUTTON LISTENERS -------------------------

    public void OnFinishTurnButtonClickedListener()
    {
        OnFinishTurnButtonClicked?.Invoke();
    }

    public void OnSpellButtonClickedListener(Spell spell)
    {
        OnSpellButtonClicked?.Invoke(spell);
    }

}

