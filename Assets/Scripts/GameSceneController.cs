using UnityEngine;
using System.Collections.Generic;

public class GameSceneController : MonoBehaviour {
    public Character mainCharacter;
    public EnemyPool enemyPool;
    public BulletPool bulletPool;

    public Wave[] waves;

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

    private bool spawned = false;
    void Update() {
        if (!spawned) {
            spawned = true;
            // TODO: 刷新敌人
            var wave = waves[0];
            foreach (var waveEle in wave.eles) {
                GameObject enemy = null;
                enemy = enemyPool.getOneInstance(waveEle.enemyType, waveEle.isRange);
                enemy.transform.position = waveEle.startPosition;
                // 设置 Enemy 的 character
                enemy.GetComponent<Enemy>().setCharacter(mainCharacter.gameObject);
            }
        }
    }

    public void StartGame() {
        // TODO: Game Init
        gameRunning = true;
    }
}
