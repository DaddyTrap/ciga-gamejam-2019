using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    public bool isHeart; // 标记是否为心型敌人
    protected bool isDead; // 标记敌人是否死亡
    public float followSpeed; // 跟踪速度
    public float followGap; // 跟踪间隔
    protected float lastFollowTime; // 上一次跟踪时间
    public GameObject character; // 玩家信息
    Rigidbody2D rb; // 敌人刚体
    protected void onStart () {
        isDead = false;
    }
    // 设置玩家
    public void setCharacter (GameObject _character) {
        character = _character;
    }
    // 行为管理器
    protected abstract void enemyAction ();
    // 跟踪行为
    protected void followCharacter () {
        Rigidbody2D rb = transform.GetComponent<Rigidbody2D> ();
        lastFollowTime = Time.time;
        var originalPosition = transform.position;
        var targetPosition = character.transform.position;
        var offset = targetPosition - originalPosition;
        var direction = offset.normalized;
        // 面向玩家
        transform.localEulerAngles = new Vector3 (0, 0, Vector3.SignedAngle (Vector3.up, direction, Vector3.forward));
        // 位移
        rb.velocity = direction * followSpeed * Time.deltaTime;
    }
    // 死亡
    protected void death (Collider2D other) {
        Debug.Log(other.tag);
        string otherTag = other.tag;
        if (otherTag == "Character" || otherTag == "CharacterBullet") {
            isDead = true;
            // TODO : Destroy the enemy
            Destroy (this.gameObject);
        }
    }
}