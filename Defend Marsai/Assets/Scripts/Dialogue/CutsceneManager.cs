using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] public GameObject leftPanel; 
    [SerializeField] public GameObject rightPanel; 
    [SerializeField] public GameObject leftPortrait; 
    [SerializeField] public GameObject rightPortrait;
    [SerializeField] public GameObject background; 
    [SerializeField] public GameObject leftCharacterName; 
    [SerializeField] public GameObject rightCharacterName; 
    [SerializeField] public GameObject leftTextSymbol; 
    [SerializeField] public GameObject rightTextSymbol; 

    [SerializeField] public AudioService _audioService;  
    private JsonReader _jsonReader = new JsonReader();
    
    private bool cutscenePlaying = false; 
    private int _index = 0; 
    private bool _left = true; 
    private Conversation _conversation;
    private Cutscene _cutscene; 

    // Start is called before the first frame update
    void Start()
    {
        LoadCutscene("Cutscenes/tutorial"); 
    }

    public void LoadCutscene(string filename){
        _jsonReader.LoadJsonFile(filename); 
        _cutscene = _jsonReader.DeserializeCutscene(); 
        _conversation = _cutscene.conversation; 

        cutscenePlaying = true; 
        UpdateBackground(_cutscene.scene, background); 
        StartCoroutine(_audioService.LoadAudio(_cutscene.music)); 
        StartCoroutine(ContinueCutscene()); 

        UpdateTextSymbol(true); 
    }

    public void NextText(){
        StartCoroutine(ContinueCutscene()); 
    }

    private IEnumerator ContinueCutscene(){
        if(_index >= _conversation.messages.Length){
            cutscenePlaying = false; 
            yield break;
        }

        Message message = _conversation.messages[_index]; 
        if(_left){
            DisplayDialogue(message.message, leftPanel, leftCharacterName, message.name); 
            UpdatePortrait(message.sprite, message.name, leftPortrait); 
            UpdateTextSymbol(true); 
        }
        else{
            DisplayDialogue(message.message, rightPanel, rightCharacterName, message.name); 
            UpdatePortrait(message.sprite, message.name, rightPortrait); 
            UpdateTextSymbol(false); 
        }
        
        _index += 1; 
        _left = !_left; 
        yield return new WaitForSeconds(3);
    }

    private void DisplayDialogue(string text, GameObject textObj, GameObject nameTextObj, string characterName){
        textObj.GetComponent<TMPro.TextMeshProUGUI>().text = text;
        nameTextObj.GetComponent<TMPro.TextMeshProUGUI>().text = characterName;
    }

    private void UpdatePortrait(string sprite, string characterName, GameObject portrait){
        portrait.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Sprites/" + characterName + "/" + sprite); 
    }

    private void UpdateBackground(string sprite, GameObject background){
        background.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Backgrounds/" + sprite); 
    }

    private void UpdateTextSymbol(bool isLeft){
        if(isLeft){
            leftTextSymbol.SetActive(true); 
            rightTextSymbol.SetActive(false); 
        }
        else{
            leftTextSymbol.SetActive(false); 
            rightTextSymbol.SetActive(true); 
        }
    }
}
