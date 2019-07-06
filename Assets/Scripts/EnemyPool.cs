using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour {
    public Sprite[] enemySprite;
    public GameObject trianglePrefab;
    public GameObject closePrefab;

    public GameObject rangePrefab;
    List<GameObject> trianglePool;
    List<GameObject> closePool;
    List<GameObject> rangePool;
    public GameObject getOneInstance (Shape enemyType, bool isRange) {
        Shape s = enemyType;
        switch (s) {
            case Shape.COIN:
                break;
            case Shape.HEART:
                break;
            case Shape.TRIANGLE:
                return getOneTriangle ();
            default:
                break;
        }
        if (isRange) {
            return getOneRange (enemyType);
        } else return getOneClose (enemyType);
    }

    private GameObject getOneTriangle () {
        // 寻找未使用实例
        for (int i = 0; i < trianglePool.Count; ++i) {
            if (!trianglePool[i].activeSelf) {
                trianglePool[i].SetActive (true);
                return trianglePool[i];
            }
        }

        // 能执行到这里，即当前实例都在使用，需要创建实例
        var gobj = Instantiate (trianglePrefab, transform) as GameObject;
        trianglePool.Add (gobj);
        gobj.GetComponent<Triangle> ().setEnemyType (Shape.TRIANGLE, enemySprite[2]);
        return gobj;
    }
    private GameObject getOneRange (Shape enemyType) {
        // 寻找未使用实例
        for (int i = 0; i < rangePool.Count; ++i) {
            if (!rangePool[i].activeSelf) {
                rangePool[i].SetActive (true);
                return rangePool[i];
            }
        }

        // 能执行到这里，即当前实例都在使用，需要创建实例
        var gobj = Instantiate (rangePrefab, transform) as GameObject;
        rangePool.Add (gobj);
        gobj.GetComponent<Enemy_Range> ().setEnemyType (enemyType, enemySprite[(int) enemyType]);
        return gobj;
    }
    private GameObject getOneClose (Shape enemyType) {
        // 寻找未使用实例
        for (int i = 0; i < closePool.Count; ++i) {
            if (!closePool[i].activeSelf) {
                closePool[i].SetActive (true);
                return rangePool[i];
            }
        }

        // 能执行到这里，即当前实例都在使用，需要创建实例
        var gobj = Instantiate (closePrefab, transform) as GameObject;
        closePool.Add (gobj);
        gobj.GetComponent<Enemy_Close> ().setEnemyType (enemyType, enemySprite[(int) enemyType]);
        return gobj;
    }
}