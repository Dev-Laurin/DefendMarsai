using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimationManager : MonoBehaviour
{
    [SerializeField] private List<SpriteAnimation> _spriteAnimations = new List<SpriteAnimation>(); 
    private SpriteAnimation _currentAnimation; 
    private int _currentAnimationIndex = 0; 

    //Play the Animation
    private SpriteAnimation _spriteAnimation; 
    private List<Sprite> _sprites; 
    private List<float> _secondsBetSprites; 

    private int _index = 0; 
    private bool _isDone;
    private Image _image; 

    public void PlayerStopAnimation(){
        _isDone = true; 
        StopCoroutine(PlayAnimation());  
    }

    public void PlayerStartAnimation(SpriteAnimation spriteAnim, Image image){
        _image = image; 
        _spriteAnimation = spriteAnim; 
        _secondsBetSprites = _spriteAnimation.GetSecondsBetSprites(); 
        _sprites = _spriteAnimation.GetSprites(); 
        _isDone = false; 
        StartCoroutine(PlayAnimation()); 
    }

    private IEnumerator PlayAnimation(){
        Debug.Log("Started playing animation"); 
        int secondsIndex = _index - 1; 
        if(secondsIndex <= 0){
            secondsIndex = 0; 
        }
        Debug.Log("Before wait for seconds"); 
        yield return new WaitForSeconds(_secondsBetSprites[secondsIndex]); 

        Debug.Log("After wait for seconds"); 
        if(_index >= _sprites.Count){
            _index = 0; 
        }
        Debug.Log("Before image override"); 

        _image.overrideSprite = _sprites[_index]; 
        _image.SetMaterialDirty(); 
        _index++; 
        
        if(!_isDone){
            StartCoroutine(PlayAnimation()); 
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        _image = gameObject.GetComponent<Image>();
        if(_spriteAnimations.Count > 0){
            _currentAnimation = _spriteAnimations[_currentAnimationIndex]; 
            //PlayerStartAnimation(_currentAnimation, _image); 
        }
    }

    public void SwitchAnimation(int animationIndex){
        if(animationIndex < _spriteAnimations.Count && animationIndex > -1){
            Debug.Log($"Switching Animation to {_spriteAnimations[animationIndex]}"); 
            PlayerStopAnimation(); 
            _currentAnimationIndex = animationIndex; 
            PlayerStartAnimation(_currentAnimation, _image); 
        }
        else{
            Debug.Log($"Animation Manager: Animation Index is out of range. {animationIndex}"); 
        }
    }

    public void StopAnimation(){
        Debug.Log($"current animation index: {_currentAnimationIndex}"); 
        if(_currentAnimationIndex < _spriteAnimations.Count && _currentAnimationIndex > -1){
            PlayerStopAnimation(); 
        }
    }

    public void StartAnimation(){
        Debug.Log($"current animation index: {_currentAnimationIndex}");
        Debug.Log($"animation count: {_spriteAnimations.Count}");
        if(_currentAnimationIndex < _spriteAnimations.Count && _currentAnimationIndex > -1){
            _currentAnimation = _spriteAnimations[_currentAnimationIndex]; 
            PlayerStartAnimation(_currentAnimation, _image); 
        }
    }
}
