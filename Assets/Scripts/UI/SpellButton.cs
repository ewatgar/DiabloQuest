using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class SpellButton : MonoBehaviour
{
    private Spell _spell;

    public Spell Spell { get => _spell; set => _spell = value; }

    private void Awake()
    {
        gameObject.GetComponentInChildren<Image>().sprite = _spell.artwork;
    }

    private void OnMouseEnter()
    {
        //StateMachine.Instance.HandleSpellButtonClicked(_spell);
    }

    private void DisplayTextOnHover()
    {
        /*
        ext tempTextBox = Instantiate(textPrefab, nextPosition, transform.rotation) as Text;
                    //Parent to the panel
                    tempTextBox.transform.SetParent(renderCanvas.transform, false);
                    //Set the text box's text element font size and style:
                    tempTextBox.fontSize = defaultFontSize;
                    //Set the text box's text element to the current textToDisplay:
                    tempTextBox.text = textToDisplay;*/
    }

}
