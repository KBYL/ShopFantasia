using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ComeStoreScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//閉店ボタンを押すをメインメニューに戻る
	public void CloseShopButtonPush () {
		SceneManager.LoadScene ("MainMenu");
	}
}
