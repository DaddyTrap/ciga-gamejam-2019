using UnityEngine;

public class OPSceneController : MonoBehaviour {
    [System.Serializable]
    public class _OPCut {
        public Sprite image;
        public string subtitle;
        public float duration;
    }

    public _OPCut[] cuts;
}
