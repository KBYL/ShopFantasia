using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PriceConfig : MonoBehaviour {
	[SerializeField]
	public GameObject one;			//値段設定の1桁目
	[SerializeField]
	public GameObject ten;			//値段設定の2桁目
	[SerializeField]
	public GameObject hundred;		//値段設定の3桁目
	[SerializeField]
	public GameObject thousant;		//値段設定の4桁目

	//スクロールの数字のオブジェクトの入ったオブジェクト群　4桁分
	[SerializeField]
	public GameObject scrollOneObj;

	[SerializeField]
	public GameObject scrollTenObj;

	[SerializeField]
	public GameObject scrollHundredObj;

	[SerializeField]
	public GameObject scrollThousantObj;

	[SerializeField]
	public Image productImage;	//値段を決めている商品の画像
	[SerializeField]
	public Text productPriceText;	//値段を決めている商品の原価（仕入商人から買った値段）テキスト
	[SerializeField]
	public GameObject informationObj;	//値上がり、値下がり率を表示してるオブジェクト

	private Product productScript;
	private CustomerService customerServiceScript;

	void Awake() {
		productScript = GameObject.Find ("StageManager").GetComponent<Product> ();
		customerServiceScript = GameObject.Find ("CharacterManager").GetComponent<CustomerService> ();
	}
	
	// Update is called once per frame
	void Update () {
		SetConfigScreenImage ();
	}

	//設定した値段を4桁の数字にして返す
	//「売る」ボタンを押したときの動作
	public void GetConfigPrice( ) {

		//各桁の数値を計算して４桁の数値にする
		GameObject.Find("CharacterManager").GetComponent<CustomerService>().price = 
			(one.GetComponent<PriceConfigGetNumber> ().priceNum) +
			(ten.GetComponent<PriceConfigGetNumber> ().priceNum * 10) +
			(hundred.GetComponent<PriceConfigGetNumber> ().priceNum * 100) +
			(thousant.GetComponent<PriceConfigGetNumber> ().priceNum * 1000);

		//ステータスを変える
		//gameStatus
		GameObject.Find ("Main Camera").GetComponent<GameLogic> ().gameStatus = (int)GameLogic.PLAY_STATUS.CONVERSATION;

		//customerServiceのisPriceConfig
		GameObject.Find ("CharacterManager").GetComponent<CustomerService> ().isPriceConfig = true;

		//SetHighPriceを実行しisHighPriceの設定
		GameObject.Find ("CharacterManager").GetComponent<CustomerService> ().SetIsHighPrice ();

	}

	//値段設定画面の初期化
	public void InitPriceConfig( ) {
		//値段の各位の数字を0の位置にする
		scrollOneObj.transform.localPosition = new Vector3 (0, 0, 0);
		scrollTenObj.transform.localPosition = new Vector3 (0, 0, 0);
		scrollHundredObj.transform.localPosition = new Vector3 (0, 0, 0);
		scrollThousantObj.transform.localPosition = new Vector3 (0, 0, 0);

		//非表示にしておく
		informationObj.SetActive (false);
	}

	public void SetConfigScreenImage() {
		productImage.sprite = Resources.Load<Sprite>("Image/ProductImage/" + productScript.productData.Find(x => x.name == customerServiceScript.productName).id);
		productPriceText.text = "基準価格$" + customerServiceScript.standardPrice.ToString();
		if (customerServiceScript.bonusPercent != 0) {
			//テキストの設定
			informationObj.GetComponentInChildren<Text> ().text = 
				customerServiceScript.bonusPercent > 0 
				? (customerServiceScript.bonusPercent * 100).ToString () + "%UP!" 
				: (customerServiceScript.bonusPercent * 100).ToString () + "%DOWN...";
			informationObj.SetActive (true);
		} else {
			informationObj.SetActive (false);
		}
	}
}
