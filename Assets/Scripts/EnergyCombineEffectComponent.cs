using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCombineEffectComponent : MonoBehaviour {
    public float liftHeight = 0.6f;
    public float liftSpeed = 1.0f;
    public float rotationSpeed = 60.0f;
    public float maxLightRange = 3.0f;

    private bool combining;
    private Vector3 destination;
    private Light light;

    private float initialLightRange;
    private float t;

    // Start is called before the first frame update
    void Start() {
        combining = false;
        if (transform.childCount > 0) {
            // breaks if you add more children to the energy ball, so don't pls
            light = transform.GetChild(0).GetComponent<Light>();
        }
    }

    // Update is called once per frame
    void Update() {
        if (combining) {
            transform.position = Vector3.Lerp( transform.position, destination, liftSpeed * Time.deltaTime);
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
            light.range = Mathf.Lerp(initialLightRange, maxLightRange, t);
            t += 0.5f * Time.deltaTime;
        }
    }

    public void Combine() {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        destination = transform.position + new Vector3(0, liftHeight, 0);

        initialLightRange = light.range;

        combining = true;
    }
}
