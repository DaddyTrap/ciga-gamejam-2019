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
}
