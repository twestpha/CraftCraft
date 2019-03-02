using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesComparer {
    public static bool TilesMatch(TileType[] aTiles, TileType[] bTiles) {
        var aDict = buildIngredientDictFromArray(aTiles);
        var bDict = buildIngredientDictFromArray(bTiles);
        return dictsAreSame(aDict, bDict);
    }

    static Dictionary<TileType, int> buildIngredientDictFromArray(TileType[] ingredients) {
        var dict = new Dictionary<TileType, int>();
        foreach(var ingredient in ingredients) {
            if (dict.ContainsKey(ingredient)) {
                dict[ingredient]++;
            } else {
                dict[ingredient] = 1;
            }
        }
        return dict;
    }

    static bool dictsAreSame(Dictionary<TileType, int> a, Dictionary<TileType, int> b) {
        if (a.Count != b.Count) {
            return false;
        }

        foreach(var entry in a) {
            if (!b.ContainsKey(entry.Key)) {
                return false;
            }
            if ( a[entry.Key] != b[entry.Key] ) {
                return false;
            }
        }

        return true;
    }
}