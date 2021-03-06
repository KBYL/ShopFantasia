using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager: MonoBehaviour {
	//各キャンバスを参照する
	[SerializeField]
	private GameObject bigStageNameCanvas;

	[SerializeField]
	private GameObject characterCanvas;

	[SerializeField]
	private GameObject purchaseCanvas;

	[SerializeField]
	public GameObject conversationCanvas;

	[SerializeField]
	public GameObject priceConfigCanvas;

	[SerializeField]
	public GameObject resultCanvas;

	public Image bigStageNameImage;
	public Image stageNameImage;
	public GameObject bigStageNameText;
	public Text moneyText;
	public Text targetText;
	public int money = 500;
	public int target = 500;
	public bool isPurchaseIn = false;
	public bool stageNameDrawFinish = false;
	public GameObject purchaseOKButton;
	[SerializeField]
	public GameObject purchaseManagerObj;

	private GameLogic gameLogicScript;
	private GameTimer gameTimerScript;
	private StageManager stageManagerScript;
	private CustomerInStore customerInStoreScript;
	public PurchaseManager purchaseManagerScript;

	void Awake() {
		gameLogicScript = GameObject.Find ("Main Camera").GetComponent<GameLogic> ();
		gameTimerScript = GameObject.Find ("Main Camera").GetComponent<GameTimer> ();
		stageManagerScript = GameObject.Find ("StageManager").GetComponent<StageManager> ();
		customerInStoreScript = GameObject.Find ("CharacterManager").GetComponent<CustomerInStore> ();
		purchaseManagerScript = purchaseManagerObj.GetComponent<PurchaseManager> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//各数値を入れる
		moneyText.text = money >= 0 ? money.ToString () : "お金が足りないよ";
		targetText.text = "" + target.ToString ();

		//各gameStatusによるUIを動かす
		switch ((int)gameLogicScript.gameStatus) {
		case (int)GameLogic.PLAY_STATUS.NORMAL:
			customerInStoreScript.isStopPlayer = true;
			//タイムが0になったら
			if (gameTimerScript.time == 0) {

				//ステージコールの処理
				if (stageNameDrawFinish == true) {
					isPurchaseIn = true;
				} else {
					gameLogicScript.gameStatus = (int)GameLogic.PLAY_STATUS.STAGE_NAME_CALL;
				}

				//どのPLAY_STATUSに行くか
				if (isPurchaseIn == true && gameLogicScript.gameStatus != (int)GameLogic.PLAY_STATUS.CONVERSATION) {
					isPurchaseIn = true;
					if (gameTimerScript.endMouthTime == 0) {
						SEManager.isSeSound [(int)SEManager.SE_LIST.MONTH_PASS] = true;
						gameLogicScript.gameStatus = (int)GameLogic.PLAY_STATUS.RESULT;
					} else {
						gameLogicScript.gameStatus = (int)GameLogic.PLAY_STATUS.PURCHASE_SELECT;
					}
				}
				purchaseManagerScript.ResetPurchase ();
				InitWeek ();
			}
			SetBasicCanvas ();
			break;

		case (int)GameLogic.PLAY_STATUS.STAGE_NAME_CALL:
			StartCoroutine( "SetBigStageName", 2.0f);
			break;

		case (int)GameLogic.PLAY_STATUS.PURCHASE_SELECT:
			SetPurchaseSelectCanvas ();
			break;

		case (int)GameLogic.PLAY_STATUS.RESULT:
			SetResulutCanvas ();
			break;

		case (int)GameLogic.PLAY_STATUS.CONVERSATION:
			SetConversationCanvas ();
			gameTimerScript.isTimerStop = false;
			break;

		case (int)GameLogic.PLAY_STATUS.EVENT_CONVERSATION:
			SetConversationCanvas ();
			gameTimerScript.isTimerStop = true;
			break;

		case (int)GameLogic.PLAY_STATUS.PRICE_CONFIG:
			SetPriceConfigCanvas ();
			break;
		}
	}

	private void SetBasicCanvas( ) {
		customerInStoreScript.isStopPlayer = false;
		gameTimerScript.isTimerStop = false;
		bigStageNameCanvas.SetActive (false);
		characterCanvas.SetActive (true);
		purchaseCanvas.SetActive (false);
		conversationCanvas.GetComponent<Canvas>().enabled = false;
		resultCanvas.SetActive(false);
		priceConfigCanvas.SetActive(false);
	}


	private void SetBigStageNameCanvas( ) {
		customerInStoreScript.isStopPlayer = true;
		gameTimerScript.isTimerStop = true;
		bigStageNameCanvas.SetActive (true);
		characterCanvas.SetActive (false);
		purchaseCanvas.SetActive (false);
		conversationCanvas.GetComponent<Canvas>().enabled = false;
		resultCanvas.SetActive(false);
		priceConfigCanvas.SetActive(false);
	}

	private void SetPurchaseSelectCanvas( ) {
		customerInStoreScript.isStopPlayer = true;
		gameTimerScript.isTimerStop = true;
		bigStageNameCanvas.SetActive (false);
		characterCanvas.SetActive (false);
		purchaseCanvas.SetActive (true);
		conversationCanvas.GetComponent<Canvas>().enabled = false;
		resultCanvas.SetActive(false);
		priceConfigCanvas.SetActive(false);
	}

	private void SetResulutCanvas( ) {
		customerInStoreScript.isStopPlayer = true;
		gameTimerScript.isTimerStop = true;
		bigStageNameCanvas.SetActive (false);
		characterCanvas.SetActive (false);
		purchaseCanvas.SetActive (false);
		conversationCanvas.GetComponent<Canvas>().enabled = false;
		resultCanvas.SetActive(true);
		priceConfigCanvas.SetActive(false);
	}

	private void SetConversationCanvas( ) {
		customerInStoreScript.isStopPlayer = true;
		bigStageNameCanvas.SetActive (false);
		characterCanvas.SetActive (true);
		purchaseCanvas.SetActive (false);
		conversationCanvas.GetComponent<Canvas>().enabled = true;
		resultCanvas.SetActive(false);
		priceConfigCanvas.SetActive(false);
	}

	private void SetPriceConfigCanvas ( ) {
		customerInStoreScript.isStopPlayer = true;
		gameTimerScript.isTimerStop = false;
		bigStageNameCanvas.SetActive (false);
		characterCanvas.SetActive (true);
		purchaseCanvas.SetActive (false);
		conversationCanvas.GetComponent<Canvas>().enabled = false;
		resultCanvas.SetActive(false);
		priceConfigCanvas.SetActive(true);
	}


	private IEnumerator SetBigStageName( float stopTime ) {
		//ステージに入った時のステージ名
		bigStageNameImage.sprite = Resources.Load<Sprite>("Image/UI/" + stageManagerScript.stageData[StageManager.stageLevel].bigStageNameID);
		stageNameImage.sprite = Resources.Load<Sprite> ("Image/UI/" + stageManagerScript.stageData [StageManager.stageLevel].UIStageNameID);

		SetBigStageNameCanvas ();
		yield return new WaitForSeconds (stopTime);  

		stageNameDrawFinish = true;
		gameLogicScript.gameStatus = (int)GameLogic.PLAY_STATUS.EVENT_CONVERSATION;

	}

	public void InitWeek() {
		stageNameDrawFinish = false;

		purchaseOKButton.GetComponent<PurchaseManager> ().isPurchaseOKButtonPush = false;
		GameObject.Find("CharacterManager").GetComponent<GoBackCustomer>().AllDestroyCustomer();
		GameTimer.score = 0;
	}
}
