using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class WaypointGroup
{
    public Transform[] waypoints; // Array dari waypoint
}

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array prefab musuh yang akan di-spawn
    public Transform[] spawnPoints; // Array dari titik spawn
    public WaypointGroup[] waypointGroups; // Array dari kelompok waypoints
    public int initialEnemiesPerWave = 10; 
    public float timeBetweenWaves = 30f;
    public float spawnDelay = 0.5f;
    private int currentWave = 1; 
    public float baseSpeed = 5f; 
    public float speedIncrement = 1f;
    public int enemiesIncrementPerWave = 5;
    
    public Text waveText;

    private void Start()
    {
        UpdateWaveText(); 
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        while (true)
        {
            int enemiesToSpawn = initialEnemiesPerWave + (enemiesIncrementPerWave * (currentWave - 1));
            float currentSpeed = baseSpeed + (speedIncrement * (currentWave - 1));

            Debug.Log($"Wave {currentWave}: Spawning {enemiesToSpawn} enemies with speed {currentSpeed}");

            UpdateWaveText();

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy(currentSpeed);
                yield return new WaitForSeconds(spawnDelay);
            }

            currentWave++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private void SpawnEnemy(float speed)
    {
        // Pilih titik spawn acak
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        
        // Pilih prefab musuh acak
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);

        // Buat musuh di posisi spawn point yang dipilih
        GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnPoints[spawnIndex].position, Quaternion.identity);

        // Atur kecepatan musuh
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.SetSpeed(speed);
            
            // Set waypoints sesuai spawn point
            enemyMovement.SetWaypoints(waypointGroups[spawnIndex].waypoints);
        }
    }

    private void UpdateWaveText()
    {
        waveText.text = "Wave: " + currentWave.ToString();
    }
}
