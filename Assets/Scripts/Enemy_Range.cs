using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Range : Enemy {
    public float attackGap; // 攻击间隔
    private float lastAttackTime; // 上一次攻击时间
    public GameObject bullet; // 子弹

    // Start is called before the first frame update
    void Start () {
        onStart ();
    }

    // Update is called once per frame
    void Update () {

    }

    void FixedUpdate () {
        // 调用行为
        enemyAction ();
    }

    // 行为管理器
    protected override void enemyAction () {
        var currentTime = Time.time;
        bool needFollow = (currentTime - lastFollowTime >= followGap) ? true : false;
        bool needAttack = (currentTime - lastAttackTime >= attackGap) ? true : false;

        // 跟踪
        if (needFollow) followCharacter ();
        // 攻击
        if (needAttack) fire ();
    }

    public float bulletBaseSpeed;

    // 发射子弹
    void fire () {
        lastAttackTime = Time.time;
        var bulletPool = GameSceneController.instance.bulletPool;
        // Instantiate the bullet
        var bulletObj = bulletPool.Get(enemyType, false);
        // var deg = transform.rotation.z;
        // var dir = new Vector2 (Mathf.Cos(deg * Mathf.Deg2Rad), Mathf.Sin(deg * Mathf.Deg2Rad));
        var dir = (character.transform.position - transform.position).normalized;
        bulletObj.GetComponent<Bullet>().Init(transform.position, transform.eulerAngles, "EnemyBullet", dir * bulletBaseSpeed);
    }

    void OnTriggerEnter2D (Collider2D other) {
        death (other);
    }

}