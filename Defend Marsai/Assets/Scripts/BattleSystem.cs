using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State{
        PLAYER_TURN = 1, 
        ENEMY_TURN = 2, 
        OTHER_TURN = 3, 
        DIALOGUE = 4 
    }; 

public class BattleSystem : MonoBehaviour
{

    //prefabs
    [SerializeField] private GameObject tile;
    [SerializeField] private Transform _cam; 
    [SerializeField] private GameObject _pawn; 
    [SerializeField] private GameObject _enemy; 
    [SerializeField] private Material _availableMat; 

    //map
    [SerializeField] private int _width, _height = 10; 
    private List<List<GameObject>> _map = new List<List<GameObject>>(); 

    //pawns
    private List<GameObject> _playerPawns = new List<GameObject>(); 
    private List<GameObject> _enemyPawns = new List<GameObject>(); 
    private GameObject _selectedPawn;
    private List<Tile> _availableTiles;

    //state
    private State _state; 

    //enemy
    [SerializeField] private int _enemyCount = 1;

    public void StartBattle(){
        GenerateMap(); 
        InstantiateCamera(); 
        InstantiatePawns(); 
        InstantiateEnemies(); 
        PlayerTurn();  
    }

    private void InstantiatePawns(){
        var pawn = MonoBehaviour.Instantiate(_pawn, new Vector3(0, tile.transform.position.y + 0.1f, 0), Quaternion.identity); 
        var pawnScript = pawn.GetComponent<Pawn>(); 
        pawnScript.Start(); 
        pawnScript.SetCurrentTile(_map[0][0].GetComponent<Tile>()); 
        _playerPawns.Add(pawn); 
    }

    private void InstantiateEnemies(){
        for(var i = 0; i<_enemyCount; i++){
            var x = _map.Count - 1 - i; 
            var z = _map[x].Count - 1 - i; 
            var enemy = MonoBehaviour.Instantiate(_enemy, new Vector3(x, tile.transform.position.y + 0.1f, z), Quaternion.identity); 
            var enemyScript = enemy.GetComponent<Pawn>(); 
            enemyScript.Start(); 
            enemyScript.SetCurrentTile(_map[x][z].GetComponent<Tile>());
            enemyScript.UpdateUI();
            enemyScript.SetAsEnemy(true);
            _enemyPawns.Add(enemy); 
        }
    }

    private void InstantiateCamera(){
        _cam.transform.position = new Vector3((float)_width/2 -0.5f, 8, -2); 
        _cam.transform.Rotate(60, 0, 0); 
    }

    private void GenerateMap(){
        for(int x = 0; x < _width; x++){
            _map.Add(new List<GameObject>()); 
            for(int z = 0; z < _height; z++){
                var spawnedTile = MonoBehaviour.Instantiate(tile, new Vector3(x, 0.0f, z), Quaternion.identity); 
                spawnedTile.name = $"Tile {x} {z}"; 
                Tile spawnedTileComponent = spawnedTile.GetComponent<Tile>(); 
                spawnedTileComponent.SetXCoord(x); 
                spawnedTileComponent.SetZCoord(z); 
                _map[x].Add(spawnedTile); 
            }
        }
    }

    public State GetState(){
        return _state; 
    }

    public bool isPlayerTurn(){
        return isSelectable(); 
    }

    public GameObject TileToGameObject(Tile tile){
        return _map[tile.GetXCoord()][tile.GetZCoord()]; 
    }

    public List<Tile> FindNeighbors(Tile tile){
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

    public void DisplayOptions(Pawn pawn, List<Pawn> interactablePawns){
        var _uiManager = GameObject.Find("GameManager").GetComponent<GameManager>().GetUIManager();
        _uiManager.UpdateOptionsMenu(pawn, interactablePawns[0]); 
    }

    public bool isSelectable(){
        return _state == State.PLAYER_TURN; 
    }

    private List<Pawn> findInteractableUnits(GameObject selectedPawn){
        List<Pawn> unitsInRange = new List<Pawn>(); 

        List<GameObject> allPawns = _playerPawns; 
        allPawns.AddRange(_enemyPawns);  

        foreach(GameObject pawn in allPawns){
            if(selectedPawn.GetComponent<Pawn>().UnitInRange(pawn.GetComponent<Pawn>())){
                unitsInRange.Add(pawn.GetComponent<Pawn>()); 
            }
        }

        return unitsInRange; 
    }

    private IEnumerator EnemyTurn(){
        _state = State.ENEMY_TURN; 
        var _uiManager = GameObject.Find("GameManager").GetComponent<GameManager>().GetUIManager();
        _uiManager.UpdateTurnText("Enemy Turn");  
        //move towards player pawns TODO advanced pathfinding here / search
        foreach(GameObject pawn in _enemyPawns){
            yield return GameObject.Find("GameManager").GetComponent<GameManager>().StartCoroutine(AttackPlayer(pawn.GetComponent<Pawn>()));  
        }
        
        yield return new WaitForSeconds(1); 
        PlayerTurn(); 
    }

    private void PlayerTurn(){
        _state = State.PLAYER_TURN;
        var _uiManager = GameObject.Find("GameManager").GetComponent<GameManager>().GetUIManager();
        _uiManager.UpdateTurnText("Player Turn");  
        Debug.Log("Player Turn"); 
    }

    private IEnumerator AttackPlayer(Pawn pawn){
        //find player pawns we can do 'good' damage to and list them in a priority queue 
        Debug.Log("Enemy Turn");
        PriorityQueue<GameObject> targets = new PriorityQueue<GameObject>(); 
        foreach(GameObject playerPawn in _playerPawns){
            targets.Enqueue(playerPawn, pawn.GetComponent<Pawn>().CalculateTargetPriority(playerPawn.GetComponent<Pawn>())); 
        }
        
        Pawn pawnToAttack = targets.Dequeue().GetComponent<Pawn>();
        Queue<Tile> path = PathFinding.DijkstraWithGoal(pawn.GetComponent<Pawn>().GetTile(), pawnToAttack.GetTile(), pawn.GetComponent<Pawn>().GetMovement(), FindNeighbors);

        //Find a tile we can move to within the path 
        List<Tile> availableTiles = PathFinding.DijkstraAvailablePaths(pawn.GetComponent<Pawn>().GetTile(), pawn.GetComponent<Pawn>().GetMovement(), FindNeighbors); 
        
        int availableMovement = pawn.GetMovement(); 
        while(availableMovement > 0){
            Tile tile = path.Dequeue(); 
            int cost = tile.GetCost(); 

            if((availableMovement - cost) > 0){
                availableMovement -= cost; 
                yield return GameObject.Find("GameManager").GetComponent<GameManager>().StartCoroutine(nameof(pawn.MoveToTile), TileToGameObject(tile)); 
                yield return new WaitForSeconds(1); 
            }
        }

        if(path.Count <= 0){
            //we can attack the unit
            if(pawnToAttack){
                pawnToAttack.TakeDamage(pawn.GetStrength()); 
            }
        }
        Debug.Log("Enemy Finished"); 
    }

    public void RemoveUnit(GameObject pawn){
        Debug.Log($"Removing {pawn}"); 
        if(pawn.GetComponent<Pawn>().isEnemy()){
            _enemyPawns.Remove(pawn);
        }
        else{
            _playerPawns.Remove(pawn); 
        }
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
        _availableTiles = PathFinding.DijkstraAvailableTiles(pawn.GetComponent<Pawn>().GetTile(), movement, FindNeighbors); 
        HighlightAvailableTiles(_availableTiles); 
    }

    public void DeselectedPawn(){
        DeHighlightTiles(); 
        _selectedPawn = null; 
    }

    public void TileSelected(GameObject tile){
        //see if a pawn is selected 
        if(_selectedPawn){
            //if tile is selectable when a pawn is selected, move the pawn there
            var isEnemy = _selectedPawn.GetComponent<Pawn>().isEnemy(); 
            GameObject pawn = _selectedPawn; 
            _selectedPawn.GetComponent<Pawn>().MoveToTile(tile); 
            DeHighlightTiles(); 
            List<Pawn> interactableUnits = findInteractableUnits(pawn);
            Debug.Log($"Num of interactable units {interactableUnits.Count}"); 
            if(interactableUnits.Count > 0){
                var _uiManager = GameObject.Find("GameManager").GetComponent<GameManager>().GetUIManager();
                _uiManager.UpdateOptionsMenu(pawn.GetComponent<Pawn>(), interactableUnits[0]); 
            }
            
            if(!isEnemy){
                GameObject.Find("GameManager").GetComponent<GameManager>().StartCoroutine(EnemyTurn()); 
            }
        }
    }

}
