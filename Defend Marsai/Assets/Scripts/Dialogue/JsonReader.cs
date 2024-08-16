using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    private TextAsset _jsonFile; 

    public Cutscene DeserializeCutscene(){
        Cutscene cutscene = JsonUtility.FromJson<Cutscene>(_jsonFile.text); 
        return cutscene; 
    }

    public void LoadJsonFile(string jsonFile){
        _jsonFile = Resources.Load(jsonFile) as TextAsset; 
    }
}