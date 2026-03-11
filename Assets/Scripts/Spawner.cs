using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnInterval = 2f;
    public Transform[] spawnPoints;
    public bool spawnContinuously = true;

    private float timer;

    private void Update()
    {
        if (!spawnContinuously) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            Spawn();
            timer = 0f;
        }
    }

    public void Spawn()
    {
        if (prefabToSpawn == null) return;

        Transform spawnPoint = transform;
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            spawnPoint = spawnPoints[randomIndex];
        }

        Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
    }
}