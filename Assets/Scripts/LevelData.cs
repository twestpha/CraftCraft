using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "leveldata", menuName = "Level Data", order = 2)]
public class LevelData : ScriptableObject {
    public TileType[] tilesToSpawn;
    public TileType[] tilesToComplete;
}
