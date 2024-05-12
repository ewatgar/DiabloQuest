using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    private static BattleUIManager _instance;
    public static BattleUIManager Instance { get => _instance; }

    [Header("UI Elements")]
    [SerializeField] private Canvas canvas;
    //[SerializeField] private GameObject spellZone;
    [SerializeField] private SpellButton spellButtonPrefab;
    //[SerializeField] private GameObject otherButtonsZone;
    [SerializeField] private GameObject mainBar;
    [SerializeField] private GameObject charInfo;
    [SerializeField] private GameObject battleInfo;
    [SerializeField] private GameObject spellInfo;
    [SerializeField] private GameObject charPointsSelection;

    private Player _player;
    private List<Enemy> _enemiesList;

    private Character _selectedChar;
    private Spell _selectedSpell;
    private bool _enableSpellInfoUi;
    private int _spareCharPoints = 3;

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
        _selectedChar = _player;
        _selectedSpell = _player.ListSpells.First();
        AddOtherButtonsListeners();
        ShowSpellInfoUI(false);
        ShowCharPointsSelectionUI(false);
    }


    private void Update()
    {
        //TODO update UI only when necesary (when AP, MP o HP changes)
        UpdateMainBarHealth();
        UpdateCharInfoText(_selectedChar);
        UpdateBattleInfoText();
        UpdateSpellInfoText(_selectedSpell);
    }

    private void ShowSpellInfoUI(bool value)
    {
        spellInfo.SetActive(value);
    }

    public void ShowCharPointsSelectionUI(bool value)
    {
        charPointsSelection.SetActive(value);
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

    private void UpdateMainBarHealth()
    {
        TextMeshProUGUI hpText = mainBar.transform.Find("HealthIcon").Find("HealthText").GetComponent<TextMeshProUGUI>();
        hpText.text = _player.Health.ToString();
    }

    private void UpdateCharInfoText(Character character)
    {
        TextMeshProUGUI charName = charInfo.transform.Find("CharName").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI healthText = charInfo.transform.Find("HealthText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI apText = charInfo.transform.Find("APText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI mpText = charInfo.transform.Find("MPText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI resPercText = charInfo.transform.Find("ResPercText").GetComponent<TextMeshProUGUI>();

        charName.text = character.gameObject.name;
        healthText.text = character.Health.ToString();
        apText.text = character.ActionPoints.ToString();
        mpText.text = character.MovementPoints.ToString();
        resPercText.text = (character.ResistancePerc * 10).ToString() + "%";
    }

    private void UpdateBattleInfoText()
    {
        TextMeshProUGUI roundText = battleInfo.transform.Find("RoundText").GetComponent<TextMeshProUGUI>();
        roundText.text = StateMachine.Instance.NumRounds.ToString();
    }

    private void UpdateSpellInfoText(Spell spell)
    {
        TextMeshProUGUI spellName = spellInfo.transform.Find("SpellName").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI utilityText = spellInfo.transform.Find("UtilityText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI apText = spellInfo.transform.Find("APText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI damageText = spellInfo.transform.Find("DamageText").GetComponent<TextMeshProUGUI>();

        spellName.text = spell.name;
        String utilityStr = "";
        switch (spell.utilityType)
        {
            case UtilityType.Damage:
                utilityStr = "Daños";
                break;
            case UtilityType.Healing:
                utilityStr = "Curas";
                break;
            case UtilityType.Knockback:
                utilityStr = "Empuje";
                break;
        }
        utilityText.text = utilityStr;
        apText.text = spell.actionPointCost.ToString();
        damageText.text = spell.baseDamageOrHealing.ToString();
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

    private void AddOtherButtonsListeners()
    {
        GameObject otherButtonsZone = mainBar.transform.Find("OtherButtonsZone").gameObject;
        Button finishTurnButton = otherButtonsZone.transform.Find("FinishTurnButton").gameObject.GetComponent<Button>();
        finishTurnButton.onClick.AddListener(() => OnFinishTurnButtonClickedListener());

        //TODO add charPointsSelection listeners
    }

    // EVENTS ---------------------------------

    private void HandleCharClicked(Character character)
    {
        if (StateMachine.Instance.CurrectState != State.PlayerSelectingSpell) _selectedChar = character;//UpdateCharInfoText(character);
    }

    // BUTTON LISTENERS -------------------------

    public void OnFinishTurnButtonClickedListener()
    {
        OnFinishTurnButtonClicked?.Invoke();
    }

    public void OnSpellButtonClickedListener(Spell spell)
    {
        if (_selectedSpell == spell) _enableSpellInfoUi = !_enableSpellInfoUi;
        else _enableSpellInfoUi = true;
        ShowSpellInfoUI(_enableSpellInfoUi);
        _selectedSpell = spell;
        OnSpellButtonClicked?.Invoke(spell);
    }

    //private void On

}

