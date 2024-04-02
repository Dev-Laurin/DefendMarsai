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
    }

    private void InstantiateCamera(){
        _cam.transform.position = new Vector3((float)_width/2 -0.5f, 8, -2); 
        _cam.transform.Rotate(60, 0, 0); 
    }

    private void GenerateMap(){
        for(int x = 0; x < _width; x++){
            Debug.Log(x); 
            _map.Add(new List<GameObject>()); 
            for(int z = 0; z < _height; z++){
                var spawnedTile = Instantiate(tile, new Vector3(x, 0.0f, z), Quaternion.identity); 
                spawnedTile.name = $"Tile {x} {z}"; 
                Tile spawnedTileComponent = spawnedTile.GetComponent<Tile>(); 
                spawnedTileComponent.SetXCoord(x); 
                spawnedTileComponent.SetZCoord(z); 
                Debug.Log(spawnedTile); 
                Debug.Log(_map.Count); 

                _map[x].Add(spawnedTile); 
                
            }
        }

        Debug.Log(_map[_width - 1]); 
    }

    public void TileSelected(GameObject tile){
        //see if a pawn is selected 
        if(_selectedPawn){
            Debug.Log($"tile is chosen {tile}"); 
            //if tile is selectable when a pawn is selected, move the pawn there
            _selectedPawn.GetComponent<Pawn>().MoveToTile(tile); 
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
        
        try{
            neighbors.Add(_map[right][z].GetComponent<Tile>()); 
            //Debug.Log($"Neighbor: {right}, {z}"); 
        }
        catch(Exception e){
            //doesn't exist, keep going
        }

        try{
            neighbors.Add(_map[left][z].GetComponent<Tile>()); 
            //Debug.Log($"Neighbor: {left}, {z}"); 
        }
        catch(Exception e){

        }
        
        try{
            neighbors.Add(_map[x][down].GetComponent<Tile>());
            //Debug.Log($"Neighbor: {x}, {down}"); 
        }
        catch(Exception e){

        }
        
        try{
            neighbors.Add(_map[x][up].GetComponent<Tile>()); 
            //Debug.Log($"Neighbor: {x}, {up}"); 
        }
        catch(Exception e){

        }
        return neighbors; 
    }

    private PriorityQueue<Tile> _priorityQueue = new PriorityQueue<Tile>(); 
    private Dictionary<Tile, int> _costToReachTile = new Dictionary<Tile, int>(); 
    private Dictionary<Tile, Tile> nextTileToGoal = new Dictionary<Tile, Tile>();  

    private List<Tile> Dijkstra(Tile start, int movement){

        //Our starting point costs nothing
        var startNeighbors = FindNeighbors(start); 
        if(startNeighbors.Count < 0){
            return null; 
        }
        _priorityQueue.Enqueue(start, 0); 
        _costToReachTile[start] = 0; 

        //
        while(_priorityQueue.Count > 0){
            Tile currentTile = _priorityQueue.Dequeue(); 
            Debug.Log($"Evaluating current tile {currentTile} "); 

            foreach(Tile neighbor in FindNeighbors(currentTile)){
                 
                int newCost = _costToReachTile[currentTile] + neighbor.GetCost(); 
                Debug.Log($"Found neighbors, cost: {newCost}");
                if((_costToReachTile.ContainsKey(neighbor) == false || newCost < _costToReachTile[neighbor] || currentTile == start) && newCost <= movement){
                    Debug.Log($"Adding neighbor to path {neighbor}"); 
                    _costToReachTile[neighbor] = newCost; 
                    int priority = newCost; 
                    _priorityQueue.Enqueue(neighbor, priority); 
                    nextTileToGoal[neighbor] = currentTile; 
                    //neighbor._text.text = _costToReachTile[neighbor].ToString(); 
                }
            }
        }

        return new List<Tile>(nextTileToGoal.Keys); 
    }

    public void HighlightAvailableTiles(List<Tile> tiles){ 
        foreach(Tile tile in tiles){
            Debug.Log(tile); 
            tile.ChangeTilesMat(_availableMat, 0); 
            tile.SetSelection(true); 
        }
    }

    public void ShowAvailableMovement(GameObject pawn){
        _selectedPawn = pawn; 
        int movement = pawn.GetComponent<Pawn>().GetMovement(); 
        List<Tile> tiles = Dijkstra(pawn.GetComponent<Pawn>().GetTile(), movement); 
        Debug.Log(tiles); 
        HighlightAvailableTiles(tiles); 
    }

    
}
