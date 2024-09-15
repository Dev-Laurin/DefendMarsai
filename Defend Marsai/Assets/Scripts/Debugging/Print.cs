using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Print
{
    public static void Path<T>(Queue<T> path){
        if(path == null || path.Count <= 0){
            return; 
        }

        Queue<T> copy = new Queue<T>(path); 
        for(int i=0; i<copy.Count; i++){
            Debug.Log(copy.Dequeue());
        }
    }

    // public static void Path(PriorityQueue<T> path){
    //     if(path == null || path.Count <= 0){
    //         return; 
    //     }

    //     PriorityQueue<Tile> copy = new PriorityQueue<Tile>(path); 
    //     for(int i=0; i<copy.Count; i++){
    //         Debug.Log(copy.Dequeue());
    //     }
    // }

    public static string List<T>(List<T> list){
        foreach(T item in list){
            Debug.Log(item); 
        }
        return null; 
    }
    
}
