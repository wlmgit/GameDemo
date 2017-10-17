using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public float vSpeed = 100f;
    public Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
        rigidbody.velocity = new Vector3(0,0,10);

    }
	
	// Update is called once per frame
	void Update () {
        float v = vSpeed * Input.GetAxis("Mouse Y");
        //transform.Rotate(v, 0, 0);
    }
}
