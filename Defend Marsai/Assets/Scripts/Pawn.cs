using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    //Stats
    [SerializeField] private string _name; 
    [SerializeField] private int movement; 

    //selection
    private Color _oldMatColor; 
    private bool isSelected; 
    private Renderer _renderer; 

    void Start(){
        _renderer = gameObject.GetComponent<Renderer>(); 
    }

    void Highlight(bool selecting = false){
        if(!selecting){
            _oldMatColor = _renderer.material.color;
        }
        _renderer.material.color = new Color(1, 0.54f, 0, 1);         
    }

    void Unhighlight(){
        _renderer.material.color = _oldMatColor; 
    }

    void OnMouseEnter(){
        if(!isSelected){
            Highlight(); 
        }
    }
    
    void OnMouseExit(){
        if(!isSelected){
            Unhighlight(); 
        }
    }

    void OnMouseDown(){
        if(isSelected){
            Debug.Log($"Deselected {_name}"); 
            isSelected = false; 
            Unhighlight(); 
        }
        else{
            Debug.Log($"Selected {_name}"); 
            isSelected = true; 
            Highlight(isSelected);
            ShowAvailableMovement(); 
        }
    }

    void ShowAvailableMovement(){
        Debug.Log($"{_name}'s movement is {movement}"); 
    }
}
