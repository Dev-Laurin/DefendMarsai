using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using UnityEngine.UI;
using TMPro; 

enum State{
        PLAYER_TURN = 1, 
        ENEMY_TURN = 2, 
        OTHER_TURN = 3, 
        DIALOGUE = 4 
    }; 

public class GameManager : MonoBehaviour
{

    public GameObject tile; 
    [SerializeField] private int _width, _height; 
    [SerializeField] private Transform _cam; 
    [SerializeField] private GameObject _pawn; 
    [SerializeField] private GameObject _enemy; 
    [SerializeField] private int _enemyCount = 2; 

    private GameObject _selectedPawn; 
    private List<List<GameObject>> _map = new List<List<GameObject>>(); 
    [SerializeField] private List<GameObject> portraits = new List<GameObject>(); 
    [SerializeField] private Material _availableMat; 
    private List<Tile> _availableTiles; 

    //UI
    [SerializeField] private GameObject _strengthUI; 
    [SerializeField] private GameObject _speedUI;
    [SerializeField] private GameObject _defenseUI;
    [SerializeField] private GameObject _willUI;
    [SerializeField] private GameObject _fatigueUI;
    [SerializeField] private GameObject _hpUI; 
    [SerializeField] private GameObject _nameUI; 
    [SerializeField] private GameObject _portraitImage;
    [SerializeField] private GameObject _classImage;
    [SerializeField] private GameObject _turnTextUI; 

    //Vars
    private TextMeshProUGUI _turnText; 

    //State
    private State _state; 

    //Pawns
    private List<GameObject> _playerPawns = new List<GameObject>(); 
    private List<GameObject> _enemyPawns = new List<GameObject>(); 

    // Start is called before the first frame update
    void Start()
    {
        _width = 10; 
        _height = _width; 
        _turnText = _turnTextUI.GetComponent<TMPro.TextMeshProUGUI>(); 
        GenerateMap(); 
        InstantiateCamera(); 
        InstantiatePawns(); 
        InstantiateEnemies(); 
        PlayerTurn();   
        
    }

    private void InstantiatePawns(){
        var pawn = Instantiate(_pawn, new Vector3(0, tile.transform.position.y + 0.1f, 0), Quaternion.identity); 
        var pawnScript = pawn.GetComponent<Pawn>(); 
        pawnScript.Start(); 
        pawnScript.SetCurrentTile(_map[0][0].GetComponent<Tile>()); 
        pawnScript.SetPortrait(portraits[0]); 
        pawnScript.UpdateUI(); 
        _playerPawns.Add(pawn); 
    }

    private void InstantiateEnemies(){
        for(var i = 0; i<_enemyCount; i++){
            var x = _map.Count - 1 - i; 
            var z = _map[x].Count - 1 - i; 
            var enemy = Instantiate(_enemy, new Vector3(x, tile.transform.position.y + 0.1f, z), Quaternion.identity); 
            var enemyScript = enemy.GetComponent<Pawn>(); 
            enemyScript.Start(); 
            enemyScript.SetCurrentTile(_map[x][z].GetComponent<Tile>());
            enemyScript.SetPortrait(portraits[0]); 
            enemyScript.UpdateUI();
            enemyScript.SetAsEnemy(true);
            _enemyPawns.Add(enemy); 
        }
    }

    public void UpdatePortraitUI(int strength, int speed, int defense, int will, int fatigue, int hp, string name, Sprite portraitImage, Sprite classImage){
        _strengthUI.GetComponent<TMPro.TextMeshProUGUI>().text = strength.ToString(); 
        _speedUI.GetComponent<TMPro.TextMeshProUGUI>().text = speed.ToString(); 
        _defenseUI.GetComponent<TMPro.TextMeshProUGUI>().text = defense.ToString(); 
        _willUI.GetComponent<TMPro.TextMeshProUGUI>().text = will.ToString(); 
        _fatigueUI.GetComponent<Slider>().value = fatigue; 
        _hpUI.GetComponent<Slider>().value = hp; 
        _nameUI.GetComponent<TMPro.TextMeshProUGUI>().text = name; 
        _portraitImage.GetComponent<Image>().overrideSprite = portraitImage; 
        _classImage.GetComponent<Image>().overrideSprite = classImage; 
    }

    private void InstantiateCamera(){
        _cam.transform.position = new Vector3((float)_width/2 -0.5f, 8, -2); 
        _cam.transform.Rotate(60, 0, 0); 
    }

    private void GenerateMap(){
        for(int x = 0; x < _width; x++){
            _map.Add(new List<GameObject>()); 
            for(int z = 0; z < _height; z++){
                var spawnedTile = Instantiate(tile, new Vector3(x, 0.0f, z), Quaternion.identity); 
                spawnedTile.name = $"Tile {x} {z}"; 
                Tile spawnedTileComponent = spawnedTile.GetComponent<Tile>(); 
                spawnedTileComponent.SetXCoord(x); 
                spawnedTileComponent.SetZCoord(z); 
                _map[x].Add(spawnedTile); 
            }
        }
    }

    public void TileSelected(GameObject tile){
        //see if a pawn is selected 
        if(_selectedPawn){
            //if tile is selectable when a pawn is selected, move the pawn there
            var isEnemy = _selectedPawn.GetComponent<Pawn>().isEnemy(); 
            _selectedPawn.GetComponent<Pawn>().MoveToTile(tile); 
            DeHighlightTiles(); 
            if(!isEnemy){
                StartCoroutine(EnemyTurn()); 
            }
        }
    }

    public bool isPlayerTurn(){
        return _state == State.PLAYER_TURN; 
    }

    private IEnumerator AttackPlayer(){
        Debug.Log("Enemy Turn"); 
        yield return new WaitForSeconds(2); 
        Debug.Log("Enemy Finished"); 
    }
    
    private IEnumerator EnemyTurn(){
        _state = State.ENEMY_TURN; 
        _turnText.text = "Enemy Turn";  
        //move towards player pawns TODO advanced pathfinding here / search
        yield return StartCoroutine(AttackPlayer());  
        yield return new WaitForSeconds(1); 
        PlayerTurn(); 
    }

    private void PlayerTurn(){
        _state = State.PLAYER_TURN;
        _turnText.text = "Player Turn";  
        Debug.Log("Player Turn"); 
    }

    private List<Tile> FindNeighbors(Tile tile){
        List<Tile> neighbors = new List<Tile>(); 
        int x = tile.GetXCoord(); 
        int z = tile.GetZCoord(); 
        int right = x + 1; 
        int left = x - 1; 
        int up = z + 1; 
        int down = z - 1; 

        if(right < _map.Count){
            neighbors.Add(_map[right][z].GetComponent<Tile>());
        }
        if(left >= 0){
            neighbors.Add(_map[left][z].GetComponent<Tile>()); 
        }
        if(up < _map[x].Count){
            neighbors.Add(_map[x][up].GetComponent<Tile>()); 
        }
        if(down >= 0){
            neighbors.Add(_map[x][down].GetComponent<Tile>());
        }
        return neighbors; 
    }

    public void HighlightAvailableTiles(List<Tile> tiles){ 
        foreach(Tile tile in tiles){
            tile.ChangeTilesMat(_availableMat, 0); 
            tile.SetSelection(true); 
        }
    }

    public void DeHighlightTiles(){
        foreach(Tile tile in _availableTiles){
            tile.RevertToOriginalTilesMat(); 
            tile.SetSelection(false); 
        }
    }

    public void ShowAvailableMovement(GameObject pawn){
        _selectedPawn = pawn; 
        int movement = pawn.GetComponent<Pawn>().GetMovement(); 
        _availableTiles = PathFinding.Dijkstra(pawn.GetComponent<Pawn>().GetTile(), movement, FindNeighbors); 
        HighlightAvailableTiles(_availableTiles); 
    }

    public void DeselectedPawn(){
        DeHighlightTiles(); 
        _selectedPawn = null; 
    }
    
}
