using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
	[SerializeField]
	private float rotationSpeed = 0.5f;


	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
	}
}
