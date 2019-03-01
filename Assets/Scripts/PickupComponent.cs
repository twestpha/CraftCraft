using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupComponent : MonoBehaviour {
    public const float MAX_RAYCAST_DISTANCE = 20.0f;
    public const int TILE_COLLISION_MASK = 1 << 9;

    public const float TILE_RAISE_HEIGHT = 1.5f;
    public const float TILE_SEEK_TIME = 0.15f;

    private Plane groundplane;
    private GameObject heldTile;

    private Vector3 tileVelocity;

	void Start(){
        groundplane = new Plane(Vector3.down /* wtf plane normals */, TILE_RAISE_HEIGHT);
	}

	void Update(){
        if(Input.GetMouseButtonDown(0)){
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(mouseRay, out hit, MAX_RAYCAST_DISTANCE, TILE_COLLISION_MASK)){
                heldTile = hit.collider.gameObject;

                heldTile.GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        if(heldTile){
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance = 0.0f;
            Vector3 tileTargetPosition = Vector3.zero;

            if(groundplane.Raycast(mouseRay, out distance)){
                tileTargetPosition = mouseRay.GetPoint(distance);
            }

            heldTile.transform.position = Vector3.SmoothDamp(
                heldTile.transform.position,
                tileTargetPosition,
                ref tileVelocity,
                TILE_SEEK_TIME
            );
        }

        if(Input.GetMouseButtonUp(0) && heldTile){
            heldTile.GetComponent<Rigidbody>().isKinematic = false;
            heldTile.GetComponent<Rigidbody>().velocity = tileVelocity;

            heldTile = null;
        }
	}
}
