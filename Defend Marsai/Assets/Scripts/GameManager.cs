using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using UnityEngine.UI;
using TMPro; 

public enum GameState{
    PAUSE = 1,
    MAIN_MENU = 2, 
    BATTLE = 3
}

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

    public void AttackButtonPressed(){
        Debug.Log("Attack button pressed, gamemanager"); 
        StartCoroutine(_battleSystem.AttackButtonPressed()); 
    }

    public void EndUnitTurnButtonPressed(){
        _battleSystem.EndUnitTurnButtonPressed(); 
    }

    public void ResetMatchButtonPressed(){
        Debug.Log("Re-match!"); 
        _uiManager.StartBattleUI(); 
        _battleSystem.StartBattle(); 
    }

    public void QuitButtonPressed(){
        Debug.Log("Thanks for playing, Good bye."); 
        Application.Quit(); 
    }
}