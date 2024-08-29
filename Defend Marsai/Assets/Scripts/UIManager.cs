using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum UIState{
        PLAY = 1,
        IN_MENU = 2
    }
    private UIState _state; 

    //Battle UI
    [SerializeField] private GameObject _strengthUI; 
    [SerializeField] private GameObject _speedUI;
    [SerializeField] private GameObject _defenseUI;
    [SerializeField] private GameObject _willUI;
    [SerializeField] private GameObject _fatigueUI;
    [SerializeField] private GameObject _hpUI; 
    [SerializeField] private GameObject _nameUI; 
    [SerializeField] private GameObject _portraitImage;
    [SerializeField] private GameObject _portraitPanel; 
    [SerializeField] private GameObject _classImage;
    [SerializeField] private GameObject _turnTextUI; 
    [SerializeField] private GameObject _options; 
    [SerializeField] private GameObject _actions;

    //Battle Options UI
    //unit 1 
    [SerializeField] private GameObject _unit1Image; 
    [SerializeField] private GameObject _unit1HPSliderUI;
    [SerializeField] private GameObject _unit1FatigueSliderUI;
    [SerializeField] private GameObject _unit1StrengthUI; 
    [SerializeField] private GameObject _unit1DefenseUI; 
    [SerializeField] private GameObject _unit1SpeedUI; 
    [SerializeField] private GameObject _unit1WillUI; 
    [SerializeField] private GameObject _unit1ClassUI; 
    [SerializeField] private GameObject _actionTypeImage; 

    //unit 2
    [SerializeField] private GameObject _unit2Image; 
    [SerializeField] private GameObject _unit2HPSliderUI;
    [SerializeField] private GameObject _unit2FatigueSliderUI;
    [SerializeField] private GameObject _unit2StrengthUI; 
    [SerializeField] private GameObject _unit2DefenseUI; 
    [SerializeField] private GameObject _unit2SpeedUI; 
    [SerializeField] private GameObject _unit2WillUI; 
    [SerializeField] private GameObject _unit2ClassUI; 

    //UI
    [SerializeField] private GameObject _menuPanel; 
    [SerializeField] private GameObject _noAttackActionsMenu; 

    //vars 
    private TMPro.TextMeshProUGUI _turnText; 

    public void UpdateSelectionUI(Pawn pawn){
        _strengthUI.GetComponent<TMPro.TextMeshProUGUI>().text = pawn.GetStrength().ToString(); 
        _speedUI.GetComponent<TMPro.TextMeshProUGUI>().text = pawn.GetSpeed().ToString(); 
        _defenseUI.GetComponent<TMPro.TextMeshProUGUI>().text = pawn.GetDefense().ToString(); 
        _willUI.GetComponent<TMPro.TextMeshProUGUI>().text = pawn.GetWill().ToString(); 
        _fatigueUI.GetComponent<Slider>().value = pawn.GetFatigue(); 
        _fatigueUI.GetComponent<Slider>().maxValue = pawn.GetMaxFatigue(); 
        _hpUI.GetComponent<Slider>().value = pawn.GetHP(); 
        _hpUI.GetComponent<Slider>().maxValue = pawn.GetMaxHP(); 
        _nameUI.GetComponent<TMPro.TextMeshProUGUI>().text = pawn.GetName(); 
        _portraitImage.GetComponent<UISpriteAnimationManager>().PlayerStopAnimation(); 
        _portraitImage.GetComponent<Image>().overrideSprite = pawn.GetPortrait(); 
        _portraitImage.GetComponent<UISpriteAnimationManager>().PlayerStartAnimation(pawn.GetPortraitAnimation()); 
        _classImage.GetComponent<Image>().overrideSprite = pawn.GetClassImage(); 
    }

    public void UpdateOptionsMenu(Pawn pawn, Pawn otherPawn){
        _unit1HPSliderUI.GetComponent<Slider>().value = pawn.GetHP();
        _unit1HPSliderUI.GetComponent<Slider>().maxValue = pawn.GetMaxHP();
        _unit1FatigueSliderUI.GetComponent<Slider>().value = pawn.GetFatigue();
        _unit1FatigueSliderUI.GetComponent<Slider>().maxValue = pawn.GetMaxFatigue();
        _unit1StrengthUI.GetComponent<TMPro.TextMeshProUGUI>().text = pawn.GetStrength().ToString(); 
        _unit1DefenseUI.GetComponent<TMPro.TextMeshProUGUI>().text = pawn.GetDefense().ToString(); 
        _unit1ClassUI.GetComponent<Image>().overrideSprite = pawn.GetClassImage();
        _unit1SpeedUI.GetComponent<TMPro.TextMeshProUGUI>().text = pawn.GetSpeed().ToString();
        _unit1WillUI.GetComponent<TMPro.TextMeshProUGUI>().text = pawn.GetWill().ToString(); 
        _unit1Image.GetComponent<Image>().overrideSprite = pawn.GetPortrait(); 

        _unit2HPSliderUI.GetComponent<Slider>().value = otherPawn.GetHP();
        _unit2HPSliderUI.GetComponent<Slider>().maxValue = otherPawn.GetMaxHP();
        _unit2FatigueSliderUI.GetComponent<Slider>().value = otherPawn.GetFatigue();
        _unit2FatigueSliderUI.GetComponent<Slider>().maxValue = otherPawn.GetMaxFatigue();
        _unit2StrengthUI.GetComponent<TMPro.TextMeshProUGUI>().text = otherPawn.GetStrength().ToString(); 
        _unit2DefenseUI.GetComponent<TMPro.TextMeshProUGUI>().text = otherPawn.GetDefense().ToString(); 
        _unit2ClassUI.GetComponent<Image>().overrideSprite = otherPawn.GetClassImage();
        _unit2SpeedUI.GetComponent<TMPro.TextMeshProUGUI>().text = otherPawn.GetSpeed().ToString();
        _unit2WillUI.GetComponent<TMPro.TextMeshProUGUI>().text = otherPawn.GetWill().ToString();
        _unit2Image.GetComponent<Image>().overrideSprite = otherPawn.GetPortrait(); 

        bool active = true; 
        _actionTypeImage.SetActive(active); 
        _options.SetActive(active); 
        _actions.SetActive(active); 
        ShowNoAttackActionsMenu(false); 

        UpdateOptionSliders(pawn, otherPawn); 

        _state = UIState.IN_MENU; 
    }

    public void ShowItemsMenu(){
        Debug.Log("Show Items Menu"); 
    }

    public void ShowActionsMenu(){
        _actions.SetActive(true); 
    }

    public void ShowNoAttackActionsMenu(bool active){
        Debug.Log("Show no attack actions menu"); 
        _noAttackActionsMenu.SetActive(active); 
    }

    private void UpdateOptionSliders(Pawn pawn, Pawn otherPawn){
        _unit1HPSliderUI.GetComponent<Slider>().value = pawn.GetHP();
        _unit1HPSliderUI.GetComponent<Slider>().maxValue = pawn.GetMaxHP();
        _unit1FatigueSliderUI.GetComponent<Slider>().value = pawn.GetFatigue();
        _unit1FatigueSliderUI.GetComponent<Slider>().maxValue = pawn.GetMaxFatigue();
        _unit2HPSliderUI.GetComponent<Slider>().value = otherPawn.GetHP();
        _unit2HPSliderUI.GetComponent<Slider>().maxValue = otherPawn.GetMaxHP();
        _unit2FatigueSliderUI.GetComponent<Slider>().value = otherPawn.GetFatigue();
        _unit2FatigueSliderUI.GetComponent<Slider>().maxValue = otherPawn.GetMaxFatigue();
        _state = UIState.IN_MENU; 
    }

    public void StartBattleUI(){
        try{
            _turnText = _turnTextUI.GetComponent<TMPro.TextMeshProUGUI>();
        }
        catch{
            Debug.Log("Turn Text UI not found."); 
        }

        bool active = false; 
        try{
            ShowPortrait(active); 
            DisplayOptions(active); 
            ShowEndBattleMenu(active); 
            ShowNoAttackActionsMenu(active); 
        }
        catch{
            Debug.Log("UI elements disabled"); 
        }
         
        _state = UIState.PLAY; 
    }

    public void DisplayOptions(bool show){
        _options.SetActive(show); 
        _actions.SetActive(show); 
        _actionTypeImage.SetActive(show); 

        if(show){
            _state = UIState.IN_MENU;
        }
        else{
            _state = UIState.PLAY; 
        }
         
    }

    public void UpdateTurnText(string text){
        _turnText.text = text; 
    }

    public void ShowPortrait(bool show){
        _portraitPanel.SetActive(show); 
        if(!show){
            _portraitImage.GetComponent<UISpriteAnimationManager>().StopAnimation();
        }
    }

    public void ShowLoseUI(){
        UpdateTurnText("You Lost."); 
        ShowEndBattleMenu(true); 
    }

    public void ShowWinUI(){
        UpdateTurnText("You Won!");
        ShowEndBattleMenu(true); 
    }

    public void ShowEndBattleMenu(bool show){
        _menuPanel.SetActive(show); 
        _state = UIState.IN_MENU;
    }

    public bool IsMenuState(){
        return _state == UIState.IN_MENU; 
    }

    public bool IsPlayState(){
        return _state == UIState.PLAY; 
    }
}
