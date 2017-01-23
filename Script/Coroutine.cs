/*
using UnityEngine;
using System.Collections;

public class Coroutine : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// コルーチンを実行  
		StartCoroutine ("StageManager");  
	}  

	// コルーチン  
	private IEnumerator StageManager() {  
		// コルーチンの処理  
	}  

	// Update is called once per frame
	void Update () {

	private IEnumerator StageManager() {  
		for (int i = 0; i < 10; i++) {  
			Debug.Log ("i:" + i);  
			yield return new WaitForSeconds (1f);  

			// i==2になったらコルーチン終了  
			if (i == 2) {  
				yield break;  
			}  
		} 
	
	}
*/
