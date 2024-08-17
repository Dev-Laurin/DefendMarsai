using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneInputManager : MonoBehaviour
{

    [SerializeField] public GameObject cutsceneManagerObj; 

    private CutsceneManager _cutsceneManager; 
    // Start is called before the first frame update
    void Start()
    {
        _cutsceneManager = cutsceneManagerObj.GetComponent<CutsceneManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            _cutsceneManager.NextText(); 
        }
    }
}
