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
    Left,
    Center,
    Right,

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

    public Vector3 newTileSpawnPoint;

    private int levelIndex;
    public LevelData[] levels;

    void Start(){
        levelIndex = 0;
        // setup first level?

        if(tilePrefabs.Length != (int) TileType.Count){ Debug.LogError("TilePrefabs is wrong length"); }
        if(detectors.Length != (int) Detector.Count){ Debug.LogError("Detectors is wrong length"); }
    }

    void Update(){
        // Build state of the board
        List<TileType> leftTiles = new List<TileType>();
        List<TileType> rightTiles = new List<TileType>();
        int suppliedEnergy = 0;

        for(int i = 0; i < (int) Detector.Count; ++i){
            DetectorComponent d = detectors[i].GetComponent<DetectorComponent>();
            switch ((Detector)i) {
            case Detector.Center:
                suppliedEnergy = d.getNumEnergyTiles();
                break;
            case Detector.Left:
                leftTiles = d.getAllValidTiles();
                break;
            case Detector.Right:
                rightTiles = d.getAllValidTiles();
                break;
            };
        }

        CombinerResult result = new Combiner().CombineTiles(
            leftTiles,
            rightTiles,
            suppliedEnergy
        );

        if (result.Success) {
            // Old tiles are destroyed - ooh, ahh, effects
            for(int i = 0; i < (int) Detector.Count; ++i){
                detectors[i].GetComponent<DetectorComponent>().DestroyAllTiles();
            }

            // Spawn new thangs
            var tilesToSpawn = new List<TileType>();
            for (int i = 0; i < result.LeftoverEnergy; i++) {
                tilesToSpawn.Add(TileType.Energy);
            }
            tilesToSpawn.AddRange(result.CreatedTiles);

            foreach(var t in tilesToSpawn) {
                Instantiate(tilePrefabs[(int)t], newTileSpawnPoint, Quaternion.identity);
            }
        }

        // TODO Check for win condition

        // We should probably have a list of "starting tiles", "starting energy", and "requirements", and an index into that table representing the "level" we're on
        // Then on success, play effects and increment that index, then set up the next level
    }
}

