using UnityEngine;

public class Bullet : MonoBehaviour {
    public void Init(Vector3 pos, Vector3 eulerAngle, string tag, Vector2 velocity) {
        transform.localPosition = pos;
        transform.localEulerAngles = eulerAngle;
        transform.tag = tag;
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    private Rigidbody2D rb;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
}
