using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public float spawnInterval = 1f;
    public float minX = -8f;
    public float maxX = 8f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (CatchGameManager.Instance != null && !CatchGameManager.Instance.isGameActive)
            {
                yield return null;
                continue;
            }

            if (itemPrefabs.Length > 0)
            {
                float randomX = Random.Range(minX, maxX);
                Vector3 spawnPos = new Vector3(randomX, transform.position.y, 0f);
                int randomIndex = Random.Range(0, itemPrefabs.Length);
                Instantiate(itemPrefabs[randomIndex], spawnPos, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}