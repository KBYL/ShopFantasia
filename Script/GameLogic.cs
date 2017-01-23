using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

	public enum PLAY_STATUS {
		NORMAL,
		STAGE_NAME_CALL,
		PURCHASE_SELECT,
		RESULT,
		CONVERSATION,
		EVENT_CONVERSATION,
		PRICE_CONFIG,

	};

	private UIManager uiManagerScript;
	private GameTimer gameTimerScript;
	[SerializeField]
	public GameObject resultCanvas;
	private Result resultScript;
	private StageManager stageManagerScript;
	public GameObject goBackCustomerObj;
	private Product productScript;
	private DeskOnImageManager deskOnImageManagerScript;

	public const int FPS = 60;
	public int gameStatus = (int)PLAY_STATUS.NORMAL;

	void Awake() {
		uiManagerScript = GameObject.Find ("UIManager").GetComponent<UIManager> ();
		gameTimerScript = GameObject.Find ("Main Camera").GetComponent<GameTimer> ();
		resultScript = resultCanvas.GetComponent<Result> ();
		stageManagerScript = GameObject.Find ("StageManager").GetComponent<StageManager> ();
		productScript = GameObject.Find ("StageManager").GetComponent<Product> ();
		deskOnImageManagerScript = GameObject.Find ("Canvas").GetComponent<DeskOnImageManager> ();
	}

	// Use this for initialization
	void Start () {
		InitGame ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	//ゲームの初期化
	public void InitGame() {
		gameTimerScript.time = GameTimer.MAX_TIME;
		gameTimerScript.endMouthTime = stageManagerScript.stageData [StageManager.stageLevel].week;
		gameStatus = (int)PLAY_STATUS.STAGE_NAME_CALL;
		uiManagerScript.stageNameDrawFinish = false;
		uiManagerScript.money = stageManagerScript.stageData [StageManager.stageLevel].firstMoney;
		uiManagerScript.target = stageManagerScript.stageData [StageManager.stageLevel].targetMoney;
		productScript.buyProduct.Clear ();
		deskOnImageManagerScript.SetDeskImage ();

		resultScript.InitResult ();
	}

	//週の初期化
	public void InitWeek() {
		Debug.Log ("InitWeek in");
		uiManagerScript.stageNameDrawFinish = false;

		uiManagerScript.purchaseOKButton.GetComponent<PurchaseManager> ().isPurchaseOKButtonPush = false;
		GameTimer.score = 0;
	}



}
