using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Purchase : MonoBehaviour {
	
	public GameObject backGround;		//クリックしたときの背景の色のオブジェクト

	public Image productImage;			//商品の画像
	public Text productNameText;		//商品の名前テキスト
	public Text productRankText;		//商品のランクテキスト
	public Text productPriceText;		//商品の値段テキスト
	[SerializeField]
	public int productPrice;			//保存した商品の値段
	[SerializeField]
	public int productRand;				//保存した商品の番号
	[SerializeField]
	private GameObject productObj;		//ClickProduct内で残金表示するのに使う

	public PurchaseManager purchaseScriptInOKButton;	//OKボタンに入ってるpurchaseManagerScript
	private GameObject backGroundButton;	//背景の色を変えるためのボタン

	public bool isBackGroundActive = false;

	[SerializeField]
	public bool isSelectProduct = false;
	
	private GameLogic gameLogicScript;
	private Product productScript;

	void Awake() {
		gameLogicScript = GameObject.Find ("Main Camera").GetComponent<GameLogic> ();
		productScript = GameObject.Find("StageManager").GetComponent<Product>();
		purchaseScriptInOKButton = GameObject.Find ("OKButton").GetComponent<PurchaseManager> ();
	}

	void Start () {
		backGroundButton = gameObject.transform.FindChild ("BackGroundButton").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		//OKボタンを押された時の処理
		if (purchaseScriptInOKButton.isPurchaseOKButtonPush == true ) {
			Debug.Log ("push in");

			//選んだ商品を買う
			if (backGroundButton.GetComponent<Purchase>().isSelectProduct == true) {
				SelectPurchase ();
			}

			gameLogicScript.gameStatus = (int)GameLogic.PLAY_STATUS.NORMAL;
		}
	}

	//仕入れ画面の商品がクリックされた時のもの
	public void ClickProduct( ) {
		PurchaseManager purchaseManagerScript = GameObject.Find ("PurchaseManager").GetComponent<PurchaseManager> ();
		UIManager uiManagerScript = GameObject.Find ("UIManager").GetComponent<UIManager> ();
		bool isBuyCountInformationObjActive = false;	//buyCountInformationObjのアクティブの変数
		if (isBackGroundActive == true) {	//既に選ばれていたら(バックグラウンドが付いてたら)
			isBackGroundActive = false;
			isSelectProduct = false;
			purchaseManagerScript.purchaseBuyCount--;
			uiManagerScript.money += productObj.GetComponent<Purchase> ().productPrice;
			isBuyCountInformationObjActive = false;
		} else {							//選ばれていないなら
			//選択個数が上限に達していなければ
			if (purchaseManagerScript.purchaseBuyCount < GameObject.Find ("StageManager").GetComponent<StageManager> ().stageData [StageManager.stageLevel].purchaseBuyMax) {
				isBackGroundActive = true;
				isSelectProduct = true;
				purchaseManagerScript.purchaseBuyCount++;
				uiManagerScript.money -= productObj.GetComponent<Purchase> ().productPrice;
				isBuyCountInformationObjActive = false;
			} else {
				isBuyCountInformationObjActive = true;
			}
		}

		//仕入れ画面の残金表示処理
		purchaseManagerScript.purchaseMoneyText.text = uiManagerScript.money >= 0 ? uiManagerScript.money.ToString () : "お金が足りないよ";

		purchaseManagerScript.buyCountInformationObj.SetActive (isBuyCountInformationObjActive);
		purchaseManagerScript.buyCountInformationObj.gameObject.GetComponentInChildren<Text> ().text = "これ以上は買えないよ";
		backGround.SetActive (isBackGroundActive);
	}

	//仕入れ画面から商品の購入処理
	public void SelectPurchase( ) {
		//仕入れた商品の情報を保存する
		productScript.buyProduct.Add ( new Product.BUY_PRODUCT(productRand, productPrice, productScript.productData[productRand].name));
	}
}