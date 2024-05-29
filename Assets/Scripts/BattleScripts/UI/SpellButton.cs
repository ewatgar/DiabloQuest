using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class SpellButton : MonoBehaviour
{
    [SerializeField] private Spell _spell;

    public Spell Spell { get => _spell; set => _spell = value; }

    private void Start()
    {
        BattleUIManager ui = BattleUIManager.Instance;
        Button bt = GetComponent<Button>();
        bt.onClick.AddListener(() => ui.OnSpellButtonClickedListener(_spell));
    }
}
