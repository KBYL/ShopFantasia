using UnityEngine;
using System.Collections;

public class CustomerClick : MonoBehaviour {

	public string npcId;		//クリックした客のID
	public GameObject canvas;	//conversationCanvasを取得するためのオブジェクト

	private CustomerInStore customerInStoreScript;
	private GameLogic gameLogicScript;
	private ConversationManager conversationManagerScript;
	private CustomerService customerServiceScript;

	void Awake() {
		customerInStoreScript = GameObject.Find ("CharacterManager").GetComponent<CustomerInStore>();
		gameLogicScript = GameObject.Find ("Main Camera").GetComponent<GameLogic> ();
		conversationManagerScript = canvas.transform.Find("ConversationCanvas").gameObject.GetComponent<ConversationManager>();
		customerServiceScript = GameObject.Find ("CharacterManager").GetComponent<CustomerService> ();
    }

	void Start() {
		
	}


	// Update is called once per frame
	void Update () {
        //タップしたら
		if (Input.GetMouseButtonDown (0)) {
			//タップ音を鳴らす
			SEManager.isSeSound[(int)SEManager.SE_LIST.TAP] = true;

			Vector3 aTapPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Collider2D aCollider2d = Physics2D.OverlapPoint (aTapPoint);

			//キャラがタップされたかどうか
			if (aCollider2d) {
				//npcIdを取得する
				GameObject obj = aCollider2d.transform.gameObject;
				npcId = obj.name.Substring (9, 4);		//数字の部分だけ抽出する

				//一致するキャラクターを探す
				for ( int i = 0; i < customerInStoreScript.comeCharacterList.Count; i++ ) {
					if ( obj.name == customerInStoreScript.comeCharacterList[i] + "(Clone)") {
						//アクション
						Debug.Log ("character ifin");

						//タップしたキャラクターのセリフデータを入れる
						conversationManagerScript.tempSerifData = conversationManagerScript.serifData.FindAll (x => x.npcId == npcId);
						conversationManagerScript.tempSerifData.Insert (0, conversationManagerScript.serifData.Find (x => x.serifId == "T001"));
						conversationManagerScript.tempSerifData.Add (conversationManagerScript.serifData.Find (x => x.serifId == "T002"));
						conversationManagerScript.tempSerifData.Add (conversationManagerScript.serifData.Find (x => x.serifId == "T003"));
						conversationManagerScript.tempSerifData.Add (conversationManagerScript.serifData.Find (x => x.serifId == "T004"));
						conversationManagerScript.tempSerifData.Add (conversationManagerScript.serifData.Find (x => x.serifId == "T005"));

						//RandSelectProductを呼ぶ
						customerServiceScript.RandSelectProduct (npcId);

						//isSoldOutを設定する
						customerServiceScript.SetIsSoldOut ();
                           
						//ステータスを変える
						gameLogicScript.gameStatus = (int)GameLogic.PLAY_STATUS.CONVERSATION;
					}
				}
			}
		}
	}
}