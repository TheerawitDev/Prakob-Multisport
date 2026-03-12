using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class WaveConfig
{
    public string waveName;
    public GameObject[] enemyPrefabs;
    public int enemyCount;
    public float spawnInterval;
}

public class WaveManager : MonoBehaviour
{
    public WaveConfig[] waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 3f;

    public UnityEvent<string> OnWaveStarted;
    public UnityEvent OnWaveCompleted;
    public UnityEvent OnAllWavesCompleted;

    private int currentWaveIndex = 0;
    private int enemiesAlive = 0;
    private bool isSpawning = false;

    private void Start()
    {
        if (waves.Length > 0)
        {
            StartCoroutine(StartNextWave());
        }
    }

    private IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        WaveConfig currentWave = waves[currentWaveIndex];
        OnWaveStarted?.Invoke(currentWave.waveName);

        isSpawning = true;
        for (int i = 0; i < currentWave.enemyCount; i++)
        {
            SpawnRandomEnemy(currentWave);
            yield return new WaitForSeconds(currentWave.spawnInterval);
        }
        isSpawning = false;
    }

    private void SpawnRandomEnemy(WaveConfig wave)
    {
        if (wave.enemyPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        int randomEnemy = Random.Range(0, wave.enemyPrefabs.Length);
        int randomPoint = Random.Range(0, spawnPoints.Length);

        GameObject enemy = Instantiate(wave.enemyPrefabs[randomEnemy], spawnPoints[randomPoint].position, Quaternion.identity);

        HealthSystem enemyHealth = enemy.GetComponent<HealthSystem>();
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath.AddListener(OnEnemyDied);
        }

        enemiesAlive++;
    }

    private void OnEnemyDied()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && !isSpawning)
        {
            OnWaveCompleted?.Invoke();
            currentWaveIndex++;

            if (currentWaveIndex < waves.Length)
            {
                StartCoroutine(StartNextWave());
            }
            else
            {
                OnAllWavesCompleted?.Invoke();
            }
        }
    }
}