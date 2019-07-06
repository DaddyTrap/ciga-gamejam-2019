using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    public Shape enemyType;
    protected bool isDead; // 标记敌人是否死亡
    public float followSpeed; // 跟踪速度
    public float followGap; // 跟踪间隔
    protected float lastFollowTime; // 上一次跟踪时间
    public GameObject character; // 玩家信息
    Rigidbody2D rb; // 敌人刚体
    public Animator anim; // 敌人动画控制器
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
        anim.Play ("Walk", 0, 0.0f);
        Rigidbody2D rb = transform.GetComponent<Rigidbody2D> ();
        lastFollowTime = Time.time;
        var originalPosition = transform.position;
        var targetPosition = character.transform.position;
        var offset = targetPosition - originalPosition;
        var direction = offset.normalized;
        // 面向玩家
        transform.localEulerAngles = new Vector3 (0, 0, Vector3.SignedAngle (Vector3.up, direction, Vector3.forward));
        // 位移
        rb.velocity = direction * followSpeed;
    }
    // 死亡
    protected void death () {
        anim.Play ("Dead", 0, 0.0f);
        isDead = true;
        // Destroy the enemy
        delay (() => {
            gameObject.SetActive (false);
        }, 0.433f);
    }
    // 设置类型
    public void setEnemyType (Shape _enemyType, Sprite _enemySprite, RuntimeAnimatorController _anim) {
        var enemySprite = this.GetComponent<SpriteRenderer> ().sprite = _enemySprite;
        enemyType = _enemyType;
        anim.runtimeAnimatorController = _anim;
    }

    // 延迟
    public void delay (Action act, float duration) {
        StartCoroutine (_delay (act, duration));
    }

    IEnumerator _delay (Action act, float duration) {
        yield return new WaitForSeconds (duration);
        act ();
    }

    public void BeDamaged() {
        death();
    }
}