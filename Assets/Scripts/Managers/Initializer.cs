using UnityEngine;

public class Initializer : MonoBehaviour {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void GlobalInit() {
        var main = Resources.Load<GameObject>("Main");
        var mainObj = Instantiate(main) as GameObject;
        DontDestroyOnLoad(mainObj);
        mainObj.GetComponent<Initializer>().Init();
    }

    public void Init() {

    }
}
