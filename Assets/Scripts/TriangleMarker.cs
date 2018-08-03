using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleMarker : MonoBehaviour {

	[SerializeField]
	private MovementScript player;

	private Color tempColor;

	private SpriteRenderer[] pointRenderers = null;

	private const float movEpsilon = 0.001f;

	private float prevAlpha = 1.0f;
	private float currentAlpha = 1.0f;

	private float prevHealth = 1.0f;
	private float currentHealth = 1.0f;

	[SerializeField]
	private Color fullHealth = Color.green;

	[SerializeField]
	private Color noHealth = Color.red;

	private void Start() {
		pointRenderers = new SpriteRenderer[player.Points.Length];
		for (int i = 0; i < player.Points.Length; i++)
		{
			pointRenderers[i] = player.Points[i].GetComponent<SpriteRenderer>();
		}
	}
	
	// Update is called once per frame
	public void SetAlpha() {
		currentAlpha = Mathf.Clamp01(player.SlowMoTimeNormalized);
		if ( Mathf.Abs(prevAlpha - currentAlpha) >= movEpsilon) {
			for (int i = 0; i < pointRenderers.Length; i++)
			{
				tempColor = pointRenderers[i].color;
				tempColor.a = currentAlpha;
				pointRenderers[i].color = tempColor;
			}
			prevAlpha = currentAlpha;
		}
	}

	public void SetColor() {
		currentHealth = Mathf.Clamp01(PlayerLife.player.NormalizedCurrentLife);
		if ( Mathf.Abs(prevHealth - currentHealth) >= movEpsilon) {
			for (int i = 0; i < pointRenderers.Length; i++)
			{
				tempColor = Color.Lerp(noHealth, fullHealth, currentHealth);
				tempColor.a = pointRenderers[i].color.a;
				pointRenderers[i].color = tempColor;
			}
			prevHealth = currentHealth;
		}
	}
}
