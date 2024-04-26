using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishTurnButton : MonoBehaviour
{
    private void Start()
    {
        UIManager ui = UIManager.Instance;
        Button bt = GetComponent<Button>();
        bt.onClick.AddListener(() => ui.OnFinishTurnButtonClickedListener());
    }

}
