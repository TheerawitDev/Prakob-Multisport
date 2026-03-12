using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public LevelChunk startChunk;
    public LevelChunk[] chunkPrefabs;
    public Transform player;
    public float spawnDistance = 20f;
    public int initialChunks = 3;

    private Vector3 nextSpawnPosition;
    private Queue<LevelChunk> spawnedChunks = new Queue<LevelChunk>();

    private void Start()
    {
        nextSpawnPosition = startChunk.endPoint.position;
        spawnedChunks.Enqueue(startChunk);

        for (int i = 0; i < initialChunks; i++)
        {
            SpawnChunk();
        }
    }

    private void Update()
    {
        if (player == null) return;

        if (Vector3.Distance(player.position, nextSpawnPosition) < spawnDistance)
        {
            SpawnChunk();
            RemoveOldChunk();
        }
    }

    private void SpawnChunk()
    {
        int randomIndex = Random.Range(0, chunkPrefabs.Length);
        LevelChunk newChunk = Instantiate(chunkPrefabs[randomIndex], nextSpawnPosition, Quaternion.identity);

        nextSpawnPosition = newChunk.endPoint.position;
        spawnedChunks.Enqueue(newChunk);
    }

    private void RemoveOldChunk()
    {
        if (spawnedChunks.Count > initialChunks + 2)
        {
            LevelChunk oldChunk = spawnedChunks.Dequeue();
            Destroy(oldChunk.gameObject);
        }
    }
}