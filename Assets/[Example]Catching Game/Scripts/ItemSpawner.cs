using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (CatchGameManager.Instance == null || !CatchGameManager.Instance.isGameActive)
            {
                yield return null;
                continue;
            }

            GameObject[] prefabs = CatchGameManager.Instance.itemPrefabs;

            if (prefabs != null && prefabs.Length > 0)
            {
                float minX = CatchGameManager.Instance.spawnMinX;
                float maxX = CatchGameManager.Instance.spawnMaxX;

                float randomX = Random.Range(minX, maxX);
                Vector3 spawnPos = new Vector3(randomX, transform.position.y, 0f);
                int randomIndex = Random.Range(0, prefabs.Length);
                Instantiate(prefabs[randomIndex], spawnPos, Quaternion.identity);
            }

            yield return new WaitForSeconds(CatchGameManager.Instance.spawnInterval);
        }
    }
}