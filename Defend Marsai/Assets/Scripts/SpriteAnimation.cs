using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

[CreateAssetMenu(fileName = "New Sprite Animation", menuName="Custom/Animation")]
public class SpriteAnimation : ScriptableObject
{
    [SerializeField] private string _name; 
    [SerializeField] private float _speed = 0.2f; 
    [SerializeField] private float _animationLengthSeconds = 5.0f; 
    [SerializeField] private List<Sprite> _sprites; 
    [SerializeField] private List<float> _secondsBetSprites; 
     
    public List<float> GetSecondsBetSprites(){
        return _secondsBetSprites; 
    }

    public string GetName(){
        return _name; 
    }

    public List<Sprite> GetSprites(){
        return _sprites; 
    }

}
