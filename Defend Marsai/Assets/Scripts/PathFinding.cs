using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFinding
{

    public delegate List<Tile> FindNeighborsFunction(Tile tile);

    public static List<Tile> Dijkstra(Tile start, int movement, FindNeighborsFunction FindNeighbors){
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
}
