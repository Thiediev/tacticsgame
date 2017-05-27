using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

    private Vector3 backgroundMovementVector = new Vector3(0.003f, 0, 0.003f);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position += backgroundMovementVector;
	}
}
