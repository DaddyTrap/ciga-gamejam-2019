using UnityEngine;
using System.Collections.Generic;

public class Pool : MonoBehaviour {
    public GameObject prefab;
    List<GameObject> _pool;

    public GameObject GetOneInstance() {
        // 寻找未使用实例
        for (int i = 0; i < _pool.Count; ++i) {
            if (!_pool[i].activeSelf) {
                _pool[i].SetActive(true);
                return _pool[i];
            }
        }

        // 能执行到这里，即当前实例都在使用，需要创建实例
        var gobj = Instantiate(prefab, transform) as GameObject;
        _pool.Add(gobj);
        return gobj;
    }
}
