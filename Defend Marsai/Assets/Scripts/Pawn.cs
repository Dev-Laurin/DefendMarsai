using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


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
    [SerializeField] private int _range; 

    //selection
    private Color _oldMatColor; 
    private bool isSelected; 
    private Renderer _renderer; 
    private GameManager _gameManager; 
    private bool _isEnemy = false; 
    private UIManager _uiManager; 
    private BattleSystem _battleSystem; 

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
        _uiManager = _gameManager.GetUIManager(); 
        _battleSystem = _gameManager.GetBattleSystem(); 
    }

    public void UpdateUI(){
        _uiManager.UpdateSelectionUI(gameObject.GetComponent<Pawn>()); 
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

    private void isSelectable(){

    }

    public int CalculateTargetPriority(Pawn pawn){
        //TODO
        return 1;  
    }

    void OnMouseEnter(){
        if(!isSelected && _battleSystem.isSelectable()){
            Highlight(); 
        }
    }
    
    void OnMouseExit(){
        if(!isSelected && _battleSystem.isSelectable()){
            Unhighlight(); 
        }
    }

    public bool UnitInRange(Pawn unit){
        var path = PathFinding.DijkstraWithGoal(GetTile(), unit.GetTile(), _range, _battleSystem.FindNeighbors); 
        return path != null; 
    }

    public IEnumerator MoveToTileViaPath(Queue<Tile> path){
        while(path.Count > 0){
            MoveToTile(_battleSystem.TileToGameObject(path.Dequeue())); 
            yield return new WaitForSeconds(1); 
        }
    }

    private void Deselect(){
        isSelected = false; 
        Unhighlight(); 
        HidePortrait(); 
        _battleSystem.DeselectedPawn(); 
    }

    private void Select(){
        isSelected = true; 
        Highlight(isSelected);
        ShowAvailableMovement(); 
        UpdateUI(); 
        ShowPortrait(); 
    }

    void OnMouseDown(){
        if(_battleSystem.isPlayerTurn()){
            if(isSelected){
                Deselect(); 
            }
            else{
                Select(); 
            }
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
        _battleSystem.ShowAvailableMovement(gameObject); 
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

    public int GetStrength(){
        return _strength; 
    }

    public int GetSpeed(){
        return _speed; 
    }

    public int GetWill(){
        return _will; 
    }

    public int GetFatigue(){
        return _fatigue; 
    }

    public int GetDefense(){
        return _defense; 
    }

    public int GetHP(){
        return _hp; 
    }

    public string GetName(){
        return _name; 
    }

    public Sprite GetPortrait(){
        return _portraitImage; 
    }

    public Sprite GetClassImage(){
        return _classImage; 
    }

    public Tile GetTile(){
        return _currentTile; 
    }

    public void TakeDamage(int damage){
        int damageTaken = damage - _defense;
        if(damageTaken < 0){
            damageTaken = 0; //we defended
        }  
        _hp = _hp - damageTaken; 

        if(_hp < 0){
            _hp = 0; 
            _battleSystem.RemoveUnit(gameObject); 
        }
    }

    public int LookupDamage(Pawn unit){
        int damage = _strength - unit.GetDefense(); 
        int otherUnitsEndingHealth = unit.GetHP() - damage; 
        if(otherUnitsEndingHealth <= 0){
            return -1; //snuffed
        } 
        return damage; 
    }

    

    public void SetCurrentTile(Tile tile){
        _currentTile = tile; 
    }

    public void SetPortrait(GameObject portrait){
        _portrait = portrait; 
    }

    public bool isEnemy(){
        return _isEnemy; 
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
