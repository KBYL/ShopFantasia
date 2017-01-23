using UnityEngine;
using System.Collections;

public class PriceConfigGetNumber : MonoBehaviour {
	public int priceNum;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D coll) {
		priceNum = int.Parse (coll.gameObject.name);
	}
}
