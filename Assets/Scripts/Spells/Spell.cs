using UnityEngine;

public enum SpellAreaType
{
    Melee,
    Cross,
    Donut,
    Line,
    Range,
    Self
}

public abstract class Spell : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite artwork;
    public int actionPointCost;
    //public int baseDamage;
    public SpellAreaType spellAreaType;
    public int range; //only for Line and Range
}