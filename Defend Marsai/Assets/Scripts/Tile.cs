using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Color _oldMatColor; 
    private GameObject _gameManager; 
    private bool _selectible = false; 

    void Start(){
        _gameManager = GameObject.Find("GameManager"); 
    }

    void OnMouseEnter(){
        var tileRenderer = gameObject.GetComponent<Renderer>(); 
        _oldMatColor = gameObject.GetComponent<Renderer>().material.color; 
        tileRenderer.material.color = new Color(1, 0.54f, 0, 1); 
    }
    
    void OnMouseExit(){
        gameObject.GetComponent<Renderer>().material.color = _oldMatColor; 
    }

    void OnMouseDown(){
        if(_selectible){
            _gameManager.GetComponent<GameManager>().TileSelected(gameObject); 
        } 
    }

}
