using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Range : MonoBehaviour {
    public bool isHeart; // 标记是否为心型敌人
    private bool isDead; // 标记敌人是否死亡
    public float followSpeed; // 跟踪速度
    public float followGap; // 跟踪间隔
    private float lastFollowTime; // 上一次跟踪时间
    public float attackGap; // 攻击间隔
    private float lastAttackTime; // 上一次攻击时间
    public GameObject character; // 玩家信息
    public GameObject bullet; // 子弹

    // Start is called before the first frame update
    void Start () {
        isDead = false;
        lastFollowTime = lastAttackTime = 0;
    }

    // Update is called once per frame
    void Update () {

    }

    void FixedUpdate () {
        // 调用行为
        enemyAction ();
    }
    // 设置玩家
    public void setCharacter (GameObject _character) {
        character = _character;
    }

    // 行为管理器
    void enemyAction () {
        var currentTime = Time.time;
        bool needFollow = (currentTime - lastFollowTime >= followGap) ? true : false;
        bool needAttack = (currentTime - lastAttackTime >= attackGap) ? true : false;

        // 跟踪
        if (needFollow) followCharacter ();
        // 攻击
        if (needAttack) fire ();
    }

    // 跟踪行为
    void followCharacter () {
        lastFollowTime = Time.time;
        Rigidbody2D rb = transform.GetComponent<Rigidbody2D> ();
        var originalPosition = transform.position;
        var targetPosition = character.transform.position;
        var offset = targetPosition - originalPosition;
        var direction = offset.normalized;
        // 面向玩家
        transform.localEulerAngles = new Vector3 (0, 0, Vector3.SignedAngle (Vector3.up, direction, Vector3.forward));
        // 位移
        rb.velocity = direction * followSpeed * Time.deltaTime;
    }

    // 发射子弹
    void fire () {
        lastAttackTime = Time.time;
        // TODO : Instantiate the bullet
    }

    void OnTriggerEnter2D (Collider2D other) {
        string otherTag = other.tag;
        Debug.Log (otherTag);
        if (otherTag == "Character" || otherTag == "CharacterBullet") {
            isDead = true;
            // TODO : Destroy the enemy
            Destroy (this);
        }
    }

}