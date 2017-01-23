using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour {

	public GameObject proceedsObj;			//売上
	public Text proceedsText;			//売上のテキスト
	public GameObject sellProductNumObj;	//商品を売った個数
	public Text sellProductNumText;		//商品を撃った個数のテキスト
	public GameObject targetObj;			//目標
	public Text targetText;				//目標のテキスト
	public GameObject gameClear;		//ゲームクリア時の画像
	public GameObject gameOver;			//ゲームオーバー時の画像
	public GameObject tapCollision;		//タッチ判定
	private int countTime = 0;			//経過時間

	private const int RESULT_INTERVAL = 45;	//各成績を表示させる間隔
	[SerializeField]
	private int activeCount = 0;		//どこの成績を表示させるかの段階

	public int sellCount = 0;			//売れた個数
	public int proceedsMoney = 0;		//売り上げ金額
	public bool isStageClear = false;	//ステージをクリアしたか（目標金額に到達したか）

	private UIManager uiManagerScript;

	void Awake() {
		uiManagerScript = GameObject.Find ("UIManager").GetComponent<UIManager> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		countTime++;

		//成績をINTERVALフレームごとに表示する
		if (countTime % RESULT_INTERVAL == 0) {
			switch (activeCount) {
			case 0:
				proceedsObj.SetActive (true);
				//proceedsText.text = proceedsMoney.ToString ();
				proceedsText.text = uiManagerScript.money.ToString ();
				activeCount++;
				
				break;

			case 1:
				sellProductNumObj.SetActive (true);
				sellProductNumText.text = sellCount.ToString ();
				activeCount++;
				break;

			case 2:
				targetObj.SetActive (true);
				targetText.text = uiManagerScript.target.ToString();
				activeCount++;
				break;

			case 3:
				if ( uiManagerScript.money >= uiManagerScript.target) {
					gameClear.SetActive (true);
					isStageClear = true;
					SEManager.isSeSound [(int)SEManager.SE_LIST.STAGE_CLEAR] = true;
				} else {
					gameOver.SetActive (true);
					SEManager.isSeSound [(int)SEManager.SE_LIST.STAGE_FAILURE] = true;
				}
				tapCollision.SetActive (true);
				activeCount++;
				break;

			default:
				break;
			}

		}
	}

	//画面をクリックしてステージ選択画面に戻る
	public void resultScreenClick() {
		NextStageLevel ();

		SceneManager.LoadScene ("StageSelect");
	}

	public void InitResult() {
		activeCount = 0;
		sellCount = 0;
		proceedsMoney = 0;
		isStageClear = false;
		proceedsObj.SetActive (false);
		sellProductNumObj.SetActive (false);
		targetObj.SetActive (false);
		gameClear.SetActive (false);
		gameOver.SetActive (false);
	}

	public void NextStageLevel() {
		Result resultScript = GameObject.Find ("ResultCanvas").GetComponent<Result> ();

		if (resultScript.isStageClear == true) {
			//正規仕様
			//StageManager.stageLevel++;

			//we are 仕様
			switch(StageManager.stageLevel) {
			case 1:
				StageManager.stageLevel = 3;
				break;
			case 3:
				StageManager.stageLevel = 8;
				break;
			case 8:
				StageManager.stageLevel = 9;
				break;
			case 9:
				break;
			}
			if (StageManager.stageLevelMax <= StageManager.stageLevel) {
				StageManager.stageLevelMax = StageManager.stageLevel;
			}
			resultScript.isStageClear = false;
		}
	}


}