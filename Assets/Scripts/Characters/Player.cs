using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [Header("Player Spells")]
    [SerializeField] private List<Spell> _listSpells;

    public List<Spell> ListSpells { get => _listSpells; }
}
