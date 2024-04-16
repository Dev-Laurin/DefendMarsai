using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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
        _portraitImage.GetComponent<Image>().overrideSprite = pawn.GetPortrait(); 
        _classImage.GetComponent<Image>().overrideSprite = pawn.GetClassImage(); 
    }

    public void UpdateOptionsMenu(Pawn pawn, Pawn otherPawn){
        Debug.Log("updating options menu"); 

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

        _options.SetActive(true); 

        UpdateOptionSliders(pawn, otherPawn); 
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
    }

    public void StartBattleUI(){
        _turnText = _turnTextUI.GetComponent<TMPro.TextMeshProUGUI>();
        ShowEndBattleMenu(false); 
    }

    public void DisplayOptions(bool show){
        _options.SetActive(show); 
    }

    public void UpdateTurnText(string text){
        _turnText.text = text; 
    }

    public void ShowPortrait(bool show){
        _portraitPanel.SetActive(show); 
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
    }
}
