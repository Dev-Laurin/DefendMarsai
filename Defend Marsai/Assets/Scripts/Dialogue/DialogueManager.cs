using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] public GameObject leftPanel; 
    [SerializeField] public GameObject rightPanel; 
    private JsonReader _jsonReader = new JsonReader(); 
    private bool cutscenePlaying = false; 
    private int _index = 0; 
    private bool _left = true; 
    private Conversation _conversation;

    // Start is called before the first frame update
    void Start()
    {
        LoadCutscene("Cutscenes/tutorial"); 
    }

    // Update is called once per frame
    void Update()
    {
        if(cutscenePlaying){
            //user input
        }
    }

    public void LoadCutscene(string filename){
        _jsonReader.LoadJsonFile(filename); 
        _conversation = _jsonReader.DeserializeCutscene().conversation; 

        cutscenePlaying = true; 
        StartCoroutine(ContinueCutscene()); 
    }

    private IEnumerator ContinueCutscene(){
        if(_index >= _conversation.messages.Length){
            Debug.Log("Conversation finished"); 
            cutscenePlaying = false; 
            yield break;
        }
        if(_left){
            DisplayDialogue(_conversation.messages[_index].message, leftPanel); 
        }
        else{
            DisplayDialogue(_conversation.messages[_index].message, rightPanel); 
        }
        
        _index += 1; 
        _left = !_left; 
        yield return new WaitForSeconds(4);
        yield return StartCoroutine(ContinueCutscene()); 
    }

    private void DisplayDialogue(string text, GameObject textObj){
        textObj.GetComponent<TMPro.TextMeshProUGUI>().text = text;
    }
}
