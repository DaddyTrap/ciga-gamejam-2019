using UnityEngine;

public class Bullet : MonoBehaviour {
    public void Init(Vector3 pos, Vector3 eulerAngle, string tag, Vector2 velocity) {
        transform.localPosition = pos;
        transform.localEulerAngles = eulerAngle;
        transform.tag = tag;
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    private Rigidbody2D rb;
    public bool isFromCharacter { get; set; }
    public Shape shape { get; set; }

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        // Debug.Log(collider.tag);
        if (!isFromCharacter && collider.tag == "Character") {
            // TODO: 触碰玩家，播放特效，造成伤害
            var character = collider.GetComponent<Character>();
            character.OnCollide(collider);
            gameObject.SetActive(false);
        } else if (isFromCharacter && collider.tag == "Enemy") {
            // TOOD: 触碰敌人，播放特效，造成伤害
            var enemy = collider.GetComponent<Enemy>();
            if (enemy.enemyType == shape) {
                enemy.BeDamaged();
            }
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.tag == "Bound") {
            // 离开边界，自然销毁
            gameObject.SetActive(false);
        }
    }
}
