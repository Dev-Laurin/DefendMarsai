using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Pawn : MonoBehaviour
{
    //Stats
    [SerializeField] private string _name; 
    [SerializeField] private int _movement; 
    [SerializeField] private int _strength;
    [SerializeField] private int _speed = 5; 
    [SerializeField] private int _defense; 
    [SerializeField] private int _will; 
    [SerializeField] private int _fatigue; 
    [SerializeField] private int _hp; 

    //selection
    private Color _oldMatColor; 
    private bool isSelected; 
    private Renderer _renderer; 
    private GameManager _gameManager; 
    private bool _isEnemy = false; 

    //UI
    [SerializeField] private GameObject _portrait;
    [SerializeField] private Sprite _portraitImage;
    [SerializeField] private Sprite _classImage;
        
    //position
    [SerializeField] private Tile _currentTile;
    private GameObject _targetTile;  

    //movement
    private CharacterController _controller; 
    private float _step; 
    private bool _move; 

    public void Start(){
        _renderer = gameObject.GetComponent<Renderer>(); 
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); 
        _controller = gameObject.GetComponent<CharacterController>(); 
    }

    public void UpdateUI(){
        _gameManager.UpdatePortraitUI(_strength, _speed, _defense, _will, _fatigue, _hp, _name, _portraitImage, _classImage); 
    }

    public void SetAsEnemy(bool isEnemy){
        _isEnemy = isEnemy; 
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

    private void Deselect(){
        isSelected = false; 
        Unhighlight(); 
        HidePortrait(); 
        _gameManager.DeselectedPawn(); 
    }

    private void Select(){
        isSelected = true; 
        Highlight(isSelected);
        ShowAvailableMovement(); 
        UpdateUI(); 
        ShowPortrait(); 
    }

    void OnMouseDown(){
        if(isSelected){
            Deselect(); 
        }
        else{
            Select(); 
        }
    }

    void ShowPortrait(){
        _portrait.SetActive(true); 
    }

    void HidePortrait(){
        _portrait.SetActive(false); 
    }

    void ShowAvailableMovement(){
        var tile = GetTile(); 
        var xcoord = tile.GetXCoord(); 
        var zcoord = tile.GetZCoord(); 
        _gameManager.ShowAvailableMovement(gameObject); 
    }

    public int GetMovement(){
        return _movement; 
    }

    public void MoveToTile(GameObject tile){
        if(!_isEnemy){
            tile.GetComponent<Tile>().RevertToOriginalTilesMat(); 
            _targetTile = tile;
            _move = true; 
            _currentTile.RevertToOriginalTilesMat(); 
            _currentTile = _targetTile.GetComponent<Tile>(); 
        }
        Deselect();
    }

    public Tile GetTile(){
        return _currentTile; 
    }

    public void SetCurrentTile(Tile tile){
        _currentTile = tile; 
    }

    public void SetPortrait(GameObject portrait){
        _portrait = portrait; 
    }

    void Update(){
        if(_move){
            var step = _speed * Time.deltaTime; 
            var target = _targetTile.transform.position; 
            target.y = transform.position.y; 
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }
    }
}
