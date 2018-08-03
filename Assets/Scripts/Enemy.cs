using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour {

	[SerializeField]
	private float damageToApply = 0.5f;

	[SerializeField]
	private float lifePlayerRecharge = 0.5f;

	[SerializeField]
	private float slowmoPlayerRecharge = 0.001f;

	[SerializeField]
	private float maxDistance2Center = 30.0f;
	private float maxDistance2CenterSqr;

	public UnityEvent onAttack = new UnityEvent();

	public UnityEvent onDie = new UnityEvent();
	// Use this for initialization
	public virtual void Start () {
		this.gameObject.SetActive(true);
		maxDistance2CenterSqr = maxDistance2Center * maxDistance2Center;
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (maxDistance2CenterSqr < transform.position.sqrMagnitude) {
			PlayerLife.player.Heal(lifePlayerRecharge);
			MovementScript.movement.SlowmoRecharge(slowmoPlayerRecharge);
			onDie.Invoke();
			this.gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject == PlayerLife.playerObject)
		{
			PlayerLife.player.Damage(damageToApply);
			onAttack.Invoke();
			this.gameObject.SetActive(false);
		}
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject == PlayerLife.playerObject)
		{
			PlayerLife.player.Damage(damageToApply);
			onAttack.Invoke();
			this.gameObject.SetActive(false);
		}
	}
}
