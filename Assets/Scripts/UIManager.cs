using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //[SerializeField] TextMeshPro finishPlayerTurnText;

    public void FinishPlayerTurn()
    {
        //finishPlayerTurnText.text = "Esperando a que acabe turno";
        StateMachine.Instance.ProcessEvent(Event.FinishPlayerTurn);
    }
}
