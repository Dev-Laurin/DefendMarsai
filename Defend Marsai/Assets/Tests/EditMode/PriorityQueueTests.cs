using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System; 

public class PriorityQueueTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void LowestPriorityAtTop()
    {
        PriorityQueue<string> pq = new PriorityQueue<string>(); 
        pq.Enqueue("mid", 5); 
        pq.Enqueue("low", -1); 
        pq.Enqueue("high", 10); 

        string lowest = pq.Dequeue(); 
        Assert.AreEqual(lowest, "low"); 
    }

    [Test]
    public void Enqueue()
    {
        PriorityQueue<string> pq = new PriorityQueue<string>(); 
        pq.Enqueue("mid", 5); 

        string elem = pq.Dequeue(); 
        Assert.AreEqual(elem, "mid");
    }

    [Test]
    public void Dequeue()
    {
        PriorityQueue<string> pq = new PriorityQueue<string>(); 
        pq.Enqueue("mid", 5); 
        pq.Enqueue("low", -1); 
        pq.Enqueue("high", 10); 

        string lowest = pq.Dequeue(); 
        string mid = pq.Dequeue(); 
        string high = pq.Dequeue(); 
        Assert.AreEqual(lowest, "low"); 
        Assert.AreEqual(mid, "mid"); 
        Assert.AreEqual(high, "high"); 

        Assert.Throws<Exception>(() => pq.Dequeue()); 
    }
}
