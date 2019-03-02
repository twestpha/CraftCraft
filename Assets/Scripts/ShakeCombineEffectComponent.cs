using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCombineEffectComponent : MonoBehaviour {
    public Vector2 shakeSpeedRange;
    public float shakeAmount;

    private float shakeSpeed;
    private bool combining;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start() {
        combining = false;
        shakeSpeed = Random.Range(shakeSpeedRange.x, shakeSpeedRange.y);
    }

    // Update is called once per frame
    void Update() {
        if (combining) {
            transform.position = new Vector3(
                startPos.x + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount,
                startPos.y + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount,
                startPos.z
            );
        }
    }

    public void Combine() {
        startPos = transform.position;
        combining = true;
    }
}
