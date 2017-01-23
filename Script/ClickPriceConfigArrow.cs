using UnityEngine;
using System.Collections;

public class ClickPriceConfigArrow : MonoBehaviour {
	[SerializeField]
	public GameObject baseObj;

	public GameObject scrollNumberObj;

	public float baseHeight;

	void Start() {
		baseHeight = baseObj.gameObject.GetComponent<RectTransform> ().sizeDelta.y;
	}

	//値段設定の上矢印を押した時
	public void NumUpPush( ) {
		scrollNumberObj.gameObject.transform.localPosition += new Vector3(0, baseHeight, 0);
	}

	//値段設定の下矢印を押した時
	public void NumDownPush( ) {
		scrollNumberObj.gameObject.transform.localPosition -= new Vector3(0, baseHeight, 0);
	}
}
