using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Range : Enemy {
    bool firing;
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
        if (isDead) return;
        var currentTime = Time.time;
        bool needFollow = (currentTime - lastFollowTime >= followGap) ? true : false;
        bool needAttack = (currentTime - lastAttackTime >= attackGap) ? true : false;
        // 攻击
        if (needAttack) {
            firing = true;
            this.GetComponent<Collider2D> ().isTrigger = true;
            this.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
            fire ();
            // delay (() => {
            //     if (isDead) return;
            //     fire ();
            //     delay (() => {
            //         if (isDead) return;
            //         fire ();
            //         delay (() => {
            //             this.GetComponent<Collider2D> ().isTrigger = false;
            //             firing = false;
            //             return;
            //         }, 0.5f);
            //     }, 0.7f);
            // }, 0.7f);
            delay(()=>{
                if (isDead) return;
                this.GetComponent<Collider2D> ().isTrigger = false;
                firing = false;
                return;
            }, 0.5f);
        }
        // 跟踪
        if (needFollow && !firing) followCharacter ();

    }

    public float bulletBaseSpeed;

    // 发射子弹
    void fire () {
        if (enemyType == Shape.COIN) {
            AudioInterface.Instance.playSE (AudioInterface.Instance.MonsterShoot);
        } else {
            AudioInterface.Instance.playSE (AudioInterface.Instance.BatShoot);
        }
        lastAttackTime = Time.time;
        var originalPosition = transform.position;
        var targetPosition = character.transform.position;
        var offset = targetPosition - originalPosition;
        var direction = offset.normalized;
        // 面向玩家
        transform.localEulerAngles = new Vector3 (0, 0, Vector3.SignedAngle (Vector3.up, direction, Vector3.forward));
        anim.Play ("Attack", 0, 0.0f);
        var bulletPool = GameSceneController.instance.bulletPool;
        // Instantiate the bullet
        var bulletObj = bulletPool.Get (enemyType, false);
        // var deg = transform.rotation.z;
        // var dir = new Vector2 (Mathf.Cos(deg * Mathf.Deg2Rad), Mathf.Sin(deg * Mathf.Deg2Rad));
        var dir = (character.transform.position - transform.position).normalized;
        bulletObj.GetComponent<Bullet> ().Init (transform.position, transform.eulerAngles, "EnemyBullet", dir * bulletBaseSpeed);
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

    void OnTriggerEnter2D (Collider2D other) {
        string otherTag = other.tag;
        if (otherTag == "Character") {
            death ();
        } else if (otherTag == "CharacterBullet") {
            var bullet = other.GetComponent<Bullet>();
            if (bullet.shape == enemyType) {
                death();
            }
        }
    }

}