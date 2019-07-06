using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour {
    public GameObject bulletPrefab;

    public Sprite enemyHeartBullet;

    public Sprite enemyCoinBullet;
    public Sprite characterHeartBullet;
    public Sprite characterCoinBullet;

    private List<GameObject> _pool;

    void Awake() {
        _pool = new List<GameObject>();
    }

    public GameObject Get(Shape shape, bool isCharacter) {
        GameObject gottenObj = null;
        for (int i = 0; i < _pool.Count; ++i) {
            var obj = _pool[i];
            if (!obj.activeSelf) {
                gottenObj = obj;
                break;
            }
        }
        if (gottenObj == null) {
            // 没找到空闲，新建
            gottenObj = Instantiate(bulletPrefab, transform);
            _pool.Add(gottenObj);
        }

        Sprite sp = null;
        if (shape == Shape.COIN && isCharacter) {
            sp = characterCoinBullet;
            gottenObj.tag = "CharacterBullet";
        } else if (shape == Shape.HEART && isCharacter) {
            sp = characterHeartBullet;
            gottenObj.tag = "CharacterBullet";
        } else if (shape == Shape.COIN && !isCharacter) {
            sp = enemyCoinBullet;
            gottenObj.tag = "EnemyBullet";
        } else if (shape == Shape.HEART && !isCharacter) {
            sp = enemyHeartBullet;
            gottenObj.tag = "EnemyBullet";
        }
        gottenObj.GetComponent<SpriteRenderer>().sprite = sp;

        var bullet = gottenObj.GetComponent<Bullet>();
        bullet.shape = shape;
        bullet.isFromCharacter = isCharacter;

        gottenObj.SetActive(true);

        return gottenObj;
    }
}
