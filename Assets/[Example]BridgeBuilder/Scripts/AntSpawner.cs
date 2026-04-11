using UnityEngine;
using System.Collections;

public class AntSpawner : MonoBehaviour
{
    public GameObject[] antPrefabs; // ใส่ Prefab มดดี และ มดอันตราย
    public Transform spawnPoint;    // ใส่จุดเกิดฝั่งซ้าย
    public float spawnInterval = 2f; // ความเร็วในการปล่อยมด

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // หยุดเสกมดถ้าเกมโอเวอร์
            if (BridgeGameManager.Instance != null && !BridgeGameManager.Instance.isGameActive)
            {
                yield return null;
                continue;
            }

            if (antPrefabs.Length > 0 && spawnPoint != null)
            {
                int randomAnt = Random.Range(0, antPrefabs.Length);
                Instantiate(antPrefabs[randomAnt], spawnPoint.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}