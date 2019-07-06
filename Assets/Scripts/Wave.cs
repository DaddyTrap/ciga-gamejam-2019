using UnityEngine;

[System.Serializable]
public class Wave {
    [System.Serializable]
    public class WaveEle {
        public Vector3 position;
        public bool isRange;
    }
    public WaveEle[] eles;
}
