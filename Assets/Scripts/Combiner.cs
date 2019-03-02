using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CombinerResult {
    public bool Success;
    public int LeftoverEnergy;
    public List<TileType> CreatedTiles;
}

public class Combiner {
    // Recipe declarations (is there a way to do this from a file instead of
    // hard-coded?)
    public Recipe[] recipes = new Recipe[]{
        // 1 circle + 1 square + 1 energy = 1 rounded square
        new Recipe{
            aTiles = new Dictionary<TileType, int>{
                {TileType.Circle, 1},
            },
            bTiles = new Dictionary<TileType, int>{
                {TileType.Square, 1},
            },
            produces = new List<TileType>{TileType.RoundedSquare},
            energyRequired = 1,
        },
        new Recipe{
            aTiles = new Dictionary<TileType, int>{
                {TileType.Square, 1},
            },
            bTiles = new Dictionary<TileType, int>{
                {TileType.Square, 1},
            },
            produces = new List<TileType>{TileType.Circle},
            energyRequired = 1,
        },
        new Recipe{
            aTiles = new Dictionary<TileType, int>{
                {TileType.Square, 1},
                {TileType.RoundedSquare, 1},
            },
            bTiles = new Dictionary<TileType, int>{
                {TileType.Circle, 1},
            },
            produces = new List<TileType>{TileType.Square, TileType.Square, TileType.Square},
            energyRequired = 2,
        },
        new Recipe{
            aTiles = new Dictionary<TileType, int>{
                {TileType.Square, 1},
            },
            bTiles = new Dictionary<TileType, int>{
                {TileType.Triangle, 1},
                {TileType.Square, 1},
                {TileType.Circle, 1},
            },
            produces = new List<TileType>{TileType.Square, TileType.Square, TileType.Square},
            energyRequired = 1,
        }
    };

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

public class Recipe {
    public int energyRequired;
    public Dictionary<TileType, int> aTiles;
    public Dictionary<TileType, int> bTiles;
    public List<TileType> produces;

    public bool IngredientsMatch(List<TileType> leftTiles, List<TileType> rightTiles, int energy) {
        if (energy < energyRequired) {
            return false;
        }

        var leftDict = buildIngredientDictFromList(leftTiles);
        var rightDict = buildIngredientDictFromList(rightTiles);


        // Check if the ingredients match in either order
        bool match = (dictsAreSame(aTiles, leftDict) && dictsAreSame(bTiles, rightDict));
        match = match || (dictsAreSame(bTiles, leftDict) && dictsAreSame(aTiles, rightDict));

        return match;
    }

    private Dictionary<TileType, int> buildIngredientDictFromList(List<TileType> ingredients) {
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