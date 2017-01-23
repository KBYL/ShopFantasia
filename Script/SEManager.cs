using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SEManager : MonoBehaviour {

	public AudioSource seAudioSource;
	public List<AudioClip> seAudioClip = new List<AudioClip> ();	//全SEのClipList
	public static bool[] isSeSound;			//音を鳴らすかどうか

	public enum SE_LIST {
		TAP,				//画面をタップした時
		MONEY,				//お金を払ったりもらった時
		CUSTOMER_NOT_BUY,	//客が商品を買わなかった時
		STAGE_CLEAR,		//ステージクリア時
		STAGE_FAILURE,		//ステージクリア失敗時
		WEEK_PASS,			//ゲーム内1週間経った時
		MONTH_PASS,			//ゲーム内1ヶ月経った時
		FIRST_COME_CUSTOMER	//最初の客が店に入った時
	}

	// Use this for initialization
	void Start () {
		isSeSound = new bool[seAudioClip.Count]; 
	}
	
	// Update is called once per frame
	void Update () {
		for ( int i = 0; i < isSeSound.Length; i++ ) {
			if (isSeSound [i] == true) {
				seAudioSource.PlayOneShot(seAudioClip[i]);
				isSeSound [i] = false;
			}
		}
	}
}
