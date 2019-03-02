using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "tiledata", menuName = "Tile Data", order = 1)]
public class TileData : ScriptableObject {
    public GameObject collisionEffectsPrefab;
    public GameObject destructionPrefab;

    public AudioClip collisionSound;
}

public class TileComponent : MonoBehaviour {

    public TileType type;
    public TileData data;

    private Timer soundCooldownTimer;

	void Start(){
        soundCooldownTimer = new Timer(0.5f);
	}

	void Update(){

	}

    // On Collision, play sound and make effects poof!
    // Then put the effects timer on cooldown so we don't spam fx
    void OnCollisionEnter(Collision coll){
        // soundCooldownTimer
    }

    public void DestroyTile(){
        // play effects and hide model, then delete gameObject poof!
    }
}
