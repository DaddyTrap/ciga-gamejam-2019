using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : Enemy
{

    private float rotateSpeed = 10.0f;
    void Start () {
        enemyType = Shape.TRIANGLE;
    }
    void FixedUpdate () {
        var angle = transform.localEulerAngles;
        angle.z += rotateSpeed;
        transform.localEulerAngles = angle;
    }
    // 行为
    protected override void enemyAction() {

    }

    void OnCollisionEnter2D (Collision2D other) {
        death (other);
    }
}
