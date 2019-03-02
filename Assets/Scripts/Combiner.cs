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


