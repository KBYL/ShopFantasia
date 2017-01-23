using UnityEngine;
using System.Collections;

public class PlayerControlier : MonoBehaviour {

	private Animator animator;

	//客の動くスピード
	public const float BASIC_MOVE_SPEED = 0.15f;
	public float moveSpeed = BASIC_MOVE_SPEED;

	enum DIR {
		NONE = -1,
		UP,
		DOWN,
		LEFT,
		RIGHT,
	};

	//const int STAGE_MAX = 24;					//ステージの最大数
	const int CHARACTER_MOVE_INTERVAL = 100;	//止まってから歩き出すまでの時間

	bool isUp = false;					//trueで上に歩く
	bool isDown = false;				//trueで下に歩く
	bool isLeft = false;				//trueで左に歩く
	bool isRight = false;				//trueで右に歩く
	float dirX = 1.0f;					//アニメーションのXの向き判定
	float dirY = 0.0f;					//アニメーションのYの向き判定
	const int WALK_COUNT_MAX = 20;		//歩くフレーム数
	int walkCount = 1;					//歩いたフレーム数
	[SerializeField]
	int patternTableCount = 0;			//moveTableの位置
	int time = -1;						//キャラが生成されてからの経過フレーム	バグ防止のため初期値は―1

	int weAreStageLevel = StageManager.stageLevel;	//we are 仕様に耐えるためstageLevelを0123にする characterMoveDataArrayにのみ使う

	private CustomerInStore customerInStoreScript;
	private StageManager stageManagerScript;

	void Awake() {
		animator = GetComponent<Animator> ();
		customerInStoreScript = GameObject.Find ("CharacterManager").GetComponent<CustomerInStore> ();
		stageManagerScript = GameObject.Find ("StageManager").GetComponent<StageManager> ();
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		switch(StageManager.stageLevel){
		case 0:
			weAreStageLevel = 0;
			break;
		case 1:
			weAreStageLevel = 0;
			break;
		case 3:
			weAreStageLevel = 1;
			break;
		case 8:
			weAreStageLevel = 2;
			break;
		case 9:
			weAreStageLevel = 3;
			break;
		}
		moveSpeed = BASIC_MOVE_SPEED * stageManagerScript.stageData[StageManager.stageLevel].stageScale;


		//CHARACTER_MOVE_INTERVALフレームごとに中に入る
		if ( time % CHARACTER_MOVE_INTERVAL == 0 ) {
			//switch(stageManagerScript.characterMoveDataArray[StageManager.stageLevel,patternTableCount]){
			switch(stageManagerScript.characterMoveDataArray[weAreStageLevel,patternTableCount]){
			case (int)DIR.UP:
					isUp = true;
					dirX = 0;
					dirY = 1;
				break;

			case (int)DIR.DOWN:
					isDown = true;
					dirX = 0;
					dirY = -1;
				break;

			case (int)DIR.LEFT:
					isLeft = true;
					dirX = -1;
					dirY = 0;
				break;

			case (int)DIR.RIGHT:
					isRight = true;
					dirX = 1;
					dirY = 0;
				break;
			}
			//このifいる？
			//if (moveTable [0, patternTableCount] != -1) {
			//INTERVAL_FRAMEごとの処理
			patternTableCount++;
			//}

		}
		if (customerInStoreScript.isStopPlayer == false) {
			time++;
		}

		if (customerInStoreScript.isStopPlayer == false) {
			if (isRight) {
				//右 
				if (walkCount<= WALK_COUNT_MAX) {
					walkCount++;
					this.transform.position += new Vector3 (moveSpeed, 0, 0);
				} else if (walkCount> WALK_COUNT_MAX) {
					isRight = false;
					walkCount= 0;
				}
			} else if (isLeft) {
				//左 
				if (walkCount<= WALK_COUNT_MAX) {
					walkCount++;
					this.transform.position += new Vector3 (-moveSpeed, 0, 0);
				} else if (walkCount> WALK_COUNT_MAX) {
					isLeft = false;
					walkCount= 0;
				}
			} else if (isUp) {
				//上 
				if (walkCount<= WALK_COUNT_MAX) {
					walkCount++;
					this.transform.position += new Vector3 (0, moveSpeed, 0);
				} else if (walkCount> WALK_COUNT_MAX) {
					isUp = false;
					walkCount= 0;
				}
			} else if (isDown) {
				//下 
				if (walkCount<= WALK_COUNT_MAX) {
					walkCount++;
					this.transform.position += new Vector3 (0, -moveSpeed, 0);
				} else if (walkCount> WALK_COUNT_MAX) {
					isDown = false;
					walkCount= 0;
				}
			}
		}
		AnimationCollision( );
	}

	public void AnimationCollision( ) {
		//モーション判定用のパラメータ
		animator.SetFloat( "DirX", dirX );
		animator.SetFloat( "DirY", dirY );
	}
}
