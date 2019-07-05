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
        var currentTime = Time.time;
        bool needFollow = (currentTime - lastFollowTime >= followGap) ? true : false;
        // 跟踪
        if (needFollow) followCharacter ();
    }

    void OnTriggerEnter2D (Collider2D other) {
        death (other);
    }

}