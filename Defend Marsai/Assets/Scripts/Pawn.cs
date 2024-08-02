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
    [SerializeField] private int _maxFatigue; 
    [SerializeField] private int _hp; 
    [SerializeField] private int _range; 
    [SerializeField] private int _maxHP; 

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
    [SerializeField] private RuntimeAnimatorController _portraitAnimator;
    [SerializeField] private SpriteAnimation _portraitAnimation;  
        
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
        _oldMatColor = _renderer.material.color;
    }

    public void UpdateUI(){
        _uiManager.UpdateSelectionUI(gameObject.GetComponent<Pawn>()); 
    }

    public SpriteAnimation GetPortraitAnimation(){
        return _portraitAnimation; 
    }

    public void SetAsEnemy(bool isEnemy){
        _isEnemy = isEnemy; 
    }

    private void Highlight(bool selecting = false){
        if(!selecting){
            _oldMatColor = _renderer.material.color;
        }
        _renderer.material.color = new Color(1, 0.54f, 0, 1);         
    }

    private void Unhighlight(){
        _renderer.material.color = _oldMatColor; 
    }

    public int CalculateTargetPriority(Pawn pawn){
        //TODO
        return 1;  
    }

    void OnMouseEnter(){
        if(!isSelected && _battleSystem.isSelectable() && _uiManager.IsPlayState()){
            Highlight(); 
        }
    }
    
    void OnMouseExit(){
        if(!isSelected && _battleSystem.isSelectable() && _uiManager.IsPlayState()){
            Unhighlight(); 
        }
    }

    public bool UnitInRange(Pawn unit){
        var path = PathFinding.DijkstraWithGoal(GetTile(), unit.GetTile(), _range, _battleSystem.FindNeighbors); 
        return path.Count <= _range; 
    }

    public IEnumerator MoveToTileViaPath(Queue<Tile> path){
        while(path.Count > 0){
            MoveToTile(_battleSystem.TileToGameObject(path.Dequeue())); 
            yield return new WaitForSeconds(1); 
        }
    }

    public void Deselect(){
        Debug.Log($"Deselecting {_name}"); 
        isSelected = false; 
        Unhighlight(); 
        HidePortrait(); 
        _battleSystem.DeselectedPawn(); 
    }

    public void Select(){
        Debug.Log($"Selecting {_name}"); 
        if(_isEnemy && _battleSystem.InRange(gameObject)){
            isSelected = true; 
            Highlight(isSelected);
            _battleSystem.UnitSelected(gameObject); 
        }
        else if(!_battleSystem.UnitSelectable(gameObject)){
            //turn has already been used
            return; 
        }
        else{
            isSelected = true; 
            Highlight(isSelected);
            ShowAvailableMovement(); 
            ShowPortrait();
            UpdateUI(); 
        }
        
    }

    void OnMouseDown(){
        if(_battleSystem.isPlayerTurn() && !_battleSystem.AwaitingPlayerOption() && _uiManager.IsPlayState()){
            if(isSelected){
                Deselect(); 
            }
            else{
                Select(); 
            }
        }
    }

    IEnumerator PlayDamageAnimation(){
        if(!isSelected){
            _oldMatColor = _renderer.material.color;
        }
        _renderer.material.color = new Color(1, 0.5f, 0.5f, 1); 
        yield return new WaitForSeconds(1); 
        _renderer.material.color = _oldMatColor;
    }

    void ShowPortrait(){
        _uiManager.ShowPortrait(true); 
    }

    void HidePortrait(){
        _uiManager.ShowPortrait(false); 
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
        _move = true; 
        tile.GetComponent<Tile>().RevertToOriginalTilesMat(); 
        _targetTile = tile;
        _currentTile.RevertToOriginalTilesMat(); 
        _currentTile = _targetTile.GetComponent<Tile>();

        bool moreActions = _battleSystem.HighlightInteractableUnits(gameObject); 
        if(!moreActions){
            Deselect();
            _battleSystem.PlayerFinishedUnit(gameObject);  
        }
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

    public int GetRange(){
        return _range; 
    }

    public int GetMaxHP(){
        return _maxHP; 
    }

    public int GetMaxFatigue(){
        return _maxFatigue; 
    }

    public Sprite GetPortrait(){
        return _portraitImage; 
    }

    public RuntimeAnimatorController GetPortraitAnimator(){
        return _portraitAnimator; 
    }

    public Sprite GetClassImage(){
        return _classImage; 
    }

    public Tile GetTile(){
        return _currentTile; 
    }

    public int EstimatedDamageTaken(int strength){
        int damageTaken = strength - _defense;
        if(damageTaken < 0){
            damageTaken = 0; //we defended
        }  
        return damageTaken; 
    }

    public int TakeDamage(int damage){
        int damageTaken = damage - _defense;
        if(damageTaken < 0){
            damageTaken = 0; //we defended
        }  
        _hp = _hp - damageTaken; 

        if(_hp <= 0){
            _hp = 0; 
            _battleSystem.RemoveUnit(gameObject); 
        }

        IncreaseFatigue(1); 
        StartCoroutine(PlayDamageAnimation()); 

        return damageTaken; 
    }

    public void IncreaseFatigue(int num){
        _fatigue += num; 
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
