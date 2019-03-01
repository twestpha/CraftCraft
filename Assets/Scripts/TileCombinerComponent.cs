using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {
    Square,
    Circle,
    RoundedSquare
}

public class TileCombinerComponent : MonoBehaviour {
    // CHRIS: This is where we'll put the logic for how to combine tiles
    // We'll have triggers for which two tiles (more than two?) are in the correct slots
    // THen we'll enumerate all the combinations

    void Start(){

    }

    void Update(){

    }

    public TileType CombineTiles(TileType a, TileType b, int energy){
        return TileType.Square;
    }
}
