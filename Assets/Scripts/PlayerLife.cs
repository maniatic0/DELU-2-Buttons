using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLife : MonoBehaviour {

	public static PlayerLife player = null;

	public static GameObject playerObject = null;

	public float CurrentLife {get; private set;}

	[SerializeField]
	private float maxLife = 100.0f;

	[SerializeField]
	private float lifeChargeModifier = .1f;

	public float MaxLife {get{return maxLife;} }

	public float NormalizedCurrentLife {get; private set;}

	public UnityEvent OnDamage = new UnityEvent();

	public UnityEvent OnHeal = new UnityEvent();

	public UnityEvent OnDeath = new UnityEvent();

	public UnityEvent OnRevive = new UnityEvent();

	private void Awake() {
		if (player == null) {
			player = this;
			playerObject = this.gameObject;
		} else {
			Debug.LogError("Duplicated Player", this.gameObject);
			Destroy(this);
		}
	}

	// Use this for initialization
	void Start () {
		CurrentLife = maxLife;
		NormalizedCurrentLife = 1.0f;
		OnHeal.Invoke();
		StartCoroutine(LifeCharge());
	}
	
	IEnumerator LifeCharge() 
	{
		while (CurrentLife > 0.0f)
		{
			if (CurrentLife == MaxLife) {
				yield return null;
				continue;
			}
			CurrentLife += Time.deltaTime * lifeChargeModifier;
			NormalizedCurrentLife = Mathf.Clamp01(CurrentLife / MaxLife);
			if (CurrentLife > MaxLife)
			{
				CurrentLife = MaxLife;
				NormalizedCurrentLife = 1.0f;
			}
			OnHeal.Invoke();
			yield return null;
		}
	}

	public void Damage(float amount) 
	{
		CurrentLife -= amount;
		if (CurrentLife <= 0.0f)
		{
			CurrentLife = 0.0f;
			NormalizedCurrentLife = 0.0f;
			Die();
			return;
		}
		NormalizedCurrentLife = Mathf.Clamp01(CurrentLife / MaxLife);
		OnDamage.Invoke();
	}

	public void Heal(float amount) 
	{
		CurrentLife += amount;
		NormalizedCurrentLife = Mathf.Clamp01(CurrentLife / MaxLife);
		OnHeal.Invoke();
	}

	public void Die()
	{
		StopAllCoroutines();
		OnDeath.Invoke();
	}

	public void Revive() 
	{
		Start();
		OnRevive.Invoke();
	}
}
