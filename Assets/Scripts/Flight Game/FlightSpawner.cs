using UnityEngine;
using System.Collections;

public class FlightSpawner : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (FlightGameManager.Instance == null || !FlightGameManager.Instance.isGameActive)
            {
                yield return null;
                continue;
            }

            GameObject[] prefabs = FlightGameManager.Instance.spawnPrefabs;

            if (prefabs != null && prefabs.Length > 0)
            {
                float minY = FlightGameManager.Instance.spawnMinY;
                float maxY = FlightGameManager.Instance.spawnMaxY;

                float randomY = Random.Range(minY, maxY);
                Vector3 spawnPos = new Vector3(transform.position.x, randomY, 0f);

                int randomIndex = Random.Range(0, prefabs.Length);
                Instantiate(prefabs[randomIndex], spawnPos, Quaternion.identity);
            }

            yield return new WaitForSeconds(FlightGameManager.Instance.spawnInterval);
        }
    }
}