using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; 
public class AudioService: MonoBehaviour
{
    private AudioSource _audioSource; 
    private string _soundPath = "file://" + Application.streamingAssetsPath + "/Music/";

    void Start(){
        _audioSource = GetComponent<AudioSource>(); 
    }
    public IEnumerator LoadAudio(string audioFile){
        string filePath = string.Format(_soundPath + "{0}", audioFile); 
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG); 

        
        yield return request.Send(); 
        if(request.isNetworkError){
            Debug.Log(request.error); 
        }
        else{
            var clip = DownloadHandlerAudioClip.GetContent(request); 
            _audioSource.clip = clip; 
            PlayAudio(); 
        }
    }

    public void PlayAudio(){
        _audioSource.Play(); 
        _audioSource.loop = true; 
    }

    public void StopAudio(){
        _audioSource.Stop(); 
    }
}