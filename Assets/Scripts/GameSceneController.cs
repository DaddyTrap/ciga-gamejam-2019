using UnityEngine;
using System.Collections.Generic;

public class GameSceneController : MonoBehaviour {
    public Character mainCharacter;
    public Pool enemyClosePool;
    public Pool enemyRangePool;

    public Pool characterBulletPool;
    public Pool enemyBulletPool;

    public Wave[] waves;

    public bool gameRunning { get; set; }

    public static GameSceneController instance;

    void Awake() {
        if (instance != null) {
            instance = this;
        } else {
            Debug.LogWarning("出现了两个 GameSceneController");
        }
        gameRunning = false;
    }

    void Update() {
        // TODO: 刷新敌人
        var wave = waves[0];
        foreach (var waveEle in wave.eles) {
            GameObject enemy = null;
            if (waveEle.isRange) {
                enemy = enemyRangePool.GetOneInstance();
            } else {
                enemy = enemyClosePool.GetOneInstance();
            }
            enemy.transform.position = waveEle.position;
            // 设置 Enemy 的 character
            enemy.GetComponent<Enemy>().setCharacter(mainCharacter.gameObject);
        }
    }

    public void StartGame() {
        // TODO: Game Init
        gameRunning = true;
    }
}
