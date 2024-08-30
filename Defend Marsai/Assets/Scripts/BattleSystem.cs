using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State{
        PLAYER_TURN = 1, 
        ENEMY_TURN = 2, 
        OTHER_TURN = 3, 
        DIALOGUE = 4,
        WIN = 5,
        LOSE = 6
    }; 

public class BattleSystem : MonoBehaviour
{

    //prefabs
    [SerializeField] private GameObject _tile;
    [SerializeField] private Transform _cam; 

    [SerializeField] private List<GameObject> _startingPlayerPawns;
    [SerializeField] private List<GameObject> _startingEnemyPawns; 
    [SerializeField] private GameObject _pawn; 
    [SerializeField] private GameObject _pawn2; 
    [SerializeField] private GameObject _enemy; 
    [SerializeField] private Material _availableMat; 

    //map
    [SerializeField] private int _width = 10, _height = 10; 
    private List<List<GameObject>> _map; 
    private List<List<bool>> _mapPos; 

    //pawns
    private List<GameObject> _playerPawns; 
    private List<GameObject> _enemyPawns; 
    private GameObject _selectedPawn;
    private GameObject _selectedPawn2; 
    private List<Tile> _availableTiles;
    private GameObject _previousTile; 

    //state
    private State _state; 
    private bool _playerChoosingOptions = false; 
    private Dictionary<GameObject, int> unitsUsed = new Dictionary<GameObject, int>(); 

    //enemy
    [SerializeField] private int _enemyCount = 2;

    //systems
    private GameManager _gameManager; 
    private UIManager _uiManager; 


    public void StartBattle(){
        _map = new List<List<GameObject>>();
        _mapPos = new List<List<bool>>(); 
        _playerPawns = new List<GameObject>(); 
        _enemyPawns = new List<GameObject>(); 
        _gameManager = gameObject.GetComponent<GameManager>(); 
        _uiManager = _gameManager.GetUIManager(); 
        _tile = Resources.Load<GameObject>("Prefab/Tile");
        try{
            GenerateMap();
        }
        catch{
            Debug.Log("Using default cube to generate map. Prefab not set."); 
             
            //GenerateMap();
        }
        
        try{
            InstantiateCamera(); 
            InstantiatePawns(); 
            InstantiateEnemies(); 
            resetUnitsUsed(); 
            PlayerTurn();  
        }
        catch{
            Debug.Log("BattleSystem: Camera could not be found."); 
        }
    }

    private void resetUnitsUsed(){
        for(int i=0; i<_playerPawns.Count; i++){
            unitsUsed[_playerPawns[i]] = 1; 
        }
    }

    private void unitTurnEnd(GameObject unit){
        Debug.Log("ending unit's turn"); 
        unitsUsed.Remove(unit); 
        unit.GetComponent<Pawn>().Deselect(); 
        ResetVars(); 
    }

    public void RestartBattle(){
        removePawns(); 
        removeTiles(); 
        StartBattle(); 
    }

    private void removeTiles(){
        foreach(List<GameObject> list in _map){
            foreach(GameObject obj in list){
                Destroy(obj); 
            }
        }
    }

    private void removePawns(){
        ResetVars(); 
        foreach(GameObject obj in _playerPawns){
            Destroy(obj); 
        }
        foreach(GameObject obj in _enemyPawns){
            Destroy(obj); 
        }
        
    }

    private void InstantiatePawn(int x, int y, GameObject pawn_){
        var tile1 = _map[x][y]; 
        Debug.Log(tile1.transform.position); 
        var pawn = MonoBehaviour.Instantiate(pawn_, new Vector3(tile1.transform.position.x, _tile.transform.position.y + 0.1f, tile1.transform.position.z), Quaternion.identity); 
        var pawnScript = pawn.GetComponent<Pawn>(); 
        pawnScript.Start(); 
        pawnScript.SetCurrentTile(_map[x][y].GetComponent<Tile>()); 
        _mapPos[x][y] = true; 
        _playerPawns.Add(pawn);  
    }

    private void InstantiatePawns(){
        for(var i =0; i<_startingPlayerPawns.Count; i++){
            InstantiatePawn(i, 0, _startingPlayerPawns[i]); 
        }
    }

    private void InstantiateEnemies(){
        for(var i = 0; i<_enemyCount; i++){
            
            var x = _map.Count - 1 - i; 
            var z = _map[x].Count - 1 - i; 
            Debug.Log($"Creating enemy at {x} {z}"); 
            var enemy = MonoBehaviour.Instantiate(_enemy, new Vector3(x, _tile.transform.position.y + 0.1f, z), Quaternion.identity); 
            var enemyScript = enemy.GetComponent<Pawn>(); 
            enemyScript.Start(); 
            enemyScript.SetCurrentTile(_map[x][z].GetComponent<Tile>());
            _mapPos[x][z] = true; 
            enemyScript.SetAsEnemy(true);
            _enemyPawns.Add(enemy); 
        }

        // for(var i=0; i<_startingEnemyPawns.Count; i++){
        //     InstantiatePawn(i, _height - 1, _startingEnemyPawns[i]); 
        // }
    }
    private void InstantiateCamera(){
        if(_cam.transform.rotation.x == 0.5){
            return; 
        }
        _cam.transform.position = new Vector3((float)_width/2, 8, -2); 
        _cam.transform.Rotate(65, 0, 0); 
    }

    private void GenerateMap(){
        for(int x = 0; x < _width; x++){
            _map.Add(new List<GameObject>()); 
            _mapPos.Add(new List<bool>()); 
            for(int z = 0; z < _height; z++){
                var spawnedTile = MonoBehaviour.Instantiate(_tile, new Vector3(x, 0.0f, z), Quaternion.identity); 
                spawnedTile.name = $"Tile {x} {z}"; 
                Tile spawnedTileComponent = spawnedTile.GetComponent<Tile>(); 
                spawnedTileComponent.SetXCoord(x); 
                spawnedTileComponent.SetZCoord(z); 
                _map[x].Add(spawnedTile); 
                _mapPos[x].Add(false); 
            }
        }
    }

    public State GetState(){
        return _state; 
    }

    public bool AwaitingPlayerOption(){
        return _playerChoosingOptions; 
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

    //Player has selected a unit to perform an action on
    public void UnitSelected(GameObject pawn){
        _selectedPawn2 = pawn; 
        _uiManager.UpdateOptionsMenu(_selectedPawn.GetComponent<Pawn>(), _selectedPawn2.GetComponent<Pawn>()); 
    }

    public void DisplayOptions(Pawn pawn, List<Pawn> interactablePawns){
        _uiManager.UpdateOptionsMenu(pawn, interactablePawns[0]); 
    }

    public bool isSelectable(){
        return _state == State.PLAYER_TURN; 
    }

    public bool HighlightInteractableUnits(GameObject pawn){
        List<GameObject> units = findInteractableUnits(pawn); 
        bool moreActions = units.Count > 0; 
        if(moreActions && pawn.GetComponent<Pawn>().isEnemy()){
            units[0].GetComponent<Pawn>().Select(true); 
        }
        else if(moreActions){
            units[0].GetComponent<Pawn>().Select(); 
        }
        
        return moreActions; 
    }

    private List<GameObject> findInteractableUnits(GameObject selectedPawn){
        List<GameObject> unitsInRange = new List<GameObject>(); 
        List<GameObject> allPawns = new List<GameObject>(); 

        if(selectedPawn.GetComponent<Pawn>().isEnemy()){
            allPawns.AddRange(_playerPawns); 
        }
        else{
            allPawns.AddRange(_enemyPawns); 
        }
        
        foreach(GameObject pawn in allPawns){
            if(pawn != selectedPawn && selectedPawn.GetComponent<Pawn>().UnitInRange(pawn.GetComponent<Pawn>())){
                unitsInRange.Add(pawn); 
            }
        }
 
        return unitsInRange; 
    }

    private void ResetVars(){
        if(_selectedPawn){
             _selectedPawn.GetComponent<Pawn>().Deselect(); 
             _selectedPawn = null; 
        }

        if(_selectedPawn2){
            _selectedPawn2.GetComponent<Pawn>().Deselect(); 
            _selectedPawn2 = null; 
        }
        _playerChoosingOptions = false; 
        _uiManager.CloseMenus(); 
    }

    private IEnumerator EnemyTurn(){
        ResetVars(); 
        _state = State.ENEMY_TURN; 
        _uiManager.UpdateTurnText("Enemy Turn");  
        yield return new WaitForSeconds(1); 
        //move towards player pawns TODO advanced pathfinding here / search
        foreach(GameObject pawn in _enemyPawns){
            yield return StartCoroutine(AttackPlayer(pawn.GetComponent<Pawn>()));  
        }
        yield return new WaitForSeconds(1); 
        PlayerTurn(); 
    }

    public Tile GetTile(int x, int z){
        return _map[x][z].GetComponent<Tile>(); 
    }

    private void PlayerTurn(){
        ResetVars(); 
        _state = State.PLAYER_TURN;
        var _uiManager = _gameManager.GetUIManager();
        _uiManager.UpdateTurnText("Your Turn");  
    }

    private int CalculateTargetPriority(Pawn target, Pawn attacker){
        return target.EstimatedDamageTaken(attacker.GetStrength()); 
    }

    private bool TileTaken(Tile tile){
        Debug.Log("Tile is already used"); 
       return _mapPos[tile.GetXCoord()][tile.GetZCoord()]; 
    }
    private IEnumerator AttackPlayer(Pawn pawn){
        //find player pawns we can do 'good' damage to and list them in a priority queue 
        Debug.Log("Enemy Turn");
        PriorityQueue<GameObject> targets = new PriorityQueue<GameObject>(); 
        foreach(GameObject playerPawn in _playerPawns){
            targets.Enqueue(playerPawn, CalculateTargetPriority(playerPawn.GetComponent<Pawn>(), pawn.GetComponent<Pawn>())); 
        }
        
        Pawn pawnToAttack = targets.Dequeue().GetComponent<Pawn>();
        Tile pawnToAttackTile = pawnToAttack.GetTile(); 
        Queue<Tile> path = PathFinding.DijkstraWithGoal(pawn.GetComponent<Pawn>().GetTile(), pawnToAttackTile, pawn.GetComponent<Pawn>().GetMovement(), FindNeighbors);

        int pathCount = 0; 
        if(path != null){
            pathCount = path.Count; 
            int availableMovement = pawn.GetMovement(); 
            while(availableMovement > 0 && path.Count > 0){
                Tile tile = path.Dequeue(); 
                int cost = tile.GetCost(); 

                if(tile == pawnToAttackTile){
                    break; //we don't need to go on the actual tile
                }

                if((availableMovement - cost) >= 0){
                    availableMovement -= cost; 
                     if(TileTaken(tile)){
                         break; 
                     }
                    Tile previousTile = pawn.GetTile(); 
                    _mapPos[previousTile.GetXCoord()][previousTile.GetZCoord()] = false; 
                    pawn.MoveToTile(TileToGameObject(tile)); 
                    _mapPos[tile.GetXCoord()][tile.GetZCoord()] = true; 
                }
                else{
                    break; 
                }
            }
            yield return new WaitForSeconds(1); 
        }

        if(pawn.UnitInRange(pawnToAttack)){
            //we can attack the unit
            if(pawnToAttack){
                int damage = pawnToAttack.TakeDamage(pawn.GetStrength()); 
            }
        }

        pawn.IncreaseFatigue(1); 
        Debug.Log("Enemy Finished"); 
    }

    private void CheckEndState(){
        if(_playerPawns.Count <= 0){
            _state = State.LOSE; 
            _uiManager.ShowLoseUI(); 
        }
        else if(_enemyPawns.Count <= 0){
            _state = State.WIN; 
            _uiManager.ShowWinUI(); 
        }
    }

    public void RemoveUnit(GameObject pawn){
        Pawn unit = pawn.GetComponent<Pawn>(); 
        Tile tile = unit.GetTile();  
        _mapPos[tile.GetXCoord()][tile.GetZCoord()] = false; 
        if(unit.isEnemy()){
            _enemyPawns.Remove(pawn);  
        }
        else{
            _playerPawns.Remove(pawn); 
            unitTurnEnd(pawn); 
        }
        CheckEndState(); 
        Destroy(pawn);
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

    public bool UnitSelectable(GameObject unit){
        return unitsUsed.ContainsKey(unit); 
    }

    public void ShowAvailableMovement(GameObject pawn){
        _availableTiles = GetAvailableTilesFromGameObject(pawn);  
        HighlightAvailableTiles(_availableTiles); 
    }

    public List<Tile> GetAvailableTilesFromGameObject(GameObject pawn){
        _selectedPawn = pawn; 
        int movement = pawn.GetComponent<Pawn>().GetMovement(); 
        return GetAvailableTiles(pawn.GetComponent<Pawn>().GetTile(), movement); 
    }

    public List<Tile> GetAvailableTiles(Tile tile, int movement){
        _availableTiles = PathFinding.DijkstraAvailableTiles(tile, movement, FindNeighbors); 
        return _availableTiles; 
    }

    public void DeselectedPawn(){
        DeHighlightTiles(); 
        _selectedPawn = null; 
    }

    public bool InRange(GameObject pawn){
        if(_selectedPawn && isPlayerTurn()){
            return _selectedPawn.GetComponent<Pawn>().UnitInRange(pawn.GetComponent<Pawn>());
        }
        return false; 
    }

    public void ItemsMenu(){
        _uiManager.ShowItemsMenu(); 
    }

    public void TileSelected(GameObject tile){
        //see if a pawn is selected 
        if(_selectedPawn){
            //if tile is selectable when a pawn is selected, move the pawn there
            var isEnemy = _selectedPawn.GetComponent<Pawn>().isEnemy(); 
            GameObject pawn = _selectedPawn; 
            if(!isEnemy){
                Tile prevTile = _selectedPawn.GetComponent<Pawn>().GetTile(); 
                
                _previousTile = tile; 
                Tile t = tile.GetComponent<Tile>();

                if(!TileTaken(tile.GetComponent<Tile>())){
                    _mapPos[prevTile.GetXCoord()][prevTile.GetZCoord()] = false; 
                    _selectedPawn.GetComponent<Pawn>().MoveToTile(tile); 
                    _mapPos[t.GetXCoord()][t.GetZCoord()] = true; 
                    List<GameObject> interactableUnits = findInteractableUnits(pawn);
                    if(interactableUnits.Count > 0){
                        _playerChoosingOptions = true; 
                        _uiManager.UpdateOptionsMenu(pawn.GetComponent<Pawn>(), interactableUnits[0].GetComponent<Pawn>()); 
                    }
                }
                else{
                    DeHighlightTiles(); 
                    return; 
                }
                
                
            }
            
            DeHighlightTiles(); 

            if(isEnemy){
                _selectedPawn.GetComponent<Pawn>().Deselect(); 
            }
            
            
            if(!isEnemy && !_playerChoosingOptions){
                unitTurnEnd(pawn); 
                EndPlayerTurn(); 
            }
        }
    }
    
    private void EndPlayerTurn(){
        if(isPlayerTurn() && unitsUsed.Count < 1){
            Debug.Log("Player turn has ended."); 
            resetUnitsUsed(); 
            StartCoroutine(EnemyTurn());
        }
        Debug.Log("Player has not moved all units."); 
    }

    public IEnumerator AttackButtonPressed(){
        if(_state != State.PLAYER_TURN ){
            //accidental over-press, ignore
            yield break; 
        }
        else{
            Pawn otherUnit = _selectedPawn2.GetComponent<Pawn>(); 
            yield return otherUnit.TakeDamage(_selectedPawn.GetComponent<Pawn>().GetStrength());
            _uiManager.DisplayOptions(false); 
            unitTurnEnd(_selectedPawn); 
            EndPlayerTurn(); 
        }
    }

    public void EndUnitTurnButtonPressed(){
        _uiManager.DisplayOptions(false); 
        _uiManager.ShowNoAttackActionsMenu(false); 
        unitTurnEnd(_selectedPawn); 
        EndPlayerTurn(); 
    }

    public void PlayerFinishedUnit(GameObject pawn){
        _uiManager.ShowNoAttackActionsMenu(false); 
        EndPlayerTurn(); 
    }

    public void CancelAction(){
        Debug.Log("Cancelling action"); 
        Debug.Log(_selectedPawn.GetComponent<Pawn>().GetName()); 
        _uiManager.DisplayOptions(false); 
        Pawn pawn = _selectedPawn.GetComponent<Pawn>();
        pawn.Deselect(); 
        _playerChoosingOptions = false; 
        pawn.MoveBackToTile(); 
    }

}
