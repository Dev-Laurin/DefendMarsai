using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using UnityEngine.UI;
using TMPro; 

public class GameManager : MonoBehaviour
{    

    private UIManager _uiManager; 
    private BattleSystem _battleSystem; 

    void Start()
    {
        _uiManager = gameObject.GetComponent<UIManager>();
        _uiManager.StartBattleUI(); 
        _battleSystem = gameObject.GetComponent<BattleSystem>(); 
        _battleSystem.StartBattle();  
    }    

    public UIManager GetUIManager(){
        return _uiManager; 
    }

    public BattleSystem GetBattleSystem(){
        return _battleSystem; 
    }
}
