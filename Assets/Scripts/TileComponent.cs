using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "tiledata", menuName = "Tile Data", order = 1)]
public class TileData : ScriptableObject {
    public GameObject collisionEffectsPrefab;
    public GameObject destructionPrefab;
    public GameObject winEffectPrefab;

    public AudioClip collisionSound;
    public AudioClip destructionSound;
}

public class TileComponent : MonoBehaviour {

    public TileType type;
    public TileData data;

    private Timer destroyTimer;
    private float secondsToDestroy = 1.0f;

	void Start(){
	}

	void Update(){
        if (destroyTimer != null && destroyTimer.Finished()) {
            DestroyTile(false);
        }
	}

    void OnCollisionEnter(Collision coll){
        if(coll.impulse.magnitude / Time.deltaTime > 2.0f){
            Vector3 spawnPosition = Vector3.Lerp(transform.position, coll.GetContact(0).point, 0.5f);
            GameObject spawnedfx = GameObject.Instantiate(data.collisionEffectsPrefab);
            spawnedfx.transform.position = spawnPosition;
        }

        AudioSource source = GetComponent<AudioSource>();
        source.volume = coll.impulse.magnitude * 3.0f;
        source.Stop();
        source.clip = data.collisionSound;
        source.pitch = 0.6f + (Random.value * 0.8f);
        source.Play();
    }

    // Start the combine effect and a timer to destroy
    public void CombineAndDestroyTile(){
        SendMessage("Combine");
        destroyTimer = new Timer(secondsToDestroy);
        destroyTimer.Start();

        AudioSource source = GetComponent<AudioSource>();
        source.volume = 0.8f;
        source.Stop();
        source.clip = data.destructionSound;
        source.pitch = 1.0f;
        source.Play();
    }

    // actually destroy the tile
    public void DestroyTile(bool didwin){
        GameObject spawnedfx = GameObject.Instantiate(didwin ? data.winEffectPrefab : data.destructionPrefab);
        spawnedfx.transform.position = transform.position;

        Destroy(gameObject);
    }
}
