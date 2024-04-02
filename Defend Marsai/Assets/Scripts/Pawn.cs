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
    private GameObject _gameManager; 

    //UI
    [SerializeField] private GameObject _portrait;
        

    //position
    [SerializeField] private Tile _currentTile;
    private GameObject _targetTile;  

    //movement
    private CharacterController _controller; 
    private float _step; 
    private bool _move; 

    void Start(){
        _renderer = gameObject.GetComponent<Renderer>(); 
        _gameManager = GameObject.Find("GameManager"); 
        _controller = gameObject.GetComponent<CharacterController>(); 
    }

    public void SetUI(GameObject strength, GameObject speed, GameObject defense, GameObject will, GameObject fatigue, GameObject hp, GameObject name){
        strength.GetComponent<TMPro.TextMeshProUGUI>().text = _strength.ToString(); 
        speed.GetComponent<TMPro.TextMeshProUGUI>().text = _speed.ToString(); 
        defense.GetComponent<TMPro.TextMeshProUGUI>().text = _defense.ToString(); 
        will.GetComponent<TMPro.TextMeshProUGUI>().text = _will.ToString(); 
        fatigue.GetComponent<Slider>().value = _fatigue; 
        hp.GetComponent<Slider>().value = _hp; 
        name.GetComponent<TMPro.TextMeshProUGUI>().text = _name; 
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
        _gameManager.GetComponent<GameManager>().DeselectedPawn(); 
    }

    private void Select(){
        isSelected = true; 
        Highlight(isSelected);
        ShowAvailableMovement(); 
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
        _gameManager.GetComponent<GameManager>().ShowAvailableMovement(gameObject); 
    }

    public int GetMovement(){
        return _movement; 
    }

    public void MoveToTile(GameObject tile){
        tile.GetComponent<Tile>().RevertToOriginalTilesMat(); 
        _targetTile = tile;
        _move = true; 
        _currentTile.RevertToOriginalTilesMat(); 
        _currentTile = _targetTile.GetComponent<Tile>(); 
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
