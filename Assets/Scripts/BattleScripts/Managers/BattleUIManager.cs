using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private GameObject winMatchText;
    [SerializeField] private GameObject charPointsSelection;
    [SerializeField] private GameObject acceptErrorText;
    [SerializeField] private GameObject loseMatchText;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject yesFleeButton;
    [SerializeField] private GameObject noFleeButton;
    [SerializeField] private int levelNumber;

    private Player _player;
    private List<Enemy> _enemiesList;

    private Character _selectedChar;
    private Spell _selectedSpell;
    private bool _enableSpellInfoUi;
    private bool _enableItemButtonsUi = false;
    private List<SpellButton> _spellButtons = new List<SpellButton>();

    // Characteristic points selection
    private int _spareCharPoints;
    private int _healthPointsGD; //gameData
    private int _actionPointsGD;
    private int _movementPointsGD;
    private int _damagePointsGD;
    private int _resistancePercGD;
    private int _critsPercGD;

    public event Action OnFinishTurnButtonClicked;
    public event Action<Spell> OnSpellButtonClicked;
    public event Action<List<int>> OnAcceptCharPointsButtonClicked;

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
        InitGameDataValues();
        AddAsObserverToAllCharacters();
        PlaceSpellButtonsWithOffset();
        _selectedChar = _player;
        _selectedSpell = _player.ListSpells.First();
        AddOtherButtonsListeners();
        ShowSpellInfoUI(false);
        AddCharPointsButtonsListeners();
        HideEndMatchUI();
    }

    private void Update()
    {
        //TODO update UI only when necesary (when AP, MP o HP changes)
        UpdateMainBarHealth();
        UpdateCharInfoText(_selectedChar);
        UpdateBattleInfoText();
        UpdateSpellInfoText(_selectedSpell);
        UpdateTurnButtonsInteractable();
    }

    private void InitGameDataValues()
    {
        _spareCharPoints = 3;
        _healthPointsGD = _player.InitHealthPoints;
        _actionPointsGD = _player.InitActionPoints;
        _movementPointsGD = _player.InitMovementPoints;
        _damagePointsGD = _player.InitDamagePoints;
        _resistancePercGD = _player.InitResistancePerc;
        _critsPercGD = _player.InitCritsPerc;
    }

    private void ShowSpellInfoUI(bool value)
    {
        spellInfo.SetActive(value);
    }

    private void HideEndMatchUI()
    {
        charPointsSelection.SetActive(false);
        winMatchText.SetActive(false);
        acceptErrorText.SetActive(false);
        loseMatchText.SetActive(false);
        background.SetActive(false);
        yesFleeButton.SetActive(false);
        noFleeButton.SetActive(false);
    }

    public void ShowEndMatchUI(bool isWin)
    {
        mainBar.SetActive(false);
        charInfo.SetActive(false);
        battleInfo.SetActive(false);
        spellInfo.SetActive(false);
        if (isWin)
        {
            charPointsSelection.SetActive(true);
            winMatchText.SetActive(true);
            background.SetActive(true);
            UpdateCharPointsSelectionText();
        }
        else
        {
            loseMatchText.SetActive(true);
            background.SetActive(true);
        }
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
            InitSpellButton(spellButton, spell);
            _spellButtons.Add(spellButton);
            i += 100;
        }
    }

    private void InitSpellButton(SpellButton spellButton, Spell spell)
    {
        spellButton.Spell = spell;
        spellButton.GetComponent<Button>().interactable = true;
        Transform artwork = spellButton.transform.Find("SpellArtwork");
        artwork.GetComponent<Image>().sprite = spell.artwork;
        artwork.gameObject.SetActive(true);

    }

    public void ReplaceSpellWithItemButtons(bool value)
    {
        for (int i = 0; i < _player.ListItems.Count; i++)
        {
            SpellButton spellButton = _spellButtons[i];
            if (value)
            {
                InitSpellButton(spellButton, _player.ListItems[i]);

                HashSet<Item> listOfUsedItems = StateMachine.Instance.ListOfUsedItems;
                Transform artwork = spellButton.transform.Find("SpellArtwork");

                if (listOfUsedItems.Contains(_player.ListItems[i]))
                {
                    spellButton.GetComponent<Button>().interactable = false;
                    artwork.gameObject.SetActive(false);
                }
            }
            else InitSpellButton(spellButton, _player.ListSpells[i]);
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
        TextMeshProUGUI dmgText = charInfo.transform.Find("DmgText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI resPercText = charInfo.transform.Find("ResPercText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI critsPercText = charInfo.transform.Find("CritsPercText").GetComponent<TextMeshProUGUI>();

        charName.text = character.gameObject.name;
        healthText.text = character.Health.ToString();
        apText.text = character.ActionPoints.ToString();
        mpText.text = character.MovementPoints.ToString();
        resPercText.text = (character.ResistancePerc * 10).ToString() + "%";
        critsPercText.text = (character.CritsPerc * 10).ToString() + "%";
    }

    private void UpdateBattleInfoText()
    {
        TextMeshProUGUI roundText = battleInfo.transform.Find("RoundText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI levelText = battleInfo.transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
        roundText.text = StateMachine.Instance.NumRounds.ToString();
        levelText.text = levelNumber.ToString();
    }

    private void UpdateSpellInfoText(Spell spell)
    {
        TextMeshProUGUI spellName = spellInfo.transform.Find("SpellName").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI utilityText = spellInfo.transform.Find("UtilityText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI apText = spellInfo.transform.Find("APText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI damageText = spellInfo.transform.Find("DamageText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI descr = spellInfo.transform.Find("Descr").GetComponent<TextMeshProUGUI>();

        spellName.text = spell.name;
        string utilityStr = "";
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
            case UtilityType.MovementPoints:
                utilityStr = "Movim.";
                break;
            case UtilityType.ActionPoints:
                utilityStr = "Acción";
                break;
            case UtilityType.Crits:
                utilityStr = "Críticos";
                break;
        }
        utilityText.text = utilityStr;
        apText.text = spell.actionPointCost.ToString();
        damageText.text = spell.value.ToString();
        if (spell.utilityType == UtilityType.Crits) damageText.text += " %";
        descr.text = spell.description;
    }

    private void UpdateCharPointsSelectionText()
    {
        TextMeshProUGUI title = charPointsSelection.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI hpText = charPointsSelection.transform.Find("HPText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI apText = charPointsSelection.transform.Find("APText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI mpText = charPointsSelection.transform.Find("MPText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI dpText = charPointsSelection.transform.Find("DPText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI resPercText = charPointsSelection.transform.Find("ResPercText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI critsPercText = charPointsSelection.transform.Find("CritsPercText").GetComponent<TextMeshProUGUI>();

        title.text = _spareCharPoints == 1 ? "Queda 1 punto" : $"Quedan {_spareCharPoints} puntos";
        hpText.text = _healthPointsGD.ToString();
        apText.text = _actionPointsGD.ToString();
        mpText.text = _movementPointsGD.ToString();
        dpText.text = _damagePointsGD.ToString();
        resPercText.text = _resistancePercGD.ToString();
        critsPercText.text = _critsPercGD.ToString();
    }

    private void UpdateTurnButtonsInteractable()
    {
        GameObject otherButtonsZone = mainBar.transform.Find("OtherButtonsZone").gameObject;
        Button finishTurnButton = otherButtonsZone.transform.Find("FinishTurnButton").gameObject.GetComponent<Button>();
        Transform finishTurnButtonIcon = finishTurnButton.transform.Find("ButtonIcon");
        if (StateMachine.Instance.CurrectState == State.PlayerTurn)
        {
            finishTurnButton.interactable = true;
            finishTurnButtonIcon.gameObject.SetActive(true);
        }
        else
        {
            finishTurnButton.interactable = false;
            finishTurnButtonIcon.gameObject.SetActive(false);
        }
    }

    // ADD OBSERVERS

    public void AddAsObserverToAllCharacters()
    {
        _player.OnCharClicked += HandleCharClicked;
        foreach (Enemy enemy in _enemiesList)
        {
            enemy.OnCharClicked += HandleCharClicked;
        }
    }

    public void AddAsObserverUI(Action HandleFinishTurnButtonClicked, Action<Spell> HandleSpellButtonClicked, Action<List<int>> HandleAcceptCharPointsButtonClicked)
    {
        OnFinishTurnButtonClicked += HandleFinishTurnButtonClicked;
        OnSpellButtonClicked += HandleSpellButtonClicked;
        OnAcceptCharPointsButtonClicked += HandleAcceptCharPointsButtonClicked;
    }

    // ADD LISTENERS

    private void AddOtherButtonsListeners()
    {
        GameObject otherButtonsZone = mainBar.transform.Find("OtherButtonsZone").gameObject;
        Button finishTurnButton = otherButtonsZone.transform.Find("FinishTurnButton").gameObject.GetComponent<Button>();
        Button fleeButton = otherButtonsZone.transform.Find("FleeButton").gameObject.GetComponent<Button>();
        Button itemListButton = otherButtonsZone.transform.Find("ItemListButton").gameObject.GetComponent<Button>();
        finishTurnButton.onClick.AddListener(() => OnFinishTurnButtonClickedListener());
        fleeButton.onClick.AddListener(() => OnFleeButtonClickedListener());
        itemListButton.onClick.AddListener(() => OnItemListButtonClickedListener());
    }

    private void AddCharPointsButtonsListeners()
    {
        foreach (Transform childGameObject in charPointsSelection.transform)
        {
            Button button = childGameObject.GetComponent<Button>();
            if (childGameObject.name == "AcceptButton") button.onClick.AddListener(() => OnAcceptCharPointsButtonClickedListener());
            else if (button != null) button.onClick.AddListener(() => OnPlusMinusCharPointsButtonClickedListener(childGameObject.gameObject));
        }

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

    public void OnFleeButtonClickedListener()
    {
        TextMeshProUGUI warningText = loseMatchText.GetComponent<TextMeshProUGUI>();
        warningText.text = "¿Estás seguro que quieres salir? se perderá el progreso";
        warningText.fontSize = 36;
        yesFleeButton.GetComponent<Button>().onClick.AddListener(() => OnFleeOptionsClickedListener(true));
        noFleeButton.GetComponent<Button>().onClick.AddListener(() => OnFleeOptionsClickedListener(false));
        background.SetActive(true);
        loseMatchText.SetActive(true);
        yesFleeButton.SetActive(true);
        noFleeButton.SetActive(true);
    }

    public void OnFleeOptionsClickedListener(bool value)
    {
        if (value) SceneManager.LoadScene("LevelSelectionScene");
        else HideEndMatchUI();
    }

    private void OnItemListButtonClickedListener()
    {
        _enableItemButtonsUi = !_enableItemButtonsUi;
        ReplaceSpellWithItemButtons(_enableItemButtonsUi);
    }

    public void OnSpellButtonClickedListener(Spell spell)
    {
        if (_selectedSpell == spell) _enableSpellInfoUi = !_enableSpellInfoUi;
        else _enableSpellInfoUi = true;
        ShowSpellInfoUI(_enableSpellInfoUi);
        _selectedSpell = spell;
        OnSpellButtonClicked?.Invoke(spell);
    }

    private void OnAcceptCharPointsButtonClickedListener()
    {
        if (_spareCharPoints == 0)
        {
            acceptErrorText.SetActive(false);
            List<int> listCharPointsGD = new List<int>
            {
                _healthPointsGD,
                _actionPointsGD,
                _movementPointsGD,
                _damagePointsGD,
                _resistancePercGD,
                _critsPercGD
            };
            OnAcceptCharPointsButtonClicked?.Invoke(listCharPointsGD);
        }
        acceptErrorText.SetActive(true);
    }

    private void OnPlusMinusCharPointsButtonClickedListener(GameObject buttonGameObject)
    {
        print(buttonGameObject.name);
        string regex = @"^(HP|AP|DP|MP|ResPerc|CritsPerc)(Plus|Minus)Button$";
        Match match = Regex.Match(buttonGameObject.name, regex);

        if (!match.Success)
        {
            print("Button name doesn't match pattern");
            return;
        }

        string typeData = match.Groups[1].Value;
        bool addPoint = match.Groups[2].Value == "Plus";

        switch (typeData)
        {
            case "HP":
                if (addPoint && _spareCharPoints > 0) _healthPointsGD++;
                else if (!addPoint && _spareCharPoints < 3 && _healthPointsGD > _player.InitHealthPoints) _healthPointsGD--;
                break;
            case "AP":
                if (addPoint && _spareCharPoints > 0) _actionPointsGD++;
                else if (!addPoint && _spareCharPoints < 3 && _actionPointsGD > _player.InitActionPoints) _actionPointsGD--;
                break;
            case "DP":
                if (addPoint && _spareCharPoints > 0) _damagePointsGD++;
                else if (!addPoint && _spareCharPoints < 3 && _damagePointsGD > _player.InitDamagePoints) _damagePointsGD--;
                break;
            case "MP":
                if (addPoint && _spareCharPoints > 0) _movementPointsGD++;
                else if (!addPoint && _spareCharPoints < 3 && _movementPointsGD > _player.InitMovementPoints) _movementPointsGD--;
                break;
            case "ResPerc":
                if (addPoint && _spareCharPoints > 0) _resistancePercGD++;
                else if (!addPoint && _spareCharPoints < 3 && _resistancePercGD > _player.InitResistancePerc) _resistancePercGD--;
                break;
            case "CritsPerc":
                if (addPoint && _spareCharPoints > 0) _critsPercGD++;
                else if (!addPoint && _spareCharPoints < 3 && _critsPercGD > _player.InitCritsPerc) _critsPercGD--;
                break;
        }
        if (addPoint && _spareCharPoints > 0) _spareCharPoints--;
        else if (!addPoint && _spareCharPoints < 3) _spareCharPoints++;

        UpdateCharPointsSelectionText();
        //print($"Spare Points: {_spareCharPoints}, HP: {_healthPointsGD}, AP: {_actionPointsGD}, MP: {_movementPointsGD}, DP: {_damagePointsGD}, ResPerc: {_resistancePercGD}, CritsPerc: {_critsPercGD}");
    }
}

