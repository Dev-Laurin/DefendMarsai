using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    //Stats
    [SerializeField] private string _name; 
    [SerializeField] private int _movement; 

    //selection
    private Color _oldMatColor; 
    private bool isSelected; 
    private Renderer _renderer; 
    private GameObject _gameManager; 

    //UI
    [SerializeField] private GameObject _portrait;

    void Start(){
        _renderer = gameObject.GetComponent<Renderer>(); 
        _gameManager = GameObject.Find("GameManager"); 
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
            HidePortrait(); 
        }
        else{
            Debug.Log($"Selected {_name}"); 
            isSelected = true; 
            Highlight(isSelected);
            ShowAvailableMovement(); 
            ShowPortrait(); 
        }
    }

    void ShowPortrait(){
        _portrait.SetActive(true); 
    }

    void HidePortrait(){
        _portrait.SetActive(false); 
    }

    void ShowAvailableMovement(){
        Debug.Log($"{_name}'s movement is {_movement}"); 
        _gameManager.GetComponent<GameManager>().ShowAvailableMovement(gameObject); 
    }

    public int GetMovement(){
        return _movement; 
    }

    public void MoveToTile(GameObject tile){
        //TODO
    }
}
