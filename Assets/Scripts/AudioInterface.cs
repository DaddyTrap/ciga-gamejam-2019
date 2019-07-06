using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInterface : MonoBehaviour {
    public AudioSource audioSource;
    public AudioClip MonsterShoot;
    public AudioClip MonsterHit;
    public AudioClip ShootLeft;
    public AudioClip ShootRight;
    public AudioClip PlayerHurt;
    public AudioClip MonsterHurt;
    public AudioClip DeathSE;
    public AudioClip VictorySE;
    public AudioClip PlayerTransform;
    public AudioClip BatShoot;
    public AudioClip TitleBGM;
    public AudioClip CutSceneComicBGM;
    public AudioClip Stage0BGM;
    public AudioClip Stage1BGM;
    public AudioClip TrueEndBGM;
    public AudioClip BadEndBGM;
    public AudioClip GoodEndBGM;
    public void playSE (AudioClip _se) {
        audioSource.PlayOneShot (_se);
    }
    public void playBGM (AudioClip _bgm) {
        audioSource.clip = _bgm;
        audioSource.Play ();
    }
    static AudioInterface _instance;
    public static AudioInterface Instance {
        get {
            if (_instance == null) {
                return null;
            } else {
                return _instance;
            }
        }
    } 
    void Awake () {
        _instance = this;
    }
}