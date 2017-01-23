using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerInStore : MonoBehaviour {
	
	public List<GameObject> characters = new List<GameObject>();	//このステージで入店する客のリスト

	float nextSpawnTime = 0;				//客が入ってくる間隔の処理

	[SerializeField]
	float interval = 4.0f;					//客が入ってくる間隔

	[SerializeField]
	public bool isStopPlayer = false;	//キャラを止めるか止めないか

	public bool isAddCharacter = false;	//キャラが増える面で生成するキャラの種類を増やしたか

	public bool isFirstCustomer = false;

	public List<string> comeCharacterList = new List<string> ();	//入店してる客の名前リスト

	private StageManager stageManagerScript;

	void Awake() {
		stageManagerScript = GameObject.Find ("StageManager").GetComponent<StageManager> ();
	}
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (StageManager.stageLevel >= 2 && isAddCharacter == false) {
			characters.Add(Resources.Load<GameObject>("Prefab/Object/CharacterN004"));
			characters.Add(Resources.Load<GameObject>("Prefab/Object/CharacterN005"));
			characters.Add(Resources.Load<GameObject>("Prefab/Object/CharacterN008"));
			characters.Add(Resources.Load<GameObject>("Prefab/Object/CharacterN011"));
			isAddCharacter = true;
		}
		//キャラを一定時間(interval)ごとに生成する
		if (nextSpawnTime < Time.timeSinceLevelLoad && isStopPlayer == false) {
			nextSpawnTime = Time.timeSinceLevelLoad + interval;
			LocalInstantate ();
		}
	}

	void LocalInstantate( ) {
		Debug.Log ("LocalInstantate in");
		//どのキャラを入店させるかの乱数生成
		int rand = Random.Range (0, characters.Count - comeCharacterList.Count);

		//全てのキャラが出ていないなら
		if (characters.Count > comeCharacterList.Count) {
			//ダブリチェック
			rand = checkDoubleCharacter (rand);

			//全てのキャラが出ていなければ
			if (rand <= characters.Count) {
				//キャラを指定の位置に生成
				GameObject obj = (GameObject)GameObject.Instantiate (characters [rand]);
				obj.transform.parent = transform;
				if (StageManager.stageLevel <= 3) {
					obj.transform.localPosition = new Vector3 (-38.0f, 193.0f, 0);
				} else {
					obj.transform.localPosition = new Vector3 (-78.0f, 193.0f, 0);
				}

				//stageLevelによってサイズを変える
				if ( stageManagerScript.stageData [StageManager.stageLevel].stageScale != 1 ) {
					float scale = obj.transform.localScale.x;
					scale *= stageManagerScript.stageData [StageManager.stageLevel].stageScale;
					obj.transform.localScale = new Vector3 (scale, scale, scale);
				}

				//生成したキャラを保存する
				comeCharacterList.Add (obj.name.Substring (0, 13));	//CharacterNXXX の部分

			}
		}
		if (isFirstCustomer == false) {
			SEManager.isSeSound [(int)SEManager.SE_LIST.FIRST_COME_CUSTOMER] = true;
			isFirstCustomer = true;
		}
	}

	//ダブっていたらダブっていない変数に進める
	//[0][1][2][3][4]で2,3が既に生成されていて、残りの0,3,4を0,1,2と捉え、
	//これに2が入ったら足して[4]に進める
	int checkDoubleCharacter(int tempRand) {
		for ( int i = 0; i < comeCharacterList.Count; i++ ) {
			if ( comeCharacterList[i] == characters[tempRand].name){
				tempRand++;
				//再度ダブってないか1から確認する
				tempRand = checkDoubleCharacter (tempRand);
				i = comeCharacterList.Count;
			}
		}
		return tempRand;
	}
}
