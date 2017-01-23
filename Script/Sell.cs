using UnityEngine;
using System.Collections;

public class Sell : MonoBehaviour {

	public DeskOnImageManager deskOnImageManagerScript;
	public Canvas canvas;
	public GameObject desk;

	void Awake() {
		deskOnImageManagerScript = GameObject.Find ("Canvas").GetComponent<DeskOnImageManager>();
		canvas = GetComponent<Canvas> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BuyProduct( ) {
		Debug.Log ("BuyProduct Clicked.");
		/*
		Debug.Log ("in after " + this.name);
		foreach (Transform child in desk.transform) {
			Debug.Log ("foreach in this name " + this.name);
			if (child.name == this.name) {
				Debug.Log ("1desk in");
				Debug.Log("1 this " + this.name);
				foreach (Transform child2 in child.transform) {
					Debug.Log ("2 in");
				}
			}
			if (child.name == "ProductImage") {
				Debug.Log ("image in");
			}
		}
		*/

		/*
		foreach (Transform child in canvas.transform) {
			Debug.Log ("1 in");
			if (child.name == "StageMap") {

				foreach (Transform child2 in child.transform) {
					Debug.Log ("2 in");
					Debug.Log ("2 " + this.name);
					if (child2.name == "Desk (0)") {
						
						foreach (Transform child3 in child2.transform) {
							Debug.Log ("3 in");
							if (child3.name == "ProductImage") {
								
							}
						}
					}
				}
			}

		}
		*/
	}

}
