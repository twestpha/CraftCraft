using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowRotationComponent : MonoBehaviour {

    public float xrate;
    public float yrate;

    private MeshRenderer meshrenderer;

    void Start(){
        meshrenderer = GetComponent<MeshRenderer>();
    }

    void Update(){
        meshrenderer.material.SetTextureOffset("_MainTex", new Vector2(Time.time * xrate, Time.time * yrate));
    }
}
