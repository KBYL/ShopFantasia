using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConversationManager : MonoBehaviour {



	//客と仕入れ商人との会話の型
	public struct SERIF	{
		public string serifId;
		public string npcId;
		public string face;
		public string serif;
		public SERIF ( string SerifId, string NpcId, string Face, string Serif ) {
			serifId = SerifId;
			npcId = NpcId;
			face = Face;
			serif = Serif;
		}
	};

	//主に幼なじみとしゃべるイベント会話の型
	public struct EVENT_SERIF {
		public int stage;
		public int num;
		public string playerFace;
		public string npcFace;
		public string serif;
		public bool eventEnd;
		public EVENT_SERIF ( int Stage, int Num, string PlayerFace, string NpcFace, string Serif, bool EventEnd ) {
			stage = Stage;
			num = Num;
			playerFace = PlayerFace;
			npcFace = NpcFace;
			serif = Serif;
			eventEnd = EventEnd;
		}
	};

	//仕入れ商人との会話パターン
	private enum CONVERSATION_CASE_PURCHASE {
		BEGIN_STAGE,	//ステージ開始時
		ONE_WEEK,		//1週間経った時
		INTRODUCTION,	//紹介時
		BUY,			//商品を仕入れた時
		NOT_BUY			//仕入れなかった時
	};

	//客との会話パターン
	private enum CONVERSATION_CASE {
		WELCOME,	//会話開始時
		PLEASE_PRODUCT,	//商品をねだる時
		HIGH_PRICE,		//値段が高い時
		BUY,			//商品を買った時
		GO_BACK,		//帰る時
		PRICE,			//値段提示
		REPRICE,		//値段再提示
		SELL,			//売れた時
		SOLD_OUT		//品切れ時
	}

	//会話テキストを表示するテキスト
	[SerializeField] Text uiText;

	//文字を表示するスピード
	[SerializeField][Range(0.001f, 0.3f)]
	float intervalForCharacterDisplay = 0.05f;

	//会話画面のプレイヤーの画像(左固定)
	[SerializeField]
	public Image playerImage;

	//会話画面のプレイヤーと会話しているキャラの画像(右固定)
	[SerializeField]
	public Image customerImage;

	//NpcIdから参照して会話メッセージを格納するリスト
	[SerializeField]
	public List<SERIF> tempSerifData = new List<SERIF>();

	//会話画面に使用する透明な画像
	private Sprite noneImage;

	[SerializeField]
	private string currentText = string.Empty;	//表示する文章
	private float timeUntilDisplay = 0;			//文章の表示完了関連
	private float timeElapsed = 1;				//文章の表示完了関連
	private int lastUpdateCharacter = -1;		//文章の表示完了関連
	[SerializeField]
	private int currentLine = 0;				//表示する文章の行
	public bool isConversationEnd = false;		//会話が終了したか
	private bool conversationActive = false;	//会話キャンバスのアクティブ
	private bool oldConversationActive = false;	//会話キャンバスの1F前のアクティブ
	private bool isPriceConfigFlag = false;		//trueで値段設定画面に移行する
	private bool isSetEventConversationCurrentLine = false;	//イベント会話のcurrentLineを設定したか

	int weAreStageLevel = StageManager.stageLevel;	//we are 仕様に耐えるためstageLevelを0123にする


	public List<SERIF> serifData = new List<SERIF>();						//客との会話メッセージのリスト
	public List<EVENT_SERIF> eventSerifData = new List<EVENT_SERIF> ();		//イベント会話メッセージのリスト

	public GameObject inPriceConfigScript;

	private UIManager uiManagerScript;
	private CustomerService customerServiceScript;
	private Product productScript;
	private DeskOnImageManager deskOnImageManagerScript;
	private GameLogic gameLogicScript;
	private CustomerClick customerClickScript;
	private PriceConfig priceConfigScript;
	public GameObject resultCanvas;
	private Result resultScript;
	private GoBackCustomer goBackCustomerScript;

	// 文字の表示が完了しているかどうか
	public bool IsCompleteDisplayText {
		get { return  Time.time > timeElapsed + timeUntilDisplay; }
	}

	void Awake() {
		customerServiceScript = GameObject.Find ("CharacterManager").GetComponent<CustomerService> ();
		productScript = GameObject.Find("StageManager").GetComponent<Product>();
		gameLogicScript = GameObject.Find ("Main Camera").GetComponent<GameLogic> ();
		customerClickScript = GameObject.Find ("Main Camera").GetComponent<CustomerClick> ();
		priceConfigScript = inPriceConfigScript.GetComponent<PriceConfig> ();
		uiManagerScript = GameObject.Find ("UIManager").GetComponent<UIManager> ();
		resultScript = resultCanvas.GetComponent<Result> ();
		deskOnImageManagerScript = GameObject.Find ("Canvas").GetComponent<DeskOnImageManager> ();
		noneImage = Resources.Load<Sprite>("Image/none");
		goBackCustomerScript = GameObject.Find ("CharacterManager").GetComponent<GoBackCustomer> ();
	}

	void Start() {
		Entity_SerifData entitySerifData = Resources.Load ("Data/SerifData") as Entity_SerifData;
		Entity_EventSerifData entityEventSerifData = Resources.Load ("Data/EventSerifData") as Entity_EventSerifData;


		for (int i = 0; i < entityEventSerifData.param.Count; i++) {
			eventSerifData.Add (new EVENT_SERIF (
				entityEventSerifData.param[i].stage,
				entityEventSerifData.param[i].num,
				entityEventSerifData.param[i].playerFace,
				entityEventSerifData.param[i].npcFace,
				entityEventSerifData.param[i].serif,
				entityEventSerifData.param[i].eventEnd)
			);
		}


		for (int i = 0; i < entitySerifData.param.Count; i++) {
			serifData.Add (new SERIF (
				entitySerifData.param [i].id,
				entitySerifData.param [i].npcid,
				entitySerifData.param [i].face,
				entitySerifData.param [i].serif)
			);

		}

		SetNextLine();

		switch(StageManager.stageLevel){
		case 0:
			weAreStageLevel = 0;
			break;
		case 2:
			weAreStageLevel = 1;
			break;
		case 7:
			weAreStageLevel = 2;
			break;
		case 8:
			weAreStageLevel = 3;
			break;
		}
	}


	void Update () {
		oldConversationActive = conversationActive;
		conversationActive = GetConversationActive ();

		//ConversationCanvasがアクティブに変わった瞬間
		if (oldConversationActive == false && conversationActive == true) {
			SetNextLine ();
		}


		//セリフの最後を表示した後
		if (Input.GetMouseButtonDown (0)) {
			if (isConversationEnd == true && IsCompleteDisplayText == true) {
				//gameStatusによって動作を変える
				//イベント会話が終わったら
				if (gameLogicScript.gameStatus == (int)GameLogic.PLAY_STATUS.EVENT_CONVERSATION) {
					//デバッグ用　デバッグボタンを消す
					GameObject obj = GameObject.Find ("ConversationButton");
					obj.GetComponent<Button> ().enabled = false;
					obj.GetComponent<Image> ().enabled = false;
					obj.GetComponentInChildren<Text> ().enabled = false;
					//デバッグ用　ここまで

					//仕入れ画面に移行する
					gameLogicScript.gameStatus = (int)GameLogic.PLAY_STATUS.PURCHASE_SELECT;

				//客との会話が終わったら
				} else if (gameLogicScript.gameStatus == (int)GameLogic.PLAY_STATUS.CONVERSATION) {
					//商品が売れたなら
					if (customerServiceScript.isSell == true) {
						Debug.Log ("buyproductcount " + productScript.buyProduct.Count);
						//売り上げを得て、商品は消える
						for (int i = 0; i < productScript.buyProduct.Count; i++) {
							if (customerServiceScript.productName == productScript.buyProduct [i].productName) {
								//売り上げを所持金に加算する
								uiManagerScript.money += customerServiceScript.price;
								SEManager.isSeSound [(int)SEManager.SE_LIST.MONEY] = true;
								//リザルト画面の変数処理
								resultScript.sellCount++;
								resultScript.proceedsMoney += customerServiceScript.price;

								//買われた商品をlistから消す
								productScript.buyProduct.RemoveAt (i);

								//全て商品が売れたら
								if (productScript.buyProduct.Count <= 0) {
									GameTimer.FinishTimer ();
								}

								//for文を抜ける
								i = productScript.buyProduct.Count;
							}
						}
						//机の上の画像の処理をする
						deskOnImageManagerScript.SetDeskImage ();
					} else {
						//売れなかったら
						SEManager.isSeSound [(int)SEManager.SE_LIST.CUSTOMER_NOT_BUY] = true;
					}
					//客を消す
					goBackCustomerScript.DestoryCustomer (customerClickScript.npcId);

					//通常画面に移行する
					gameLogicScript.gameStatus = (int)GameLogic.PLAY_STATUS.NORMAL;
				}

				//会話処理の初期化
				InitConversation ();

				//客のクリック判定を有効にする
				customerClickScript.enabled = true;

			}
		}


		// 文字の表示が完了してるならクリック時に次の行を表示する
		if (gameLogicScript.gameStatus == (int)GameLogic.PLAY_STATUS.CONVERSATION
		    || gameLogicScript.gameStatus == (int)GameLogic.PLAY_STATUS.EVENT_CONVERSATION) {
			if (IsCompleteDisplayText) {
				if (currentLine < serifData.Count && Input.GetMouseButtonDown (0)) {
					if (gameLogicScript.gameStatus == (int)GameLogic.PLAY_STATUS.CONVERSATION) {
						if (isPriceConfigFlag == true) {
							gameLogicScript.gameStatus = (int)GameLogic.PLAY_STATUS.PRICE_CONFIG;
							if (currentLine == (int)CONVERSATION_CASE.PLEASE_PRODUCT) {
								currentLine = (int)CONVERSATION_CASE.PRICE;
							} else {
								currentLine = (int)CONVERSATION_CASE.REPRICE;
							}
							isPriceConfigFlag = false;
						}
					}
					SetNextLine ();

				}
			} else {
				// 完了してないなら文字をすべて表示する
				if (Input.GetMouseButtonDown (0)) {
					timeUntilDisplay = 0;
				}
			}
		}

		int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);

		//字数が増えたら
		if ( displayCharacterCount != lastUpdateCharacter ) {
			//currentTextに字が入っていたら
			if (currentText != string.Empty) {
				//文字をdisplayCharacterCountの分表示する
				uiText.text = currentText.Substring (0, displayCharacterCount);
			} else {
				//字が入っていなかったら(初期化された後だったら)空白にする
				uiText.text = string.Empty;
			}
			//毎フレーム処理をしないようにlastUpdateCharacterで字数が変わったか判定するための処理
			lastUpdateCharacter = displayCharacterCount;
		}
	}


	private void SetNextLine() {
		Debug.Log ("SetNextLine in");
		Debug.Log ("currentline " + currentLine);
		//客のクリック判定を無効にする
		customerClickScript.enabled = false;

		//デバッグ用　イベント会話スキップのボタンの表示処理
		if (gameLogicScript.gameStatus == (int)GameLogic.PLAY_STATUS.EVENT_CONVERSATION) {
			GameObject obj = GameObject.Find ("ConversationButton");
			obj.GetComponent<Button> ().enabled = true;
			obj.GetComponent<Image> ().enabled = true;
			obj.GetComponentInChildren<Text> ().enabled = true;
		}

		//表示するテキストを選出する
		if (gameLogicScript.gameStatus == (int)GameLogic.PLAY_STATUS.CONVERSATION) {
			Debug.Log ("normalconversation in");

			/*
			商品名 productName
			商品の種類 productStatus
			値段 price
			*/

			SERIF tempSerif = tempSerifData [currentLine];
			tempSerif.serif = tempSerif.serif.Replace ("productName", customerServiceScript.productName);
			tempSerif.serif = tempSerif.serif.Replace ("productStatus", productScript.PRODUCT_STATUS_TABLE[customerServiceScript.productStatus]);
			tempSerif.serif = tempSerif.serif.Replace ("price", customerServiceScript.price.ToString());
			tempSerifData [currentLine] = tempSerif;

			//if ( tempSerifData.Count > currentLine ) {
			currentText = tempSerifData [currentLine].serif;
			SetConversationImage (tempSerifData [currentLine].npcId, tempSerifData [currentLine].face);
			//} 

			//会話のCASEを設定
			switch(currentLine) {
			case (int)CONVERSATION_CASE.WELCOME:
				currentLine = (int)CONVERSATION_CASE.PLEASE_PRODUCT;
				break;

			case (int)CONVERSATION_CASE.PLEASE_PRODUCT:
				if (customerServiceScript.isPriceConfig == true) {
					currentLine = (int)CONVERSATION_CASE.PRICE;
				} else {
					if (customerServiceScript.isSoldOut == true) {
						currentLine = (int)CONVERSATION_CASE.SOLD_OUT;
					} else {
						isPriceConfigFlag = true;
					}
				}
				break;

			case (int)CONVERSATION_CASE.PRICE:
				if (customerServiceScript.isHighPrice == true) {
					currentLine = (int)CONVERSATION_CASE.HIGH_PRICE;
				} else {
					currentLine = (int)CONVERSATION_CASE.BUY;
				}
				break;

			case (int)CONVERSATION_CASE.HIGH_PRICE:
				if (customerServiceScript.isPriceConfig != false) {
					isPriceConfigFlag = true;
				} else {
					currentLine = (int)CONVERSATION_CASE.REPRICE;
				}
				break;

			case (int)CONVERSATION_CASE.BUY:
				currentLine = (int)CONVERSATION_CASE.SELL;
				break;

			case (int)CONVERSATION_CASE.REPRICE:
				if (customerServiceScript.isHighPrice == true) {
					currentLine = (int)CONVERSATION_CASE.GO_BACK;
				} else {
					currentLine = (int)CONVERSATION_CASE.BUY;
				}
				break;

			case (int)CONVERSATION_CASE.SELL:
			case (int)CONVERSATION_CASE.SOLD_OUT:
			case (int)CONVERSATION_CASE.GO_BACK:
				if (currentLine == (int)CONVERSATION_CASE.SELL) {
					customerServiceScript.isSell = true;
				}
				isConversationEnd = true;
				break;
			}


		} else if (gameLogicScript.gameStatus == (int)GameLogic.PLAY_STATUS.EVENT_CONVERSATION) {
			Debug.Log ("eventconversation in");
			//イベント会話のイベントセリフデータのcurrentLineを設定する
			if (isSetEventConversationCurrentLine != true) {
				for (int i = 0; i < eventSerifData.Count; i++) {
					//if (eventSerifData [i].stage == StageManager.stageLevel) {
					//we are 仕様　通常は上のを使う
					if (eventSerifData [i].stage == weAreStageLevel) {
						currentLine = i;
						Debug.Log ("event currentline Set " + currentLine);
						isSetEventConversationCurrentLine = true;

						//for文を抜ける
						i = eventSerifData.Count;
					}
				}
			}

			currentText = eventSerifData [currentLine].serif;
			SetEventConversationImage (eventSerifData [currentLine].playerFace, "N014", eventSerifData [currentLine].npcFace);
			if ( eventSerifData [currentLine].eventEnd == false ) {
				currentText = eventSerifData [currentLine].serif;
				SetEventConversationImage (eventSerifData [currentLine].playerFace, "N014", eventSerifData [currentLine].npcFace);
			} else {
				isConversationEnd = true;
			}
			currentLine ++;
		}


		timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
		timeElapsed = Time.time;

		lastUpdateCharacter = -1;

	}

	private void SetConversationImage( string npcId, string face ) {
		if (npcId == "N001") {
			playerImage.sprite = Resources.Load<Sprite> ("Image/2DCharacterImage/N001/N001" + face);
		} else {
			customerImage.sprite = Resources.Load<Sprite> ("Image/2DCharacterImage/" + npcId + "/" + npcId + face);
		}

	}

	private void SetEventConversationImage( string playerFace, string npcId, string npcFace ) {
		playerImage.sprite = Resources.Load<Sprite> ("Image/2DCharacterImage/N001/N001" + playerFace);
		customerImage.sprite = Resources.Load<Sprite> ("Image/2DCharacterImage/" + npcId + "/" + npcId + npcFace);
	}

	private void SetTutorialConversationImage(string playerFace, string npcId, string npcFace){
		
	}

	private void InitConversation( ) {
		Debug.Log ("InitConverstiaon in");
		currentText = string.Empty;
		timeUntilDisplay = 0;
		timeElapsed = 1;
		currentLine = 0;
		lastUpdateCharacter = -1;
		isConversationEnd = false;
		isSetEventConversationCurrentLine = false;
		playerImage.sprite = Resources.Load<Sprite> ("Image/2DCharacterImage/N001/N001A");
		customerImage.sprite = noneImage;

		customerServiceScript.InitCustomerService ();
		priceConfigScript.InitPriceConfig();
		SetNextLine();
	}

	private bool GetConversationActive() {
		return GetComponent<Canvas> ().enabled;
	}

	//デバッグ用　イベント会話を飛ばす
	public void EndEventConversation() {
		GameObject obj = GameObject.Find ("ConversationButton");
		obj.GetComponent<Button> ().enabled = false;
		obj.GetComponent<Image> ().enabled = false;
		obj.GetComponentInChildren<Text> ().enabled = false;

		gameLogicScript.gameStatus = (int)GameLogic.PLAY_STATUS.PURCHASE_SELECT;

		deskOnImageManagerScript.SetDeskImage ();
		GameObject.Find ("ConversationCanvas").GetComponent<ConversationManager> ().InitConversation();
		customerClickScript.enabled = true;
	}

}
