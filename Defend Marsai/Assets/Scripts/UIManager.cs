using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //UI
    [SerializeField] private GameObject _strengthUI; 
    [SerializeField] private GameObject _speedUI;
    [SerializeField] private GameObject _defenseUI;
    [SerializeField] private GameObject _willUI;
    [SerializeField] private GameObject _fatigueUI;
    [SerializeField] private GameObject _hpUI; 
    [SerializeField] private GameObject _nameUI; 
    [SerializeField] private GameObject _portraitImage;
    [SerializeField] private GameObject _classImage;
    [SerializeField] private GameObject _turnTextUI; 
    [SerializeField] private GameObject _options; 

    //vars 
    private TMPro.TextMeshProUGUI _turnText; 

    public void UpdateSelectionUI(Pawn pawn){
        _strengthUI.GetComponent<TMPro.TextMeshProUGUI>().text = pawn.GetStrength().ToString(); 
        _speedUI.GetComponent<TMPro.TextMeshProUGUI>().text = pawn.GetSpeed().ToString(); 
        _defenseUI.GetComponent<TMPro.TextMeshProUGUI>().text = pawn.GetDefense().ToString(); 
        _willUI.GetComponent<TMPro.TextMeshProUGUI>().text = pawn.GetWill().ToString(); 
        _fatigueUI.GetComponent<Slider>().value = pawn.GetFatigue(); 
        _hpUI.GetComponent<Slider>().value = pawn.GetHP(); 
        _nameUI.GetComponent<TMPro.TextMeshProUGUI>().text = pawn.GetName(); 
        _portraitImage.GetComponent<Image>().overrideSprite = pawn.GetPortrait(); 
        _classImage.GetComponent<Image>().overrideSprite = pawn.GetClassImage(); 
    }

    public void UpdateOptionsMenu(Pawn pawn, Pawn otherPawn){
        
        _options.SetActive(true); 
    }

    public void StartBattleUI(){
        _turnText = _turnTextUI.GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void DisplayOptions(){

    }

    public void UpdateTurnText(string text){
        _turnText.text = text; 
    }
}
