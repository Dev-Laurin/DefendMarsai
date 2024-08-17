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
    }

    private IEnumerator ContinueCutscene(){
        if(_index >= _conversation.messages.Length){
            cutscenePlaying = false; 
            yield break;
        }
        if(_left){
            DisplayDialogue(_conversation.messages[_index].message, leftPanel); 
            UpdatePortrait(_conversation.messages[_index].sprite, _conversation.messages[_index].name, leftPortrait); 
        }
        else{
            DisplayDialogue(_conversation.messages[_index].message, rightPanel); 
            UpdatePortrait(_conversation.messages[_index].sprite, _conversation.messages[_index].name, rightPortrait); 
        }
        
        _index += 1; 
        _left = !_left; 
        yield return new WaitForSeconds(3);
        yield return StartCoroutine(ContinueCutscene()); 
    }

    private void DisplayDialogue(string text, GameObject textObj){
        textObj.GetComponent<TMPro.TextMeshProUGUI>().text = text;
    }

    private void UpdatePortrait(string sprite, string characterName, GameObject portrait){
        portrait.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Sprites/" + characterName + "/" + sprite); 
    }

    private void UpdateBackground(string sprite, GameObject background){
        background.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Backgrounds/" + sprite); 
    }
}
