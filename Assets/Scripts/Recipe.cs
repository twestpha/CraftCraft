using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "recipe", menuName = "Recipe Data", order = 3)]
public class Recipe : ScriptableObject {
    public TileType[] aTiles;
    public TileType[] bTiles;
    public int energyRequired = 1;
    public TileType[] produces;

    public bool IngredientsMatch(List<TileType> leftTiles, List<TileType> rightTiles, int energy) {
        if (energy < energyRequired) {
            return false;
        }

        var leftTilesArr = leftTiles.ToArray();
        var rightTilesArr = rightTiles.ToArray();

        bool match = TilesComparer.TilesMatch(leftTilesArr, aTiles) && TilesComparer.TilesMatch(rightTilesArr, bTiles);
        match = match || TilesComparer.TilesMatch(rightTilesArr, aTiles) && TilesComparer.TilesMatch(leftTilesArr, bTiles);
        return match;
    }

}