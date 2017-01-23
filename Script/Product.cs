using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Product : MonoBehaviour {

	public enum PRODUCT_TYPE {
		NONE,
		WEAPON,
		PROTECTOR,
		CLOTHES,
		FOOD,
		ACSSEESORIE,
		SWORD,
		WAND,
		BOW,
		SHIELD,
		ARMOR,
		CAPE,
		SHOES,
	};

	public enum RANK {
		C,
		B,
		A,
		S,
	};

	public class PRODUCT {
		public string id;
		public string name;
		public int purchaseStatus;
		public int productStatus;
		public int rank;
		public int min;
		public int max;
		public PRODUCT(string Id, string Name, int PurchaseStatus, int ProductStatus, int Rank, int Min, int Max ) {
			id = Id;
			name = Name;
			purchaseStatus = PurchaseStatus;
			productStatus = ProductStatus;
			rank = Rank;
			min = Min;
			max = Max;
		}
	}

	public struct BUY_PRODUCT {
		public int productIdNum;
		public int productPrice;
		public string productName;
		public BUY_PRODUCT(int ProductIdNum, int ProductPrice, string ProductName ) {
			productIdNum = ProductIdNum;
			productPrice = ProductPrice;
			productName = ProductName;
		}
	
	}

	public string [] PRODUCT_STATUS_TABLE = new string[] {
		"NONE",	"武器", "防具", "服", "食べ物", "アクセサリー", "剣", "杖", "弓", "盾", "鎧", "ケープ", "靴"
	};

	public string [] RANK_TABLE = new string [ 4 ] {
		"C", "B", "A", "S"
	};

	//各ランクのIDを格納するリスト
	public List<PRODUCT>rankCProduct = new List<PRODUCT>();
	public List<PRODUCT>rankBProduct = new List<PRODUCT>();
	public List<PRODUCT>rankAProduct = new List<PRODUCT>();
	public List<PRODUCT>rankSProduct = new List<PRODUCT>();

	//仕入れ時に買った商品の情報
	[SerializeField]
	public List<BUY_PRODUCT> buyProduct = new List<BUY_PRODUCT>();

	//全商品データを入れるリスト
	[SerializeField]
	public List<PRODUCT> productData = new List<PRODUCT>();

	//private CustomerService customerServiceScript;

	void Awake() {
		//customerServiceScript = GameObject.Find ("CharacterManager").GetComponent<CustomerService> ();
	}

	void Start () {
		
		Entity_ProductData entityProductData = Resources.Load ("Data/ProductData") as Entity_ProductData;

		//productDataにデータを入れる
		for (int i = 0; i < entityProductData.param.Count; i++) {
			productData.Add (new PRODUCT (
				entityProductData.param [i].id,
				entityProductData.param [i].name,
				entityProductData.param [i].purchaseStatus,
				entityProductData.param [i].productStatus,
				entityProductData.param [i].rank,
				entityProductData.param [i].min,
				entityProductData.param [i].max)
			);

			switch (entityProductData.param [i].rank) {
			case (int)RANK.C:
				rankCProduct.Add(new PRODUCT (
					entityProductData.param [i].id,
					entityProductData.param [i].name,
					entityProductData.param [i].purchaseStatus,
					entityProductData.param [i].productStatus,
					entityProductData.param [i].rank,
					entityProductData.param [i].min,
					entityProductData.param [i].max)
				);
				break;

			case (int)RANK.B:
				rankBProduct.Add(new PRODUCT (
					entityProductData.param [i].id,
					entityProductData.param [i].name,
					entityProductData.param [i].purchaseStatus,
					entityProductData.param [i].productStatus,
					entityProductData.param [i].rank,
					entityProductData.param [i].min,
					entityProductData.param [i].max)
				);
				break;

			case (int)RANK.A:
				rankAProduct.Add(new PRODUCT (
					entityProductData.param [i].id,
					entityProductData.param [i].name,
					entityProductData.param [i].purchaseStatus,
					entityProductData.param [i].productStatus,
					entityProductData.param [i].rank,
					entityProductData.param [i].min,
					entityProductData.param [i].max)
				);
				break;

			case (int)RANK.S:
				rankSProduct.Add(new PRODUCT (
					entityProductData.param [i].id,
					entityProductData.param [i].name,
					entityProductData.param [i].purchaseStatus,
					entityProductData.param [i].productStatus,
					entityProductData.param [i].rank,
					entityProductData.param [i].min,
					entityProductData.param [i].max)
				);
				break;
			}


		}
		productData.Sort ((a, b) => a.rank - b.rank);
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	/*
	//客が買いたい商品の情報をPRODUCT型で返す
	public PRODUCT GetCustomerSelectProduct() {
		List <PRODUCT> tempProduct = new List<PRODUCT>();
		tempProduct.Add(productData.Find(x => x.name == customerServiceScript.productName));
		//return tempProduct[0].name;
		return tempProduct;
	}

	public void GetProductStatus(string name) {
		PRODUCT tempProductData = productData.Find (x => x.name == name);
	}
	*/
}
