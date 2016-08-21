using UnityEngine;
using EcsRx;
using Zenject;

public class EnemyManager : MonoBehaviour
{
//    public PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;

	[Inject]
	public IPoolManager PoolManager { get; private set; }

	[Inject]
	DiContainer container { get; set; }

    void Start ()
    {
        InvokeRepeating ("Spawn", spawnTime, spawnTime);
    }

    void Spawn ()
    {
//        if(playerHealth.currentHealth <= 0f)
//        {
//            return;
//        }

        int spawnPointIndex = Random.Range (0, spawnPoints.Length);

//		Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

		var instance = container.InstantiatePrefab (enemy);
		instance.transform.position = spawnPoints [spawnPointIndex].position;
		instance.transform.rotation = spawnPoints [spawnPointIndex].rotation;
    }
}
