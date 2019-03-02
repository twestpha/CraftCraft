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

    public void DestroyAllTiles() {
        for (int i = 0; i < tiles.Length; i++) {
            Destroy(tiles[i]);
            tiles[i] = null;
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

    public List<TileType> getAllValidTiles() {
        var tiles = new List<TileType>();
        for (int j = 0; j < this.tiles.Length; j++) {
            if (this.tiles[j] != null) {
                TileComponent tc = this.tiles[j].GetComponent<TileComponent>();
                if (tc != null && tc.type != TileType.Invalid) {
                    tiles.Add(tc.type);
                }
            }
        }
        return tiles;
    }

    public int getNumEnergyTiles() {
        int numEnergy = 0;
        for (int j = 0; j < tiles.Length; j++) {
            if (tiles[j] != null) {
                TileComponent tc = tiles[j].GetComponent<TileComponent>();
                if (tc != null && tc.type == TileType.Energy) {
                    numEnergy++;
                }
            }
        }
        return numEnergy;
    }
}
