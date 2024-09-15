using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUISpriteAnimation : MonoBehaviour
{
    private SpriteAnimation _spriteAnimation; 
    private List<Sprite> _sprites; 
    private List<float> _secondsBetSprites; 

    private int _index = 0; 
    private bool _isDone;
    private Image _image; 

    public void StopAnimation(){
        _isDone = true; 
        StopCoroutine(PlayAnimation());  
    }

    public void StartAnimation(SpriteAnimation spriteAnim, Image image){
        _image = image; 
        _spriteAnimation = spriteAnim; 
        _secondsBetSprites = _spriteAnimation.GetSecondsBetSprites(); 
        _sprites = _spriteAnimation.GetSprites(); 
        _isDone = false; 
        StartCoroutine(PlayAnimation()); 
    }

    private IEnumerator PlayAnimation(){
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
}
