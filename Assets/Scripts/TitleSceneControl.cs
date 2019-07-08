﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneControl : MonoBehaviour {
    private bool inTitle, inCredit;
    public GameObject titleObj, creditObj, backObj;
    public string sceneName;
    // public GameObject audioCtrlPrefab;

    public Text difficultyText;

    void Start () {
        inTitle = true;
        inCredit = false;
        AudioInterface.Instance.playBGM (AudioInterface.Instance.TitleBGM);

        UpdateDifficultyText();
    }

    // Update is called once per frame
    void Update () {
        if (inCredit && Input.GetKeyDown (KeyCode.Escape)) {
            quitCredit ();
        }

        if (Input.GetKeyDown(KeyCode.Y)) {
            GameManager.Instance.SwitchDifficult();
            UpdateDifficultyText();
        }
    }

    void UpdateDifficultyText() {
        difficultyText.text = "按 Y 调整难度，当前难度为：" +
            GameManager.Instance.GetCurrentDifficultyName();
    }

    private void quitCredit () {
        inCredit = false;
        inTitle = true;
        backObj.SetActive (false);
        creditObj.SetActive (false);

        titleObj.SetActive(true);
    }

    public void clickStart () {
        AudioInterface.Instance.audioSource.Stop ();
        SceneManager.LoadScene ("OPScene");
    }

    public void clickCredit () {
        inCredit = true;
        inTitle = false;
        creditObj.SetActive (true);
        backObj.SetActive (true);

        titleObj.SetActive(false);
    }
    public void clickExit () {
        Application.Quit ();
    }
    public void clickBack () {
        if (inCredit) {
            quitCredit();
        }
    }
}