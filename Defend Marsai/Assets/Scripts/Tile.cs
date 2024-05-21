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
    private GameManager _gameManager; 
    private BattleSystem _battleSystem; 
    private UIManager _uiManager;
    public TextMesh _text; 
    [SerializeField] private Material _oldMat; 
    [SerializeField] private int _oldMatIndex;
    [SerializeField] private Material _originalMat; 
    [SerializeField] private TileType _type; 
    private int _xcoord; 
    private int _zcoord; 
    private bool _selectable = false; 
    [SerializeField] private Material _highlightMat; 

    void Start(){
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); 
        _battleSystem = _gameManager.GetBattleSystem(); 
        _uiManager = _gameManager.GetUIManager(); 
        _oldMatIndex = 0; 
        _oldMat = gameObject.GetComponent<Renderer>().materials[0];
        _originalMat = gameObject.GetComponent<Renderer>().materials[0];
    }

    public void HighlightTile(Color color){
        var tileRenderer = gameObject.GetComponent<Renderer>(); 
        _oldMatColor = gameObject.GetComponent<Renderer>().material.color; 
        tileRenderer.material.color = color; 
    }

    public void ChangeTilesMat(Material material, int index){
        var meshRenderer = gameObject.GetComponent<Renderer>(); 
        var materialsCopy = meshRenderer.materials;  
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

    public void RevertToOriginalTilesMat(){
        var meshRenderer = gameObject.GetComponent<Renderer>(); 
        var materialsCopy = meshRenderer.materials; 
        _oldMat = _originalMat; 
        materialsCopy[_oldMatIndex] = _originalMat; 
        meshRenderer.materials = materialsCopy; 
    }

    public void SetSelection(bool selectable){
        _selectable = selectable; 
    }

    public void DeHighlightTile(){
        gameObject.GetComponent<Renderer>().material.color = _oldMatColor; 
    }

    void OnMouseEnter(){
        if(_uiManager.IsPlayState()){
            ChangeTilesMat(_highlightMat, 0); 
        }
    }
    
    void OnMouseExit(){
        if(_uiManager.IsPlayState()){
            RevertTilesMat(); 
        }
    }

    void OnMouseDown(){
        if(_selectable && _battleSystem.isPlayerTurn() && !_battleSystem.AwaitingPlayerOption() && _uiManager.IsPlayState()){
            RevertToOriginalTilesMat();
            _battleSystem.TileSelected(gameObject); 
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
