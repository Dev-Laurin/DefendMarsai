using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimation : MonoBehaviour
{

    [SerializeField] private float _speed = 0.2f; 
    [SerializeField] private float _animationLengthSeconds = 5.0f; 
    [SerializeField] private List<Sprite> _sprites; 
    [SerializeField] private List<float> _secondsBetSprites; 
    private int _index = 0; 
    private Image _image; 
    private bool _isDone; 

    public void StopAnimation(){
        _isDone = true; 
        StopCoroutine(PlayAnimation());  
    }

    public void StartAnimation(){
        _isDone = false; 
        StartCoroutine(PlayAnimation()); 
    }

    IEnumerator PlayAnimation(){
        int secondsIndex = _index - 1; 
        if(secondsIndex <= 0){
            secondsIndex = 0; 
        }
        yield return new WaitForSeconds(_secondsBetSprites[secondsIndex]); 

        if(_index >= _sprites.Count){
            _index = 0; 
        }

        _image.overrideSprite = _sprites[_index]; 
        _image.SetMaterialDirty(); 
        _index++; 
        
        if(!_isDone){
            StartCoroutine(PlayAnimation()); 
        }

    }

    void Start(){
        _image = gameObject.GetComponent<Image>(); 
    }

}
