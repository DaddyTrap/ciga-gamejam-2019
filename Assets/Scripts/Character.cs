using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public float baseSpeed = 5.0f;
    public float Speed {
        // TODO: 速度公式
        get { return baseSpeed; }
    }

    public int maxHp = 9;
    public int hp = 9;
    public float cdAcc = 0.5f;
    public float cdTime = 0f;
    public Shape currentShape {
        get; set;
    } = Shape.TRIANGLE;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 mouseDirVec;

    void Awake () {
        rb = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start () {

    }

    void Update() {
        if (cdTime > 0) {
            cdTime -= Time.deltaTime;
        }
        Trans();
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
        var bulletPool = GameSceneController.instance.bulletPool;
        if (Input.GetButton("Fire1") && (curTime - lastFireTime) > bulletCooldown) {
            fired = true;
            lastFireTime = curTime;
            bullet = bulletPool.Get(Shape.HEART, true);
        } else if (Input.GetButton("Fire2") && (curTime - lastFireTime) > bulletCooldown) {
            fired = true;
            lastFireTime = curTime;
            bullet = bulletPool.Get(Shape.COIN, true);
        }

        if (fired) {
            var bulletComp = bullet.GetComponent<Bullet>();
            bulletComp.Init(transform.position, transform.eulerAngles, "CharacterBullet", mouseDirVec.normalized * bulletBaseSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        var other = collision.collider;
        // Debug.Log(other.tag);
        if (other.tag == "Enemy" || other.tag == "EnemyBullet") {
            // TODO: 判断是否能吸收
            bool absorbable = false;
            if (other.tag == "Enemy") {
                var enemy = other.GetComponent<Enemy>();
                if (enemy.enemyType == currentShape) {
                    absorbable = true;
                }
            } else if (other.tag == "EnemyBullet") {
                var enemyBullet = other.GetComponent<Bullet>();
                if (enemyBullet.shape == currentShape) {
                    absorbable = true;
                }
            }

            if (!absorbable) {
                // 1 点伤害
                BeDamaged(1);
            } else {
                // 吸收
                // TODO: 加 CD，特殊处理 三角形
                cdTime += cdAcc;
            }
        }
    }

    public void BeDamaged(int damage) {
        --hp;
        // TODO: 受击音效/动画

        if (hp <= 0) {
            Death();
        }
    }

    void Death() {
        // TODO: 动画/音效
        gameObject.SetActive(false);
    }

    void Trans() {
        if (Input.GetKeyDown(KeyCode.Space) && cdTime <= 0f) {
            Shape targetShape = Shape.COIN;
            bool originTri = false;
            if (currentShape == Shape.TRIANGLE) {
                originTri = true;
            }
            if (currentShape == Shape.HEART) {
                targetShape = Shape.COIN;
            } else {
                targetShape = Shape.HEART;
            }
            currentShape = targetShape;
            // TODO: 变形动画/音效
            if (originTri) {
                animator.Play("TransT2H", 0, 0f);
            } else {
                if (currentShape == Shape.COIN) {
                    animator.Play("TransH2C", 0, 0f);
                    // GetComponent<SpriteRenderer>().color = Color.yellow;
                } else {
                    animator.Play("TransC2H", 0, 0f);
                    // GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }
    }
}
