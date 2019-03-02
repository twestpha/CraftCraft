using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {
    Energy,

    Square,
    Circle,
    RoundedSquare,
    Triangle,
    Donut,
    Star,

    Count,

    Invalid,
}

public enum Detector {
    Left,
    Center,
    Right,

    Count,
}

public class TileCombinerComponent : MonoBehaviour {
    // CHRIS: This is where we'll put the logic for how to combine tiles
    // We'll have triggers for which two tiles (more than two?) are in the correct slots
    // THen we'll enumerate all the combinations

    // This is 1-to-1 with the types above
    public GameObject[] tilePrefabs;

    public GameObject[] detectors;

    public Vector3[] tileSpawnBox;

    public Recipe[] recipes;

    private int levelIndex;
    public LevelData[] levels;

    private Combiner combiner;
    private Timer winCheckTimer;
    private Timer setupNewLevelTimer;
    private Timer recipeCheckTimer;
    private float secondsBeforeWinCheck = 3.5f;
    private float newLevelDelay = 0.5f;
    private float recipeCheckDelay = 0.6f;

    void Start(){
        levelIndex = 0;
        SetupCurrentLevel();

        if(tilePrefabs.Length != (int) TileType.Count){ Debug.LogError("TilePrefabs is wrong length"); }
        if(detectors.Length != (int) Detector.Count){ Debug.LogError("Detectors is wrong length"); }

        combiner = new Combiner(recipes);

        // So the recipe doesn't feel so "sudden" after it's submitted
        recipeCheckTimer = new Timer(recipeCheckDelay);
        recipeCheckTimer.Start();
    }

    void Update(){
        // Build state of the board

        CombinerResult result = new CombinerResult();
        result.Success = false;

        if(recipeCheckTimer.Finished()){
            recipeCheckTimer.Start();

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

            result = combiner.CombineTiles(
                leftTiles,
                rightTiles,
                suppliedEnergy
            );
        }

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
                Instantiate(
                    tilePrefabs[(int)t],
                    new Vector3(
                        Vector3.Lerp(tileSpawnBox[0], tileSpawnBox[1], Random.value).x,
                        2.4f,
                        Vector3.Lerp(tileSpawnBox[0], tileSpawnBox[1], Random.value).z
                    ),
                    Quaternion.Euler(-90.0f, 0.0f, 0.0f)
                );
            }

            // start winCheckTimer
            winCheckTimer = new Timer(secondsBeforeWinCheck);
            winCheckTimer.Start();
        }

        // If it's time, check for win condition
        if (winCheckTimer != null && winCheckTimer.Finished()) {
            if (DidWin()) {
                Debug.Log("You won!");
                TearDownCurrentLevel();
                levelIndex++;
                setupNewLevelTimer = new Timer(newLevelDelay);
                setupNewLevelTimer.Start();
            } else {
                Debug.Log("Did not win yet");
            }
            winCheckTimer = null;
        }

        if(setupNewLevelTimer != null && setupNewLevelTimer.Finished()){
            SetupCurrentLevel();
            setupNewLevelTimer = null;
        }
    }

    bool DidWin() {
        var tileComps = Object.FindObjectsOfType<TileComponent>();
        var tiles = new TileType[tileComps.Length];
        for (int i = 0; i < tiles.Length; i++) {
            tiles[i] = tileComps[i].type;
        }

        return TilesComparer.TilesMatch(tiles, levels[levelIndex].tilesToComplete);
    }

    void TearDownCurrentLevel() {
        if(levels == null || levels.Length == 0 || levels[levelIndex] == null){
            Debug.Log("no level to destroy, how did you even get here?");
            return;
        } // what the actual fuck?

        var tiles = Object.FindObjectsOfType<TileComponent>();
        foreach (var t in tiles) {
            t.DestroyTile(true);
        }
    }

    void SetupCurrentLevel(){
        if(levels == null || levels.Length == 0 || levels[levelIndex] == null){
            Debug.Log("no level to load :(");
            return;
        } // what the fuck?

        TileType[] toSpawn = levels[levelIndex].tilesToSpawn;

        for(int i = 0; i < toSpawn.Length; ++i){
            Instantiate(
                tilePrefabs[(int) toSpawn[i]],
                new Vector3(
                    Vector3.Lerp(tileSpawnBox[0], tileSpawnBox[1], Random.value).x,
                    1.0f + (3.0f * Random.value),
                    Vector3.Lerp(tileSpawnBox[0], tileSpawnBox[1], Random.value).z
                ),
                Quaternion.Euler(-90.0f, 0.0f, 0.0f)
            );
        }
    }
}
