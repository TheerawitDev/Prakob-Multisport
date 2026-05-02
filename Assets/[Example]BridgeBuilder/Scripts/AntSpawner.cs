using UnityEngine;
using System.Collections;

public class AntSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (BridgeGameManager.Instance == null || !BridgeGameManager.Instance.isGameActive)
            {
                yield return null;
                continue;
            }

            GameObject[] prefabs = BridgeGameManager.Instance.antPrefabs;

            if (prefabs != null && prefabs.Length > 0 && spawnPoint != null)
            {
                int randomAnt = Random.Range(0, prefabs.Length);
                Instantiate(prefabs[randomAnt], spawnPoint.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(BridgeGameManager.Instance.spawnInterval);
        }
    }
}