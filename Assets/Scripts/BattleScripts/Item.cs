using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item", order = 0)]
public class Item : Spell
{
    public Item()
    {
        actionPointCost = 0;
        spellAreaType = SpellAreaType.Self;
        animationType = AnimationType.Idle;
    }
}