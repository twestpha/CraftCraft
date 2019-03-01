using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileComponent : MonoBehaviour {

    public TileType type;

    // universal tile data; collision sound and effects mostly

	void Start(){

	}

	void Update(){

	}

    // On Collision, play sound and make effects poof!
    // Then put the effects timer on cooldown so we don't spam fx

    public void DestroyTile(){
        // play effects and hide model, then delete gameObject poof!
    }
}
