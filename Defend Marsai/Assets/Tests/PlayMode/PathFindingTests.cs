using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PathFindingTests
{
    private List<List<GameObject>> _map; 
    private List<Tile> FindNeighborsFunc(Tile tile, Tile goal = null){
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

    private BattleSystem Setup(){
        var manager = new GameObject(); 
        manager.AddComponent<GameManager>(); 
        manager.AddComponent<UIManager>(); 
        BattleSystem battleSystem = manager.AddComponent<BattleSystem>(); 
        return battleSystem; 
    }
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator DijkstraAvailableTiles()
    {
        BattleSystem battleSystem = Setup(); 
        //Pawn pawn = new Pawn("testPawn", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0); 
        Tile start = battleSystem.GetTile(0, 0); 
       // pawn.SetCurrentTile(start); 
        battleSystem.GetAvailableTiles(start, 0); 
        //Testing if choosing all the tiles in range
        //range 0 
        int movement = 0; 
        List<Tile> path = PathFinding.DijkstraAvailableTiles(start, movement, battleSystem.FindNeighbors);
        Assert.AreEqual(path, null); 

        //range 1 == 4 tiles
        movement = 1; 
        Debug.Log($"Test 2: Start - {start} Movement - {movement}"); 
        path = new List<Tile>(PathFinding.DijkstraAvailableTiles(start, movement, FindNeighborsFunc));
        Debug.Log("Printing path:");
        Print.List(path); 
        Debug.Log($"Path count {path.Count}"); 
        Assert.AreEqual(4, path.Count); 

        //range 2 == 12 tiles
        movement = 12; 
        path = PathFinding.DijkstraAvailableTiles(start, movement, FindNeighborsFunc);
        Assert.AreEqual(12, path.Count); 

        //range 3 == 24 tiles
        movement = 24; 
        path = PathFinding.DijkstraAvailableTiles(start, movement, FindNeighborsFunc);
        Assert.AreEqual(24, path.Count); 
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
