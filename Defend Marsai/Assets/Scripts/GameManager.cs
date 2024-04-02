using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class GameManager : MonoBehaviour
{

    public GameObject tile; 
    [SerializeField] private int _width, _height; 
    [SerializeField] private Transform _cam; 
    [SerializeField] private GameObject _player; 

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
    [SerializeField] private GameObject _name; 

    // Start is called before the first frame update
    void Start()
    {
        _width = 10; 
        _height = _width; 
        GenerateMap(); 
        InstantiateCamera(); 
        InstantiatePawns(); 
    }

    private void InstantiatePawns(){
        var pawn = Instantiate(_player, new Vector3(0, tile.transform.position.y + 0.1f, 0), Quaternion.identity); 
        var pawnScript = pawn.GetComponent<Pawn>(); 
        pawnScript.SetCurrentTile(_map[0][0].GetComponent<Tile>()); 
        pawnScript.SetPortrait(portraits[0]); 
        pawnScript.SetUI(_strengthUI, _speedUI, _defenseUI, _willUI, _fatigueUI, _hpUI, _name); 
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
            _selectedPawn.GetComponent<Pawn>().MoveToTile(tile); 
            DeHighlightTiles(); 
        }
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

    

    private List<Tile> Dijkstra(Tile start, int movement){
        PriorityQueue<Tile> _priorityQueue = new PriorityQueue<Tile>(); 
        Dictionary<Tile, int> _costToReachTile = new Dictionary<Tile, int>(); 
        Dictionary<Tile, Tile> nextTileToGoal = new Dictionary<Tile, Tile>();  

        //Our starting point costs nothing
        var startNeighbors = FindNeighbors(start); 
        if(startNeighbors.Count < 0){
            return null; 
        }
        _priorityQueue.Enqueue(start, 0); 
        _costToReachTile[start] = 0; 

        while(_priorityQueue.Count > 0){
            Tile currentTile = _priorityQueue.Dequeue(); 

            foreach(Tile neighbor in FindNeighbors(currentTile)){
                 
                int newCost = _costToReachTile[currentTile] + neighbor.GetCost(); 
                if((_costToReachTile.ContainsKey(neighbor) == false || newCost < _costToReachTile[neighbor] || currentTile == start) && newCost <= movement){
                    _costToReachTile[neighbor] = newCost; 
                    int priority = newCost; 
                    _priorityQueue.Enqueue(neighbor, priority); 
                    nextTileToGoal[neighbor] = currentTile; 
                }
            }
        }

        return new List<Tile>(nextTileToGoal.Keys); 
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
        _availableTiles = Dijkstra(pawn.GetComponent<Pawn>().GetTile(), movement); 
        HighlightAvailableTiles(_availableTiles); 
    }

    public void DeselectedPawn(){
        DeHighlightTiles(); 
        _selectedPawn = null; 
    }
    
}
