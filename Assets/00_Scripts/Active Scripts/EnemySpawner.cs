using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveConfig {
    public int yellowDinoCount;
    public int greenDinoCount;
    public int blueDinoCount;
    public int redDinoCount;
    public int pinkSlimeCount;
    public int greenSlimeCount;
    public int skeletonCount;
    public int orcCount;

    public float timeBetweenEnemies = 1f;
}

public class EnemySpawner : MonoBehaviour {
    public static EnemySpawner Instance { get; private set; }

    [Header("Enemy Prefabs")]
    public GameObject enemyYellowDino;
    public GameObject enemyGreenDino;
    public GameObject enemyBlueDino;
    public GameObject enemyRedDino;
    public GameObject enemyPinkSlime;
    public GameObject enemyGreenSlime;
    public GameObject enemySkeleton;
    public GameObject enemyOrc;


    [Header("Waypoints")]
    public Transform[] waypoints;

    [Header("Waves")]
    public List<WaveConfig> waves = new List<WaveConfig>();

    private List<Enemy> enemies = new List<Enemy>();
    private int currentWaveIndex = 0;

    private bool waveDone = false;
    private bool waveOver = false;
    private int enemyCount = 0;
    [SerializeField] private GameObject wavePanel;
    [HideInInspector]public bool autoStart = false;

    [SerializeField] private Transform enemyContainer;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        
    }

    private void Start() {
        Time.timeScale = 1f;
        
    }
    private void OnEnable() {
        // Ensure the waves list is populated first
        GameStats.Instance.maxWaves = waves.Count;

        // Reset the wave counter when the scene loads
        currentWaveIndex = 0;
        waveDone = false;
        waveOver = false;
        enemies.Clear();
    }


    void Update() {
        enemies.RemoveAll(e => e == null);

        if (!waveOver && waveDone && enemies.Count == 0) {
            GameStats.Instance.playerGold += 50 + (10 * GameStats.Instance.waveCounter);
            GameStats.Instance.UpdateGoldText();
            waveOver = true;
            wavePanel.SetActive(true);

            // Increment wave counter *after* wave has ended
            if (currentWaveIndex < waves.Count) {
                GameStats.Instance.waveCounter++;
            }
            
            GameStats.Instance.UpdateWaveText();
        }

        // Auto-start next wave if toggled on and wave is over
        if (autoStart && waveOver && currentWaveIndex < waves.Count) {
            NextWave();
        }
    }

    IEnumerator SpawnWave(WaveConfig wave) {
        
        waveDone = false;
        waveOver = false;

        yield return StartCoroutine(SpawnEnemies(enemyYellowDino, wave.yellowDinoCount, wave.timeBetweenEnemies));
        yield return StartCoroutine(SpawnEnemies(enemyGreenDino, wave.greenDinoCount, wave.timeBetweenEnemies));
        yield return StartCoroutine(SpawnEnemies(enemyBlueDino, wave.blueDinoCount, wave.timeBetweenEnemies));
        yield return StartCoroutine(SpawnEnemies(enemyRedDino, wave.redDinoCount, wave.timeBetweenEnemies));
        yield return StartCoroutine(SpawnEnemies(enemyPinkSlime, wave.pinkSlimeCount, wave.timeBetweenEnemies));
        yield return StartCoroutine(SpawnEnemies(enemyGreenSlime, wave.greenSlimeCount, wave.timeBetweenEnemies));
        yield return StartCoroutine(SpawnEnemies(enemySkeleton, wave.skeletonCount, wave.timeBetweenEnemies));
        yield return StartCoroutine(SpawnEnemies(enemyOrc, wave.orcCount, wave.timeBetweenEnemies));

        waveDone = true;

        
    }

    IEnumerator SpawnEnemies(GameObject prefab, int count, float delay) {
        for (int i = 0; i < count; i++) {
            GameObject enemy = Instantiate(prefab, transform.position, Quaternion.identity, enemyContainer);

            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript == null) {
                Debug.LogError("Enemy prefab is missing the Enemy script!");
            }
            enemyScript.SetWaypoints(waypoints);
            enemies.Add(enemyScript);
            enemyCount++;

            yield return new WaitForSeconds(delay);
        }
    }

    public void NextWave() {
        
        
        if (currentWaveIndex < waves.Count) {
            WaveConfig nextWave = waves[currentWaveIndex];
            StartCoroutine(SpawnWave(nextWave));

            GameStats.Instance.UpdateWaveText();

            currentWaveIndex++;
            wavePanel.SetActive(false);
        } else {
            Debug.Log("All waves completed!");
            GameManager.Instance.GameWon();
        }
    }

}
