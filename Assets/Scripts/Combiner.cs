using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CombinerResult {
    public bool Success;
    public int LeftoverEnergy;
    public TileType[] CreatedTiles;
}

public class Combiner {
    private Recipe[] recipes;
    public Combiner(Recipe[] recipes) {
        this.recipes = recipes;
    }

    public CombinerResult CombineTiles(List<TileType> left, List<TileType> right, int energy){
        // Fail early if there are no tiles on one of the sides
        if (left.Count == 0 || right.Count == 0) {
            return new CombinerResult{
                Success = false,
            };
        }

        // Check tiles against known recipes
        foreach (var recipe in recipes) {
            if (recipe.IngredientsMatch(left, right, energy)) {
                Debug.Log("matched recipe: " + recipe);
                return new CombinerResult{
                    Success = true,
                    LeftoverEnergy = energy - recipe.energyRequired,
                    CreatedTiles = recipe.produces,
                };
            }
        }

        return new CombinerResult{
            Success = false,
        };
    }
}

[CreateAssetMenu(fileName = "recipe", menuName = "Recipe Data", order = 3)]
public class Recipe : ScriptableObject {
    public TileType[] aTiles;
    public TileType[] bTiles;
    public int energyRequired = 1;
    public TileType[] produces;

    private Dictionary<TileType, int> aTilesDict;
    private Dictionary<TileType, int> bTilesDict;

    public bool IngredientsMatch(List<TileType> leftTiles, List<TileType> rightTiles, int energy) {
        // TODO Is there a way to initialize these on level load instead of lazy initing?
        lazyInitTilesDicts();

        if (energy < energyRequired) {
            return false;
        }

        var leftDict = buildIngredientDictFromArray(leftTiles.ToArray());
        var rightDict = buildIngredientDictFromArray(rightTiles.ToArray());

        // Check if the ingredients match in either order
        bool match = (dictsAreSame(aTilesDict, leftDict) && dictsAreSame(bTilesDict, rightDict));
        match = match || (dictsAreSame(bTilesDict, leftDict) && dictsAreSame(aTilesDict, rightDict));

        return match;
    }

    private void lazyInitTilesDicts() {
        if (aTilesDict == null) {
            aTilesDict = buildIngredientDictFromArray(aTiles);
        }
        if (bTilesDict == null) {
            bTilesDict = buildIngredientDictFromArray(bTiles);
        }
    }

    private Dictionary<TileType, int> buildIngredientDictFromArray(TileType[] ingredients) {
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
