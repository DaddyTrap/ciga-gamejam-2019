using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public float duration, magnitude;
    public Vector3 cameraPosition;

    // 屏幕震动接口
    public void screenShake () {
        StartCoroutine ( Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude) {
        float elapsed = 0;

        while (elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3 (cameraPosition.x + x, cameraPosition.y + y, -10);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.position = cameraPosition;
    }
}
