using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class OPSceneController : MonoBehaviour {
    [System.Serializable]
    public class _OPCut {
        public Sprite image;
        [Multiline]
        public string subtitle;
        public float duration;
    }
    public GameObject imageObj;
    public GameObject subTitleObj;
    public _OPCut[] cuts;
    private bool isContinue, isFadingIn, isFadingOut, isStandingStill;

    void Start () {
        isContinue = true;
        if (SceneManager.GetActiveScene ().name == "GoodEnd") {
            AudioInterface.Instance.playBGM (AudioInterface.Instance.GoodEndBGM);
        } else {
            AudioInterface.Instance.playBGM (AudioInterface.Instance.CutSceneComicBGM, false);
        }
        showCut (0);
    }
    void Update () {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            isContinue = false;
        }
        if (isFadingIn) {
            var color = imageObj.GetComponent<Image> ().color;
            color.a += Time.deltaTime;
            imageObj.GetComponent<Image> ().color = color;
            subTitleObj.GetComponent<Text> ().color = color;
        }
        if (isFadingOut) {
            var color = imageObj.GetComponent<Image> ().color;
            color.a -= Time.deltaTime;
            imageObj.GetComponent<Image> ().color = color;
            subTitleObj.GetComponent<Text> ().color = color;
        }
    }

    private void showCut (int index) {
        if (index >= cuts.Length || !isContinue) {
            AudioInterface.Instance.audioSource.Stop ();
            if (SceneManager.GetActiveScene ().name == "GoodEnd") {
                SceneManager.LoadScene ("TitleScene");
                return;
            } else {
                SceneManager.LoadScene ("GameScene");
                return;
            }
        }
        // fade in
        isFadingIn = true;
        isFadingOut = isStandingStill = false;
        imageObj.GetComponent<Image> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
        subTitleObj.GetComponent<Text> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
        imageObj.GetComponent<Image> ().sprite = cuts[index].image;
        subTitleObj.GetComponent<Text> ().text = cuts[index].subtitle;
        delay (() => {
            // stay still
            isFadingIn = isFadingOut = false;
            isStandingStill = true;
            delay (() => {
                // fade out
                isFadingOut = true;
                isFadingIn = isStandingStill = false;
                delay (() => {
                    showCut (index + 1);
                }, 1.0f);
            }, cuts[index].duration);
        }, 1.0f);
    }
    // 延迟
    public void delay (Action act, float duration) {
        StartCoroutine (_delay (act, duration));
    }

    IEnumerator _delay (Action act, float duration) {
        yield return new WaitForSeconds (duration);
        act ();
    }
}