using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class DeskOnImageManager : MonoBehaviour {
	public Canvas canvas;					//productImageを取得するためにcanvasから探す

	public int deskNum = 4;					//机の数
	private Sprite noneSprite;				//机に置かれてない時の透明な画像

	public string searchStageMap = "StageMap";

	private Product productScript;
	public GameObject purchaseOKButton;

	void Awake() {
		noneSprite = Resources.Load<Sprite> ("Image/None");
		productScript = GameObject.Find ("StageManager").GetComponent<Product> ();
		canvas = GetComponent<Canvas> ();
	}

	// Use this for initialization
	void Start () {
		if (StageManager.stageLevel <= 3) {
			searchStageMap = "StageMap";
			deskNum = 4;
		} else {
			searchStageMap = "StageMap2";
			deskNum = 8;
		}
	}

	// Update is called once per frame
	void Update() {
		
	}

	void LateUpdate () {
		/*if (customerServiceScript.isSell == true) {
			Debug.Log ("deskScript if in");
			SetDeskImage ();
			customerServiceScript.isSell = false;
		} else */if (purchaseOKButton.GetComponent<PurchaseManager> ().isPurchaseOKButtonPush == true) {
			Debug.Log ("deskScript elseif in");
			SetDeskImage ();
			purchaseOKButton.GetComponent<PurchaseManager> ().isPurchaseOKButtonPush = false;

		}
	}

	//机の上に描画する画像の処理
	public void SetDeskImage( ) {
		Debug.Log ("SetDeskImage in");

		//for (int i = 0; i < productScript.buyProduct.Count; i++) {
		for (int i = 0; i < deskNum; i++) {
				//canvasから探す
			foreach (Transform child in canvas.transform) {
				//stageMapを見つけたらさらに探す
				//if (child.name == "StageMap") {
				if (child.name == searchStageMap) {
					Debug.Log ("foreach1 if in");
					foreach (Transform child2 in child.transform) {
						//Desk (i)を見つけたら
						if (child2.name == "Desk (" + i + ")") {
							Debug.Log ("foreach2 if in");
							//机の座標を取得する
							Vector3 deskPos = child2.transform.position;
							//その下のproductImageを見つける
							foreach (Transform child3 in child2.transform) {
								if (child3.name == "ProductImage") {
									Debug.Log ("foreach3 if in");
									child3.transform.position = deskPos;
									if (i < productScript.buyProduct.Count) {
										child3.gameObject.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Image/ProductImage/" + productScript.productData [productScript.buyProduct [i].productIdNum].id);
									} else {
										//画像を透明にして消す
										child3.gameObject.GetComponent<Image> ().sprite = noneSprite;

									}
								}
							}
						}
					}
				}
			}
		}
	}
}