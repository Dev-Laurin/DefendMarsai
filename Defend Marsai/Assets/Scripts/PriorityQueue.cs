using System.Collections;
using System.Collections.Generic;
using System; 

public class PriorityQueue<T>
{
    // public PriorityQueue<T>(PriorityQueue<T> path){
    //     int priority = path.Count; 
    //     while(path.Count > 0){
    //         T item = path.Dequeue(); 
    //         this.Enqueue(item, priority);
    //         priority = priority - 1;  
    //     }
    // }
    private List<Tuple<T, int>> elements = new List<Tuple<T, int>>(); 

    public int Count{
        get {return elements.Count;}
    }

    public void Enqueue(T item, int priority){
        elements.Add(Tuple.Create(item, priority)); 
    }

    public T Dequeue(){
        int bestIndex = 0; 

        for(int i = 0; i< elements.Count; i++){
            if(elements[i].Item2 < elements[bestIndex].Item2){
                bestIndex = i; 
            }
        } 

        T bestItem = elements[bestIndex].Item1; 
        elements.RemoveAt(bestIndex); 
        return bestItem; 
    }
}
