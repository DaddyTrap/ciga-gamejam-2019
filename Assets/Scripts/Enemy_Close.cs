using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Close : Enemy {

    // Start is called before the first frame update
    void Start () {
        onStart ();
    }

    // Update is called once per frame
    void Update () {

    }

    void FixedUpdate () {
        // 调用行为管理器
        enemyAction ();
    }

    // 行为管理器
    protected override void enemyAction () {
        if (isDead) return;
        var currentTime = Time.time;
        bool needFollow = (currentTime - lastFollowTime >= followGap) ? true : false;
        // 跟踪
        if (needFollow) followCharacter ();
    }

    void OnCollisionEnter2D (Collision2D other) {
        string otherTag = other.collider.tag;
        if (otherTag == "Character") {
            death ();
        } else if (otherTag == "CharacterBullet") {
            var bullet = other.collider.GetComponent<Bullet>();
            if (bullet.shape == enemyType) {
                death();
            }
        }
    }

}