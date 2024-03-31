using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject tile; 
    [SerializeField] private int _width, _height; 
    [SerializeField] private Transform _cam; 
    [SerializeField] private GameObject _player; 

    // Start is called before the first frame update
    void Start()
    {
        _width = 10; 
        _height = _width; 
        GenerateMap(); 
        InstantiateCamera(); 
        InstantiatePlayer(); 
    }

    private void InstantiatePlayer(){
        Instantiate(_player, new Vector3(0, tile.transform.position.y + 0.1f, 0), Quaternion.identity); 
    }

    private void InstantiateCamera(){
        _cam.transform.position = new Vector3((float)_width/2 -0.5f, 8, -2); 
        _cam.transform.Rotate(60, 0, 0); 
    }

    private void GenerateMap(){
        for(int x = 0; x < _width; x++){
            for(int z = 0; z < _height; z++){
                var spawnedTile = Instantiate(tile, new Vector3(x, 0.0f, z), Quaternion.identity); 
                spawnedTile.name = $"Tile {x} {z}"; 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
