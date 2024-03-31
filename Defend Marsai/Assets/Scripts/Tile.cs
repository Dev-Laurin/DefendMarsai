using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Color _oldMatColor; 

    void OnMouseEnter(){
        var tileRenderer = gameObject.GetComponent<Renderer>(); 
        _oldMatColor = gameObject.GetComponent<Renderer>().material.color; 
        tileRenderer.material.color = new Color(1, 0.54f, 0, 1); 
    }
    
    void OnMouseExit(){
        gameObject.GetComponent<Renderer>().material.color = _oldMatColor; 
    }

}
