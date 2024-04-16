using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Print
{
    public static void Path(Queue<Tile> path){
        if(path == null || path.Count <= 0){
            return; 
        }

        Queue<Tile> copy = new Queue<Tile>(path); 
        for(int i=0; i<copy.Count; i++){
            Debug.Log(copy.Dequeue());
        }
    }
    
}
