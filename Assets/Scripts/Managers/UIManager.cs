using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public void OnFinishTurnClicked()
    {
        StateMachine.Instance.ProcessEvent(Event.FinishPlayerTurn);
    }

}

