using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorComponent : MonoBehaviour {
    public const int MAX_TILE_COUNT = 8;

    public GameObject[] tiles;

    void Start(){
        tiles = new GameObject[MAX_TILE_COUNT];
    }

    void Update(){

    }

    // on collision with tiles, add them to list
    void OnTriggerEnter(Collider other){
        TileComponent tile = other.gameObject.GetComponent<TileComponent>();

        if(tile){
            for(int i = 0; i < MAX_TILE_COUNT; ++i){
                if(tiles[i] == null){
                    tiles[i] = other.gameObject;
                    break;
                }
            }
        }
    }

    // on collision end with tiles, remove them from list
    void OnTriggerExit(Collider other){
        for(int i = 0; i < MAX_TILE_COUNT; ++i){
            if(tiles[i] == other.gameObject){
                tiles[i] = null;
            }
        }
    }
}
