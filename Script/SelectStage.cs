using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SelectStage : MonoBehaviour {

	public int thisStageLevel;
	public bool isCanSelectStage = false;
	public GameObject blackObject;

	// Update is called once per frame
	void Update () {
		if ( thisStageLevel <= StageManager.stageLevelMax) {
			isCanSelectStage = true;
			blackObject.SetActive (false);
		}
	}

	//ステージをタップしたらゲーム画面に飛ぶ
	public void ClickSelectStage() {
		Debug.Log("selectStageClicked");
		if (isCanSelectStage == true) {
			StageManager.stageLevel = thisStageLevel;
			SceneManager.LoadScene ("StoreScreen");
		}
	}
}
