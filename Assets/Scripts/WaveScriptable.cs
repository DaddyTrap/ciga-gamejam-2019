using UnityEngine;

[System.Serializable]
public class WaveScriptable : ScriptableObject {
    [System.Serializable]
    public class WaveEle {
        public int positionIndex;
        public Shape enemyType;
        public bool isRange;
    }
    [System.Serializable]
    public class Wave {
        public float time;
        public WaveEle[] waveEles;
    }
    [System.Serializable]
    public class WaveScriptableWrapper {
        public Wave[] waves;
    }
    public Vector3[] positions;
    public Wave[] waves;
}
