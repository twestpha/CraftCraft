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

	void Start(){

	}

	void Update(){

	}

    void OnCollisionEnter(Collision coll){
        if(coll.impulse.magnitude / Time.deltaTime > 2.0f){
            Vector3 spawnPosition = Vector3.Lerp(transform.position, coll.GetContact(0).point, 0.5f);
            GameObject spawnedfx = GameObject.Instantiate(data.collisionEffectsPrefab);
            spawnedfx.transform.position = spawnPosition;
        }
    }

    public void DestroyTile(){
        GameObject spawnedfx = GameObject.Instantiate(data.destructionPrefab);
        spawnedfx.transform.position = transform.position;

        Destroy(gameObject);
    }
}
