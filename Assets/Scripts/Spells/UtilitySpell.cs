using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UtilityType
{
    IncreaseRes,
    Knockback,
}

[CreateAssetMenu(fileName = "NewUtilitySpell", menuName = "UtilitySpell", order = 0)]
public class UtilitySpell : Spell
{
    public UtilityType utilityType;
}
