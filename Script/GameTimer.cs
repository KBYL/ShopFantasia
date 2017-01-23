using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {

	public Text timeText;				//残り時間を写すテキスト
	public Text endMouthTimeText;		//残りの週を写すテキスト

	public static int score = 0;		//経過時間
	public const int MAX_TIME = 60;		//残り時間の最大値
	public int time = MAX_TIME;				//残り時間
	public int endMouthTime = 2;		//残りの週

	[SerializeField]
	public bool isTimerStop = true;		//タイマーを進めるかどうか

	private GameLogic gameLogicScript;

	void Awake() {
		gameLogicScript = GameObject.Find ("Main Camera").GetComponent<GameLogic> ();
	}

	void Start (){
		timeText.text = "0";
	}

	void Update (){
		//各テキストに文字を入れる
		timeText.text = "" + time.ToString();
		endMouthTimeText.text = (endMouthTime - 1).ToString();

		//タイマーが動いていいなら
		if (isTimerStop == false) {
			//タイマーを進ませる
			score++;
			time = MAX_TIME - score / GameLogic.FPS;
		}

		if (time <= 0) {
			time = 0;
		}

		//残り時間が0になったら
		if (time == 0 && gameLogicScript.gameStatus != (int)GameLogic.PLAY_STATUS.CONVERSATION && gameLogicScript.gameStatus != (int)GameLogic.PLAY_STATUS.PRICE_CONFIG) {
			SEManager.isSeSound [(int)SEManager.SE_LIST.WEEK_PASS] = true;
			endMouthTime -= 1;
			if (endMouthTime <= -1) {
				endMouthTime = -1;
			}
		}
	}

	//デバッグ用　押すとこの週の時間が終わる
	public void ClickFinishTime() {
		GameTimer.FinishTimer();
	}

	//タイマーを0に進める
	public static void FinishTimer() {
		score = 3600;
	}

}

