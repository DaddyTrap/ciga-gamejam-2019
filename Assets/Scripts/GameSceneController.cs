using UnityEngine;
using System.Collections.Generic;

public class GameSceneController : MonoBehaviour {
    public Character mainCharacter;
    public EnemyPool enemyPool;
    public BulletPool bulletPool;

    public WaveScriptable waveScriptable;

    public bool gameRunning { get; set; }

    public static GameSceneController instance;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogWarning("出现了两个 GameSceneController");
        }
        gameRunning = false;
    }

    float gameStartTime = 0f;
    float elapsedTime = 0f;
    void Update() {
        if (gameRunning) {
            elapsedTime = Time.time - gameStartTime;
            // 检测是否该生成下一波怪
            CheckSpawn();
        }
        if (Input.GetKeyDown(KeyCode.F) && !gameRunning) {
            gameRunning = true;
            gameStartTime = Time.time;
            StartGame();
        }
    }

    void CheckSpawn() {
        if (currentWaveIndex + 1 >= waveScriptable.waves.Length) {
            // 游戏结束
            gameRunning = false;
            return;
        }
        var nextWave = waveScriptable.waves[currentWaveIndex + 1];
        if (nextWave.time <= elapsedTime) {
            Debug.Log(nextWave.time);
            Debug.Log(currentWaveIndex);
            // 刷这一波
            SpawnWave(nextWave);
            currentWaveIndex = currentWaveIndex + 1;
            Debug.Log(currentWaveIndex);
        }
    }

    private int currentWaveIndex = -1;

    void SpawnWave(WaveScriptable.Wave wave) {
        foreach (var waveEle in wave.waveEles) {
            GameObject enemy = null;
            Debug.Log(waveEle.enemyType);
            enemy = enemyPool.getOneInstance(waveEle.enemyType, waveEle.isRange);
            enemy.transform.position = waveScriptable.positions[waveEle.positionIndex];
            // 设置 Enemy 的 character
            enemy.GetComponent<Enemy>().setCharacter(mainCharacter.gameObject);
        }
    }

    public void StartGame() {
        // TODO: Game Init
        Debug.Log("GAME START!");
        gameRunning = true;
    }
}
