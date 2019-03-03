using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileComponent : MonoBehaviour {

    public TileType type;

    public GameObject collisionEffectsPrefab;
    public GameObject destructionPrefab;
    public GameObject winEffectPrefab;

    public AudioClip collisionSound;
    public AudioClip destructionSound;
    public AudioClip successSound;

    private Timer destroyTimer;
    private float secondsToDestroy = 1.0f;

    private Timer soundTimer;


    public bool clickable;

	void Start(){
        clickable = true;
	}

	void Update(){
        if (destroyTimer != null && destroyTimer.Finished()) {
            // enable particles, but not win particles
            DestroyTile(true, false);
        }

        if(soundTimer != null && soundTimer.Finished()){
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter(Collision coll){
        if(coll.impulse.magnitude / Time.deltaTime > 2.0f){
            Vector3 spawnPosition = Vector3.Lerp(transform.position, coll.GetContact(0).point, 0.5f);
            GameObject spawnedfx = GameObject.Instantiate(collisionEffectsPrefab);
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
        source.clip = destructionSound;
        source.pitch = 1.0f;
        source.Play();
    }

    // actually destroy the tile
    public void DestroyTile(bool spawnParticles, bool didwin){
        if (spawnParticles) {
            GameObject spawnedfx = GameObject.Instantiate(didwin ? winEffectPrefab : destructionPrefab);
            spawnedfx.transform.position = transform.position;
        }

        if(didwin){
            AudioSource source = GetComponent<AudioSource>();
            source.volume = 0.8f;
            source.Stop();
            source.clip = successSound;
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
