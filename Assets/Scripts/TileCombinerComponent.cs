using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {
    Square,
    Circle,
    RoundedSquare,

    Energy,

    Count,

    Invalid,
}

public enum Detector {
    Right,
    Left,
    Center,

    Count,
}

[CreateAssetMenu(fileName = "leveldata", menuName = "Level Data", order = 2)]
public class LevelData : ScriptableObject {
    public TileType[] tilesToSpawn;
    public TileType[] tilesToComplete;
}

public class TileCombinerComponent : MonoBehaviour {
    // CHRIS: This is where we'll put the logic for how to combine tiles
    // We'll have triggers for which two tiles (more than two?) are in the correct slots
    // THen we'll enumerate all the combinations

    // This is 1-to-1 with the types above
    public GameObject[] tilePrefabs;

    public GameObject[] detectors;

    private int levelIndex;
    public LevelData[] levels;

    void Start(){
        levelIndex = 0;
        // setup first level?

        if(tilePrefabs.Length != (int) TileType.Count){ Debug.LogError("TilePrefabs is wrong length"); }
        if(detectors.Length != (int) Detector.Count){ Debug.LogError("Detectors is wrong length"); }
    }

    void Update(){
        // If either one of the detectors contains a tile AND the energy detector has an energy ball, do the combination
        // Energy is the *sorta* the go button, then, right? I'm kind of ok with that
        for(int i = 0; i < (int) Detector.Count; ++i){
            // How are we going to handle complex recipe matching?
            // i.e. square square square + triangle + energy ball = something
            // or is it always 1 shape + 1 shape + energy = something

            // Old tiles are destroyed - ooh, ahh, effects
            // New tile is instantiated according to CombineTiles, and dropped into the scene
        }

        // If all tiles in the scene match the correct types for "success", then we proceed
        // We should probably have a list of "starting tiles", "starting energy", and "requirements", and an index into that table representing the "level" we're on
        // Then on success, play effects and increment that index, then set up the next level
    }

    public TileType CombineTiles(TileType a, TileType b, int energy){
        return TileType.Square;
    }
}
