using UnityEngine;
using System.Collections;

public class GoBackCustomer : MonoBehaviour {

	public Transform _transform;
	public bool death;



	void Awake() {
		
	}

	// Use this for initialization
	void Start () {
		death = false;
		_transform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if ( transform.localPosition.y > 193f) {
			death = true;
		}

		if (death) {
			Destroy (gameObject);
			GameObject.Find ("CharacterManager").GetComponent<CustomerInStore> ().comeCharacterList.Remove (gameObject.name.Substring (0, 13));
		}
	}

	//客を全員消す
	public void AllDestroyCustomer() {
		//このスクリプトが付いているオブジェクトの子オブジェクトを全てDestroyする
		foreach (Transform clone in gameObject.transform) {
			Debug.Log ("foreach in");
			GameObject.Destroy (clone.gameObject);
		}

		//入店してる客リストを空にする
		this.GetComponent<CustomerInStore> ().comeCharacterList.Clear ();
	}

	public void DestoryCustomer(string npcId) {
		//客を消す
		GameObject.Find ("Character" + npcId + "(Clone)").GetComponent<GoBackCustomer>().death = true;;
	}
}
