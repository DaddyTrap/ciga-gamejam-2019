using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public float baseSpeed = 5.0f;
    public float Speed {
        // TODO: 速度公式
        get { return baseSpeed; }
    }

    private Rigidbody2D rb;

    private Vector2 mouseDirVec;

    void Awake () {
        rb = GetComponent<Rigidbody2D> ();
    }

    // Start is called before the first frame update
    void Start () {

    }

    void FixedUpdate () {
        FaceToMouse ();
        Move ();
        Fire();
    }

    void FaceToMouse () {
        // 转向鼠标所在方向
        var v3 = Input.mousePosition;
        v3.z = 10f;
        var worldPos = Camera.main.ScreenToWorldPoint (v3);

        worldPos.z = transform.position.z;
        mouseDirVec = worldPos - transform.position;
        transform.localEulerAngles = new Vector3 (0, 0, Vector3.SignedAngle (Vector3.up, mouseDirVec, Vector3.forward));
    }

    void Move () {
        // 移动
        float horizontal = 0f, vertical = 0f;
        if (Input.GetKey (KeyCode.W)) {
            vertical = 1f;
        }
        if (Input.GetKey (KeyCode.S)) {
            vertical = -1f;
        }
        if (Input.GetKey (KeyCode.A)) {
            horizontal = -1f;
        }
        if (Input.GetKey (KeyCode.D)) {
            horizontal = 1f;
        }
        var moveDirVec = new Vector2 (horizontal, vertical).normalized;
        var velocity = moveDirVec * baseSpeed;

        rb.velocity = velocity;
    }

    public GameObject triBulletPrefab;
    public GameObject heartBulletPrefab;
    public float bulletBaseSpeed = 10f;
    public float bulletCooldown = 0.333f;
    private float lastFireTime;
    void Fire () {
        // TODO: 对象池
        var curTime = Time.time;
        GameObject bullet = null;
        bool fired = false;
        if (Input.GetButton("Fire1") && (curTime - lastFireTime) > bulletCooldown) {
            fired = true;
            lastFireTime = curTime;
            bullet = Instantiate(triBulletPrefab);
        } else if (Input.GetButton("Fire2") && (curTime - lastFireTime) > bulletCooldown) {
            fired = true;
            lastFireTime = curTime;
            bullet = Instantiate(heartBulletPrefab);
        }

        if (fired) {
            var bulletComp = bullet.GetComponent<Bullet>();
            bulletComp.Init(transform.position, transform.eulerAngles, "CharacterBullet", mouseDirVec.normalized * bulletBaseSpeed);
        }
    }
}
