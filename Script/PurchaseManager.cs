using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PurchaseManager : MonoBehaviour {
	public GameObject productPrefab;	//生成する商品プレハブ

	[SerializeField]
	public Text purchaseMoneyText;		//仕入れ画面上部のお金テキストオブジェクト
	[SerializeField]
	public GameObject buyCountInformationObj;	//個数に関する忠告をするオブジェクト
    [SerializeField]
    public Image upArrowObj;
    [SerializeField]
    public Image downArrowObj;

	[SerializeField]
	public GameObject characterManagerObj;

	public float productPrefabHeight;
	public float defaultPurchaseScrollPosY;

	public AudioSource audioSource;
	public AudioClip moneySE;

    private StageManager stageManagerScript;
	private Product productScript;
	private UIManager uiManagerScript;

	[SerializeField]
	public bool isPurchaseOKButtonPush = false;

	[SerializeField]
	public int purchaseBuyCount = 0;	//仕入れ画面で商品を買おうとしてる個数
	public bool isSetPurchase = true;	//trueでSetPurchaseを呼ぶ

	void Awake() {
		stageManagerScript = GameObject.Find("StageManager").GetComponent<StageManager> ();
		productScript = GameObject.Find("StageManager").GetComponent<Product> ();
		uiManagerScript = GameObject.Find("UIManager").GetComponent<UIManager> ();
	}

	void Start() {
		productPrefabHeight = productPrefab.GetComponent<RectTransform> ().sizeDelta.y;
		defaultPurchaseScrollPosY = this.gameObject.transform.localPosition.y;
		audioSource = gameObject.AddComponent<AudioSource>();
		moneySE = Resources.Load<AudioClip> ("Sound/S0102");
	}

	void Update () {
		//SetPurchaseを呼ぶかの処理
		if (isSetPurchase == true) {
			SetPurchase ();
		}

		//仕入れ画面の矢印の表記処理
		if (this.gameObject.transform.localPosition.y > defaultPurchaseScrollPosY + 1) {
			upArrowObj.enabled = true;
		} else {
			upArrowObj.enabled = false;
		}

		if (this.gameObject.transform.localPosition.y 
			< defaultPurchaseScrollPosY + productPrefabHeight
			* (stageManagerScript.stageData [StageManager.stageLevel].purchaseNum - 5) - 1) {
			downArrowObj.enabled = true;
		} else {
			downArrowObj.enabled = false;
		}

	}

	//仕入れ画面の商品を配置する
	public void SetPurchase( ) {
		for (int i = 0; i < stageManagerScript.stageData[ StageManager.stageLevel ].purchaseNum; i++) {
			int purchaseRand = i;	//商品の番号
			int productPrice;		//商品の値段

			//プレハブを配置する
			GameObject purchase = (GameObject)Instantiate (productPrefab);
            purchase.transform.parent = transform;
            //purchase.transform.SetParent(transform.parent);
			purchase.transform.localPosition = new Vector3 (0, i * -2.5f, 0);

			//ランダムで仕入れの品物を選ぶ
			int rankRand = Random.Range (1, 100);
			if (rankRand <= stageManagerScript.stageData [StageManager.stageLevel].rankS * 100) {
				//rankSだった場合
				purchaseRand = Random.Range(0, productScript.rankSProduct.Count - 1 )
					+ productScript.rankCProduct.Count + productScript.rankBProduct.Count + productScript.rankAProduct.Count;
			} else if ( rankRand <= stageManagerScript.stageData [StageManager.stageLevel].rankA * 100) {
				//rankAだった場合
				purchaseRand = Random.Range(0, productScript.rankAProduct.Count - 1 )
					+ productScript.rankCProduct.Count + productScript.rankBProduct.Count;
			} else if ( rankRand <= stageManagerScript.stageData [StageManager.stageLevel].rankB * 100) {
				//rankBだった場合
				purchaseRand = Random.Range(0, productScript.rankBProduct.Count - 1 )
					+ productScript.rankCProduct.Count;
			} else if ( rankRand <= stageManagerScript.stageData [StageManager.stageLevel].rankC * 100) {
				//rankCだった場合
				purchaseRand = Random.Range(0, productScript.rankCProduct.Count - 1 );
			}

			//ステータスを入れる
			purchase.GetComponent<Purchase>().productImage.sprite = Resources.Load<Sprite>("Image/ProductImage/" + productScript.productData[ purchaseRand ].id);	//画像
			purchase.GetComponent<Purchase> ().productNameText.text = productScript.productData [purchaseRand].name;					//名前
			purchase.GetComponent<Purchase> ().productRankText.text = productScript.RANK_TABLE [productScript.productData [purchaseRand].rank];	//ランクの文字
			productPrice = Random.Range (productScript.productData [purchaseRand].min, productScript.productData [purchaseRand].max);	//値段を決める
			purchase.GetComponent<Purchase> ().productPriceText.text = productPrice.ToString();		//値段の文字

			purchase.GetComponent<Purchase> ().productPrice = productPrice;					//値段保存
			purchase.GetComponent<Purchase> ().productRand = purchaseRand;					//番号保存
		}
		isSetPurchase = false;
		purchaseMoneyText.text = uiManagerScript.money >= 0 ? uiManagerScript.money.ToString () : "お金が足りないよ";

	}

	//仕入れ商品の再セット
	public void ResetPurchase() {
		Debug.Log ("ResetPurchase in");

		purchaseBuyCount = 0;		//買った商品の個数の初期化

		//前に生成した仕入れ画面の商品プレハブを全部消す
		foreach (Transform clone in gameObject.transform) {
			GameObject.Destroy (clone.gameObject);
		}

		//スクロールを初期値に戻す
		this.gameObject.transform.localPosition = new Vector3(0, defaultPurchaseScrollPosY, 0);

		//商品プレハブを設置するスイッチをオンにする
		isSetPurchase = true;
	}

	//OKボタンを押した時の処理
	public void PurchaseOKButtonPush () {
		PurchaseManager purchaseManagerScript = GameObject.Find ("PurchaseManager").GetComponent<PurchaseManager> ();
		Product productScript = GameObject.Find ("StageManager").GetComponent<Product> ();
		//OKを押した時に在庫が0ではなく、所持金がマイナスでなければ
		if ((purchaseManagerScript.purchaseBuyCount != 0 
			|| productScript.buyProduct.Count != 0)
			&& GameObject.Find("UIManager").GetComponent<UIManager>().money >= 0) {

			//テキストを非表示にする
			purchaseManagerScript.buyCountInformationObj.SetActive (false);

			//ボタンを押したフラグを立てる
			isPurchaseOKButtonPush = true;

			//不具合で客が出ていたときの対処として客を消す
			characterManagerObj.GetComponent<GoBackCustomer> ().AllDestroyCustomer ();
		
			//CustomerInStoreのisFristCustomerを初期化する
			characterManagerObj.GetComponent<CustomerInStore> ().isFirstCustomer = false;

			//何か買っていたら
			if (purchaseManagerScript.purchaseBuyCount != 0) {
				SEManager.isSeSound [(int)SEManager.SE_LIST.MONEY] = true;
			}

		//} else if (purchaseManagerScript.purchaseBuyCount == 0 ) {
		} else if (productScript.buyProduct.Count == 0 ) {
			//何も買ってなかったらテキストを出す
			purchaseManagerScript.buyCountInformationObj.gameObject.GetComponentInChildren<Text> ().text = "何か買ってね";
			purchaseManagerScript.buyCountInformationObj.SetActive (true);
		}
	}

}
