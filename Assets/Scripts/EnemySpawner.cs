using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	[SerializeField]
	private GameObject spawnersHolder;

	private Transform[] spawners;

	[SerializeField]
	private float spawnTime = 0.3f;

	[System.Serializable]
	struct SpawnableEnemy
	{
		public GameObject enemy;
		public float probability;
		public int currentSpawnIndex;

		public GameObject[] pool;
		public Enemy[] poolEnemy;

		public int poolSize;
	}

	[SerializeField]
	private SpawnableEnemy[] enemies;

	private SpawnableEnemy enemiesSelected;

	private GameObject gameobjectSelected;
	private Enemy enemytSelected;

	private void Awake() {
		spawners = new Transform[spawnersHolder.transform.childCount];
		for (int i = 0; i < spawners.Length; i++)
		{
			spawners[i] = spawnersHolder.transform.GetChild(i);
		}

		GameObject holder = new GameObject("Enemies Holder");

		float sum = 0.0f;
		for (int i = 0; i < enemies.Length; i++)
		{
			sum += enemies[i].probability;
		}

		for (int i = 0; i < enemies.Length; i++)
		{
			enemies[i].probability /= sum;
			enemies[i].currentSpawnIndex = 0;
			enemies[i].pool = new GameObject[enemies[i].poolSize];
			enemies[i].poolEnemy = new Enemy[enemies[i].poolSize];
			GameObject holderSpecific = new GameObject(enemies[i].enemy.name + " Holder");
			holderSpecific.transform.parent = holder.transform;
			for (int j = 0; j < enemies[i].poolSize; j++)
			{
				enemies[i].pool[j] = Instantiate(enemies[i].enemy, holderSpecific.transform);
				enemies[i].pool[j].SetActive(false);
				enemies[i].poolEnemy[j] = enemies[i].pool[j].GetComponent<Enemy>();
			}
		}

		System.Array.Sort<SpawnableEnemy>(enemies, (x,y) => x.probability.CompareTo(y.probability));
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(Generate());
	}

	void GenerateNext() {
		float probability = Random.Range(0.0f, 1.0f);
		int i = 0;
		while (i < enemies.Length) {
			probability -= enemies[i].probability;
			if (probability <= 0.0f )
			{
				break;
			}
			i++;
		}
		i = i >= enemies.Length? enemies.Length - 1 : i;

		int spawnerIndex = Random.Range(0, spawners.Length);
		enemiesSelected = enemies[i];

		int gameobjectIndex = enemiesSelected.currentSpawnIndex;
		gameobjectSelected = enemiesSelected.pool[gameobjectIndex];
		enemytSelected = enemiesSelected.poolEnemy[gameobjectIndex];
		gameobjectSelected.SetActive(false);
		gameobjectSelected.transform.position = spawners[spawnerIndex].transform.position;
		enemytSelected.Start();
		gameobjectSelected.SetActive(true);

		enemies[i].currentSpawnIndex++;
		enemies[i].currentSpawnIndex %= enemiesSelected.poolSize;
	}

	IEnumerator Generate() {
		while (true) {
			GenerateNext();
			yield return new WaitForSeconds(spawnTime);
		}
	}

	public void OnDeath() {
		StopAllCoroutines();
	}

	public void OnRevive() {
		Start();
	}
}
