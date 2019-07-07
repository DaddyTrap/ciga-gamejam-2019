using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour {
    public float baseSpeed = 5.0f;
    public float Speed {
        // TODO: 速度公式
        get { return baseSpeed; }
    }

    private bool isDead;
    public int maxSanity = 9;
    private int _sanity = 9;
    public int sanity {
        get {
            return _sanity;
        }
        set {
            _sanity = value;
            GameSceneController.instance.ChangeSanity(_sanity);
        }
    }
    public float cdAcc = 0.5f;
    private float _cdTime = 0f;
    public float cdTime {
        get {
            return _cdTime;
        }

        set {
            _cdTime = value;
            cdBar.value = _cdTime;
            cdText.text = _cdTime.ToString("F2");
        }
    }
    public Shape currentShape {
        get;
        set;
    } = Shape.TRIANGLE;

    public Slider cdBar;
    public Image cdBarFillColor;
    public Color cdBarCoin;
    public Color cdBarHeart;
    public Text cdText;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 mouseDirVec;

    void Awake () {
        rb = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator> ();
    }

    // Start is called before the first frame update
    void Start () {
        isDead = false;
    }

    void Update () {
        if (cdTime > 0) {
            cdTime -= Time.deltaTime;
            if (cdTime < 0) {
                cdTime = 0f;
            }
        }
        Trans ();
    }

    void FixedUpdate () {
        FaceToMouse ();
        Move ();
        Fire ();
    }

    void FaceToMouse () {
        if (isDead) return;
        // 转向鼠标所在方向
        var v3 = Input.mousePosition;
        v3.z = 10f;
        var worldPos = Camera.main.ScreenToWorldPoint (v3);

        worldPos.z = transform.position.z;
        mouseDirVec = worldPos - transform.position;
        transform.localEulerAngles = new Vector3 (0, 0, Vector3.SignedAngle (Vector3.up, mouseDirVec, Vector3.forward));
    }

    void Move () {
        if (isDead) return;
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

    public float bulletBaseSpeed = 10f;
    public float bulletCooldown = 0.333f;
    private float lastFireTime;
    void Fire () {
        if (isDead) return;
        // TODO: 对象池
        var curTime = Time.time;
        GameObject bullet = null;
        bool fired = false;
        var bulletPool = GameSceneController.instance.bulletPool;
        if (Input.GetButton ("Fire1") && (curTime - lastFireTime) > bulletCooldown) {
            AudioInterface.Instance.playSE (AudioInterface.Instance.ShootLeft);
            fired = true;
            lastFireTime = curTime;
            bullet = bulletPool.Get (Shape.HEART, true);
        } else if (Input.GetButton ("Fire2") && (curTime - lastFireTime) > bulletCooldown) {
            AudioInterface.Instance.playSE (AudioInterface.Instance.ShootRight);
            fired = true;
            lastFireTime = curTime;
            bullet = bulletPool.Get (Shape.COIN, true);
        }

        if (fired) {
            var bulletComp = bullet.GetComponent<Bullet> ();
            bulletComp.Init (transform.position, transform.eulerAngles, "CharacterBullet", mouseDirVec.normalized * bulletBaseSpeed);
        }
    }

    void OnCollisionEnter2D (Collision2D collision) {
        if (isDead) return;
        var other = collision.collider;
        // Debug.Log(other.tag);
        OnCollide (other);
    }

    public void OnCollide (Collider2D other) {
        if (other.tag == "Enemy" || other.tag == "EnemyBullet") {
            // 判断是否能吸收
            bool absorbable = false;
            if (other.tag == "Enemy") {
                var enemy = other.GetComponent<Enemy> ();
                if (enemy.enemyType == currentShape) {
                    absorbable = true;
                }
            } else if (other.tag == "EnemyBullet") {
                var enemyBullet = other.GetComponent<Bullet> ();
                if (enemyBullet.shape == currentShape) {
                    absorbable = true;
                }
            }

            if (!absorbable) {
                // 1 点伤害
                BeDamaged (1);
            } else {
                // 吸收
                // TODO: 加 CD，特殊处理 三角形
                if (other.tag == "Enemy") {
                    // 吸收子弹的时候不加CD
                    cdTime += cdAcc;
                }
            }
        }
    }

    public void BeDamaged (int damage) {
        if (isDead) return;
        sanity -= damage;
        // TODO: 受击音效/动画
        AudioInterface.Instance.playSE (AudioInterface.Instance.PlayerHurt);
        GameSceneController.instance.shakeCamera.screenShake(0.2f, 0.2f);
        if (sanity <= 0) {
            isDead = true;
            Death ();
        }
    }

    void Death () {
        // TODO: 动画/音效
        string animClip = "";
        switch (currentShape) {
            case Shape.COIN:
                animClip = "C2Dead";
                break;
            case Shape.HEART:
                animClip = "H2Dead";
                break;
            case Shape.TRIANGLE:
                animClip = "T2Dead";
                break;
            default:
                break;
        }
        playDeath (animClip);

    }

    // 播放死亡动画
    private void playDeath (string s) {
        gameObject.GetComponent<BoxCollider2D> ().isTrigger = true;
        gameObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
        gameObject.GetComponent<Rigidbody2D> ().angularVelocity = 0;
        animator.Play (s);
        delay (() => {
            GameSceneController.instance.shakeCamera.screenShake (0.6f, 0.2f);
            AudioInterface.Instance.playSE (AudioInterface.Instance.DeathSE);
            delay (() => {
                gameObject.SetActive (false);
            }, 1.5f);
        }, 0.6f);
    }

    // 延迟
    public void delay (Action act, float duration) {
        StartCoroutine (_delay (act, duration));
    }

    IEnumerator _delay (Action act, float duration) {
        yield return new WaitForSeconds (duration);
        act ();
    }

    void Trans () {
        if (isDead) return;
        if (Input.GetKeyDown (KeyCode.Space) && cdTime <= 0f) {
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
            if (targetShape == Shape.COIN) {
                cdBarFillColor.color = cdBarCoin;
            } else if (targetShape == Shape.HEART) {
                cdBarFillColor.color = cdBarHeart;
            }
            // TODO: 变形动画/音效
            AudioInterface.Instance.playSE (AudioInterface.Instance.PlayerTransform);
            if (originTri) {
                animator.Play ("TransT2H", 0, 0f);
            } else {
                if (currentShape == Shape.COIN) {
                    animator.Play ("TransH2C", 0, 0f);
                    // GetComponent<SpriteRenderer>().color = Color.yellow;
                } else {
                    animator.Play ("TransC2H", 0, 0f);
                    // GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }
    }
}