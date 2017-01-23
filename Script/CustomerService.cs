using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomerService : MonoBehaviour {

	//NPCのデータの型
    public struct NPC {
        public string id;
        public int buyProduct1;
        public int buyProduct2;
        public int buyProduct3;
        public int buyProduct4;
        public int buyProduct5;
        public int buyProduct6;
        public int buyProductCount;
        public NPC(string Id, int BuyProduct1, int BuyProduct2, int BuyProduct3, int BuyProduct4, int BuyProduct5, int BuyProduct6, int BuyProductCount) {
            id = Id;
            buyProduct1 = BuyProduct1;
            buyProduct2 = BuyProduct2;
            buyProduct3 = BuyProduct3;
            buyProduct4 = BuyProduct4;
            buyProduct5 = BuyProduct5;
            buyProduct6 = BuyProduct6;
            buyProductCount = BuyProductCount;
        }
    }

	//客が値段を高いと思うかの確率
	public struct BUY {
		public float low;
		public float high;
		public float rand;
		public BUY(float Low, float High, float Rand) {
			low = Low;
			high = High;
			rand = Rand;
		}
	}

	//NPCのデータのリスト
    public List<NPC> npcData = new List<NPC>();
	public List<BUY> buyPercentageData = new List<BUY>();

	[SerializeField]
	public const float FAVORITE_BONUS_PERCENT = 0.15f;	//好みの商品の種類だった時の客が買う率を上げる率

    public string productName = "no name";	//商品の名前
    public int productStatus = 0;       	//商品のステータス
    public int price = 0;           	 	//提示する商品の値段
	[SerializeField]
    public int standardPrice;       	    //商品の基準値段
	public double bonusPercent;				//値上がり値下がりの率
	float favoriteCustomerBonusPercent = 0.0f;	//商品が客の好きなものだった時のボーナスパーセント
    public bool isHighPrice = false;    	//客が高いと思うか思わないか
    public bool isSoldOut = false;      	//品切れかそうでないか
    public bool isPriceConfig = false;  	//値段設定が済んだかそうでないか
	public bool isSell = false;				//商品が売れたら


	private Product productScript;
	private StageManager stageManagerScript;
	public GameObject priceConfigObj;
	//private PriceConfig priceConfigScript;

	public Toggle debugToggle;

	void Awake() {
		productScript = GameObject.Find("StageManager").GetComponent<Product>();
		stageManagerScript = GameObject.Find ("StageManager").GetComponent<StageManager> ();
		//priceConfigScript = priceConfigObj.GetComponent<PriceConfig> ();
	}

    // Use this for initialization
	void Start() {
		Entity_NpcData entityNpcData = Resources.Load("Data/NpcData") as Entity_NpcData;
		Entity_BuyData entityBuyData = Resources.Load ("Data/BuyData") as Entity_BuyData;

		//npcDataにデータを入れる
        for (int i = 0; i < entityNpcData.param.Count; i++) {
            npcData.Add(new NPC(
                entityNpcData.param[i].id,
                entityNpcData.param[i].buyProduct1,
                entityNpcData.param[i].buyProduct2,
                entityNpcData.param[i].buyProduct3,
                entityNpcData.param[i].buyProduct4,
                entityNpcData.param[i].buyProduct5,
                entityNpcData.param[i].buyProduct6,
                entityNpcData.param[i].buyProductCount)
            );
        }

		//buyPercentageDataにデータを入れる
		for ( int i = 0; i < entityBuyData.param.Count; i++ ) {
			buyPercentageData.Add (new BUY (
				entityBuyData.param [i].lowPercent,
				entityBuyData.param [i].highPercent,
				entityBuyData.param [i].buyPercent)
			);
		}
    }

    //客が買いたい商品の選出
    public void RandSelectProduct(string npcId) {
       //IDの下2桁を取得する
        int npcIdNum = int.Parse(npcId.Substring(npcId.Length - 2, 2));

        //キャラごとに設定されてる買いたいstatusの乱数を生成して取得する
        int randProductStatus = Random.Range(0, npcData[npcIdNum - 1].buyProductCount);

		//npcData[npcIdNum - 1].buyProductCount はnpcのbuyProductが入力されてる数
        switch (Random.Range(0, npcData[npcIdNum - 1].buyProductCount)) {
            case 0:
                randProductStatus = npcData[npcIdNum - 1].buyProduct1;
                break;

            case 1:
                randProductStatus = npcData[npcIdNum - 1].buyProduct2;
                break;

            case 2:
                randProductStatus = npcData[npcIdNum - 1].buyProduct3;
                break;

            case 3:
                randProductStatus = npcData[npcIdNum - 1].buyProduct4;
                break;

            case 4:
                randProductStatus = npcData[npcIdNum - 1].buyProduct5;
                break;

            case 5:
                randProductStatus = npcData[npcIdNum - 1].buyProduct6;
                break;

            default:
                break;
        }

		//抽出した商品を格納するリスト
		List<Product.PRODUCT> tempSelectProduct = new List<Product.PRODUCT> ();

		//商品をランクとrandProductStatusによりtempSelectProdcutに入れる
		int rankRand = Random.Range (1, 100);

		if (rankRand <= stageManagerScript.stageData [StageManager.stageLevel].rankS * 100) {
			//rankSだった場合
			tempSelectProduct = productScript.rankSProduct.FindAll (x => x.productStatus == randProductStatus);
		} else if (rankRand <= stageManagerScript.stageData [StageManager.stageLevel].rankA * 100) {
			//rankAだった場合
			tempSelectProduct = productScript.rankAProduct.FindAll (x => x.productStatus == randProductStatus);
		} else if (rankRand <= stageManagerScript.stageData [StageManager.stageLevel].rankB * 100) {
			//rankBだった場合
			tempSelectProduct = productScript.rankBProduct.FindAll (x => x.productStatus == randProductStatus);
		} else if (rankRand <= stageManagerScript.stageData [StageManager.stageLevel].rankC * 100) {
			//rankCだった場合
			tempSelectProduct = productScript.rankCProduct.FindAll (x => x.productStatus == randProductStatus);
		}

		//商品のリストからランダムで選ぶ
		if (tempSelectProduct.Count > 0) {
			productName = tempSelectProduct [Random.Range (0, tempSelectProduct.Count - 1)].name;
		}

        //デバッグ用	持ってる商品を客が頼んでくる
		if (debugToggle.isOn == true) {
			if (productScript.buyProduct.Count > 0) {
				productName = productScript.buyProduct [0].productName;
			}
		}

        //原価を取得
        standardPrice = productScript.buyProduct.Find (x => x.productName == productName).productPrice;
        
        //bonusPercentを含めたstandardPriceの設定
        SetBonusStandardPrice(productScript.productData.Find(x => x.name == productName).id);

        //tempProductName.Clear();
		tempSelectProduct.Clear ();
    }
    
   //isSoldOutの設定処理
	public void SetIsSoldOut() {
		//初期値
		isSoldOut = true;

		//判定
		for ( int i = 0; i < productScript.buyProduct.Count; i++ ) {
			if (productName == productScript.buyProduct[i].productName){
				isSoldOut = false;
			}
		}
	}


	//isHighPriceの設定処理
	public void SetIsHighPrice() {
		Debug.Log ("SetIsHighPrice in");
		//初期値
		isHighPrice = false;

		//高いか決める乱数
		float highPriceRand = Random.value;

		//原価と売値の割合
		float percentage = (float)price / (float)standardPrice;

		if ( productStatus == stageManagerScript.stageData.Find(x => x.stageName == productName).favoriteProduct1 
			|| productStatus == stageManagerScript.stageData.Find(x => x.stageName == productName).favoriteProduct2) {
			favoriteCustomerBonusPercent = FAVORITE_BONUS_PERCENT;
		}

        //判定
        for (int i = 0; i < buyPercentageData.Count; i++) {
			if (buyPercentageData[i].low <= percentage && percentage <= buyPercentageData[i].high) {
				if (highPriceRand >= buyPercentageData [i].rand + favoriteCustomerBonusPercent) {
					isHighPrice = true;
				} 
			}
		}
	}

    public void SetBonusStandardPrice(string productId) {
        //商品のstatusを取得
        int productStatus = productScript.productData.Find(x => x.id == productId).purchaseStatus;

        //bonusPercentの設定　switch文でやりたいけどエラー吐くからとりあえずこれで
        if (productStatus == stageManagerScript.stageData[StageManager.stageLevel].priceUpProduct1) {
            bonusPercent = stageManagerScript.stageData[StageManager.stageLevel].priceUpPercent1;
        } else if (productStatus == stageManagerScript.stageData[StageManager.stageLevel].priceUpProduct2) {
            bonusPercent = stageManagerScript.stageData[StageManager.stageLevel].priceUpPercent2;
        } else if (productStatus == stageManagerScript.stageData[StageManager.stageLevel].priceUpProduct3) {
            bonusPercent = stageManagerScript.stageData[StageManager.stageLevel].priceUpPercent3;
        } else if (productStatus == stageManagerScript.stageData[StageManager.stageLevel].priceDownProduct1) {
            bonusPercent = stageManagerScript.stageData[StageManager.stageLevel].priceDownPercent1;
        } else if (productStatus == stageManagerScript.stageData[StageManager.stageLevel].priceDownProduct2) {
            bonusPercent = stageManagerScript.stageData[StageManager.stageLevel].priceDownPercent2;
        } else if (productStatus == stageManagerScript.stageData[StageManager.stageLevel].priceDownProduct3) {
            bonusPercent = stageManagerScript.stageData[StageManager.stageLevel].priceDownPercent3;
        } else if (productStatus == stageManagerScript.stageData[StageManager.stageLevel].priceDownProduct4) {
            bonusPercent = stageManagerScript.stageData[StageManager.stageLevel].priceDownPercent4;
        }
        Debug.Log("bonus " + bonusPercent);

        standardPrice = (int)Mathf.Round((float)standardPrice * (float)(1 + bonusPercent));
		//priceConfigScript.SetConfigScreenImage ();
        /*
		switch(productStatus) {
		case stageManagerScript.stageData [StageManager.stageLevel].priceUpProduct1:
			
			break;

		case stageManagerScript.stageData [StageManager.stageLevel].priceDownProduct1:
		bonusPercent = (double)stageManagerScript.stageData [StageManager.stageLevel].priceDownPercent1;
			break;
		}*/
    }

	public void InitCustomerService() {
		isPriceConfig = false;
		isHighPrice = false;
		isSoldOut = false;
		isSell = false;
		bonusPercent = 0.0;
		favoriteCustomerBonusPercent = 0.0f;
	}
}
 





