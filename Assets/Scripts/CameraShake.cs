using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	[SerializeField]
	private float shakeAmplitude = 0.3f;

	[SerializeField]
	private float shakeSpeed = 0.5f;

	[SerializeField]
	private float shakeTime = 0.5f;

	private float remainingTime;

	private Coroutine startedShake = null;

	IEnumerator ShakeCoroutine() {
		Transform cameraTransform = Camera.main.transform;
		Vector3 startPos = cameraTransform.position;
		Vector3 deltaMov = Vector3.zero;
		remainingTime = shakeTime;
		float seed = Random.Range(0.0f, Time.time);
		while (remainingTime > 0f)
		{
			deltaMov.x = shakeAmplitude * Mathf.PerlinNoise(shakeSpeed * Time.time, shakeSpeed * Time.time + seed);
			//deltaMov.y = shakeAmplitude * Mathf.PerlinNoise(shakeSpeed * Time.time + seed, shakeSpeed * Time.time);
			deltaMov *= 2.0f; 
			deltaMov-= Vector3.one;
			deltaMov.y = 0f;
			deltaMov.z = 0f;
			deltaMov += startPos;
			cameraTransform.position = deltaMov;
			remainingTime -= Time.deltaTime;
			yield return null;
		}
		cameraTransform.position = startPos;
		startedShake = null;
	}

	public void Shake() {
		if (startedShake != null) {
			remainingTime += shakeTime;
			return;
		}
		startedShake = StartCoroutine(ShakeCoroutine());
	}
}
