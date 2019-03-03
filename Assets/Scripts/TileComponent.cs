using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "tiledata", menuName = "Tile Data", order = 1)]
public class TileData : ScriptableObject {
    public GameObject collisionEffectsPrefab;
    public GameObject destructionPrefab;
    public GameObject winEffectPrefab;

    public AudioClip destructionSound;
    public AudioClip successSound;
}

public class TileComponent : MonoBehaviour {

    public TileType type;
    public TileData data;

    public AudioClip collisionSound;

    private Timer destroyTimer;
    private float secondsToDestroy = 1.0f;

    private Timer soundTimer;


    public bool clickable;

	void Start(){
        clickable = true;
	}

	void Update(){
        if (destroyTimer != null && destroyTimer.Finished()) {
            DestroyTile(false);
        }

        if(soundTimer != null && soundTimer.Finished()){
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter(Collision coll){
        if(coll.impulse.magnitude / Time.deltaTime > 2.0f){
            Vector3 spawnPosition = Vector3.Lerp(transform.position, coll.GetContact(0).point, 0.5f);
            GameObject spawnedfx = GameObject.Instantiate(data.collisionEffectsPrefab);
            spawnedfx.transform.position = spawnPosition;
        }

        AudioSource source = GetComponent<AudioSource>();
        if(!source.isPlaying){
            source.volume = Mathf.Min(0.4f, (coll.impulse.magnitude / Time.deltaTime) * 3.0f);
            source.clip = collisionSound;
            source.pitch = 0.6f + (Random.value * 0.8f);
            source.Play();
        }
    }

    // Start the combine effect and a timer to destroy
    public void CombineAndDestroyTile(){
        clickable = false;

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

        if(didwin){
            AudioSource source = GetComponent<AudioSource>();
            source.volume = 0.8f;
            source.Stop();
            source.clip = data.successSound;
            source.pitch = 1.0f;
            source.Play();

            GetComponent<MeshRenderer>().enabled = false;

            soundTimer = new Timer(secondsToDestroy);
            soundTimer.Start();
        } else {
            Destroy(gameObject);
        }
    }
}
