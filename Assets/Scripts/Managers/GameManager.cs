using UnityEngine;

public class GameManager : MonoBehaviour {
    static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance == null) {
                Debug.LogError("没有 GameManager 实例");
                return null;
            }
            return _instance;
        }
    }

    void Awake() {
        if (_instance == null) {
            _instance = this;
        } else {
            Debug.LogWarning("多个 GameManager 实例");
        }
    }

    public bool shouldGotoTutorial = true;
    public Difficulty selectedDifficulty;
    public WaveScriptable[] difficultyToWaves;

    public Difficulty SwitchDifficult() {
        selectedDifficulty = (Difficulty)(((int)selectedDifficulty + 1) % difficultyCount);
        return selectedDifficulty;
    }

    public WaveScriptable GetWaveScriptableByCurrentDifficulty() {
        return difficultyToWaves[(int)selectedDifficulty];
    }

    public string GetCurrentDifficultyName() {
        return DifficultyEnumToName(selectedDifficulty);
    }

    public enum Difficulty {
        EASY = 0,
        NORMAL,
        HARD,
        EXTREME,
    }
    private readonly int difficultyCount = 4;
    public string[] difficultyIndexToName;
    public string DifficultyEnumToName(Difficulty difficulty) {
        return difficultyIndexToName[(int)difficulty];
    }
}
