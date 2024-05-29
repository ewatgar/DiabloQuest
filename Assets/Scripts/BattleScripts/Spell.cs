using UnityEngine;

public enum SpellAreaType
{
    Melee,
    Range,
    Donut,
    Line,
    Self
}

public enum UtilityType
{
    Damage,
    Healing,
    Knockback,
    MovementPoints,
    ActionPoints,
    Crits
}

[CreateAssetMenu(fileName = "NewSpell", menuName = "Spell", order = 0)]
public class Spell : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite artwork;
    public int actionPointCost;
    public int value;
    public SpellAreaType spellAreaType;
    public UtilityType utilityType;
    public AnimationType animationType;
}