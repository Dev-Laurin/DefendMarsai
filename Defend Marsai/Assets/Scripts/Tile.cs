using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TileType{
        WOODS = 3, 
        PLAIN = 1, 
        MUD = 2, 
        INPASSIBLE = 100 
    }; 

public class Tile : MonoBehaviour
{
    private Color _oldMatColor; 
    private GameObject _gameManager; 
    public TMPro.TextMeshProUGUI _text; 
    private Material _oldMat; 
    private int _oldMatIndex;
    private Material _olderMat; 
    private int _olderMatIndex;
    [SerializeField] private TileType _type; 
    private int _xcoord; 
    private int _zcoord; 
    private bool _selectable = false; 
    [SerializeField] private Material _highlightMat; 

    void Start(){
        _gameManager = GameObject.Find("GameManager"); 
    }

    public void HighlightTile(Color color){
        var tileRenderer = gameObject.GetComponent<Renderer>(); 
        _oldMatColor = gameObject.GetComponent<Renderer>().material.color; 
        tileRenderer.material.color = color; 
    }

    public void ChangeTilesMat(Material material, int index){
        var meshRenderer = gameObject.GetComponent<Renderer>(); 
        var materialsCopy = meshRenderer.materials; 
        _olderMat = _oldMat; 
        _olderMatIndex = _oldMatIndex; 
        _oldMat = materialsCopy[index]; 
        _oldMatIndex = index; 
        materialsCopy[index] = material; 
        meshRenderer.materials = materialsCopy; 
    }

    public void RevertTilesMat(){
        var meshRenderer = gameObject.GetComponent<Renderer>(); 
        var materialsCopy = meshRenderer.materials; 
        materialsCopy[_oldMatIndex] = _oldMat; 
        meshRenderer.materials = materialsCopy; 
    }

    public void RevertRevertTilesMat(){
        var meshRenderer = gameObject.GetComponent<Renderer>(); 
        var materialsCopy = meshRenderer.materials; 
        materialsCopy[_olderMatIndex] = _olderMat; 
        meshRenderer.materials = materialsCopy; 
    }

    public void SetSelection(bool selectable){
        _selectable = selectable; 
    }

    public void DeHighlightTile(){
        gameObject.GetComponent<Renderer>().material.color = _oldMatColor; 
    }

    void OnMouseEnter(){
        ChangeTilesMat(_highlightMat, 0); 
    }
    
    void OnMouseExit(){
        RevertTilesMat(); 
    }

    void OnMouseDown(){
        if(_selectable){
            _gameManager.GetComponent<GameManager>().TileSelected(gameObject); 
            RevertRevertTilesMat();
        } 
    }

    public int GetCost(){
        return (int)_type; 
    }

    public void SetXCoord(int x){
        _xcoord = x; 
    }

    public void SetZCoord(int z){
        _zcoord = z; 
    }

    public int GetXCoord(){
        return _xcoord; 
    }

    public int GetZCoord(){
        return _zcoord; 
    }
}
