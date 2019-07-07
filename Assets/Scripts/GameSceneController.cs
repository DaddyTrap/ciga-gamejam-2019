using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameSceneController : MonoBehaviour {
    private int enemyDeathCount;
    public ShakeCamera shakeCamera;
    public Character mainCharacter;
    public EnemyPool enemyPool;
    public BulletPool bulletPool;

    public WaveScriptable waveScriptable;

    public GameObject sanityItemPrefab;
    public Sprite sanityFull, sanityEmpty;
    public Transform sanityParent;

    [Header("Debug")]
    public WaveScriptable[] waveScriptables;
    public Text waveHint;

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

        // For debug
        if (Input.GetKeyDown("1")) {
            waveScriptable = waveScriptables[0];
        }
        if (Input.GetKeyDown("2")) {
            waveScriptable = waveScriptables[1];
        }
        if (Input.GetKeyDown("3")) {
            waveScriptable = waveScriptables[2];
        }
        if (Input.GetKeyDown("4")) {
            waveScriptable = waveScriptables[3];
        }
        waveHint.text = waveScriptable.name;
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
        InitGame();
        gameRunning = true;
    }

    private Image[] sanityItems;

    public void InitGame() {
        // 初始化 San 值
        sanityItems = new Image[9];
        for (int i = 0; i < 9; ++i) {
            sanityItems[i] = Instantiate<GameObject>(sanityItemPrefab, sanityParent).GetComponent<Image>();
        }
        ChangeSanity(mainCharacter.sanity);
        // 初始化敌人死亡计数
        enemyDeathCount = 0;
    }

    public void ChangeSanity(int sanity) {
        for (int i = 0; i < sanity; ++i) {
            sanityItems[i].sprite = sanityFull;
        }
        for (int i = sanity; i < 9; ++i) {
            sanityItems[i].sprite = sanityEmpty;
        }
    }
    public void OnEnemyDeath(Enemy enemy) {
        ++ enemyDeathCount;
        // TODO : 
    }

    public void OnPlayerDeath(Character character) {

    }
}
