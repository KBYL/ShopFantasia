using UnityEngine;
using System.Collections;

public class DebugStageClear : MonoBehaviour {
	public GameObject resultCanvas;
	public GameObject purchaseManagerObj;

	private GameLogic gameLogicScript;
	//private Result resultScript;
	private UIManager uiManagerScript;
	private PurchaseManager purchaseManagerScript;
	private GoBackCustomer goBackCustomerScript;

	void Awake() {
		gameLogicScript = GameObject.Find ("Main Camera").GetComponent<GameLogic> ();
		//resultScript = resultCanvas.GetComponent<Result> ();
		uiManagerScript = GameObject.Find ("UIManager").GetComponent<UIManager> ();
		purchaseManagerScript = purchaseManagerObj.GetComponent<PurchaseManager> ();
		goBackCustomerScript = GameObject.Find ("CharacterManager").GetComponent<GoBackCustomer> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void DebugStageClearButtonClick() {
		goBackCustomerScript.AllDestroyCustomer ();
		uiManagerScript.money = uiManagerScript.target;
		purchaseManagerScript.ResetPurchase ();
		gameLogicScript.gameStatus = (int)GameLogic.PLAY_STATUS.RESULT;

	}
}
