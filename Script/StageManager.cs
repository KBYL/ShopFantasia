using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StageManager : MonoBehaviour {

	public enum SEASON {
		SPRING,
		SUMMER,
		AUTUMN,
		WINTER,
	}

	public enum RANK {
		C,
		B,
		A,
		S,
	};

	public struct STAGE_DATA {
		public int stage;
		public int mouth;
		public int season;
		public float stageScale;
		public int firstMoney;
		public int targetMoney;
		public string stageName;
		public string bigStageNameID;
		public string UIStageNameID;
		public string bgmID;
		public int week;
		public int purchaseBuyMax;
		public int purchaseNum;
		public int priceUpProduct1;
		public double priceUpPercent1;
		public int priceUpProduct2;
		public double priceUpPercent2;
		public int priceUpProduct3;
		public double priceUpPercent3;
		public int priceDownProduct1;
		public double priceDownPercent1;
		public int priceDownProduct2;
		public double priceDownPercent2;
		public int priceDownProduct3;
		public double priceDownPercent3;
		public int priceDownProduct4;
		public double priceDownPercent4;
		public int favoriteProduct1;
		public int favoriteProduct2;
		public double rankC;
		public double rankB;
		public double rankA;
		public double rankS;
		public STAGE_DATA(int Stage, int Mouth, int Season, float StageScale, int FirstMoney, int TargetMoney, string StageName, string BigStageNameID, string UISTAGENameID, string BgmID, int Week, int PurchaseBuyMax, int PurchaseNum, int PriceUpProduct1, double PriceUpPercent1, int PriceUpProduct2, double PriceUpPercent2, int PriceUpProduct3, double PriceUpPercent3, int PriceDownProduct1, double PriceDownPercent1,int PriceDownProduct2, double PriceDownPercent2, int PriceDownProduct3, double PriceDownPercent3, int PriceDownProduct4, double PriceDownPercent4, int FavoriteProduct1, int FavoriteProduct2, double RankC, double RankB, double RankA, double RankS ) {
			stage = Stage;
			mouth = Mouth;
			season = Season;
			stageScale = StageScale;
			firstMoney = FirstMoney;
			targetMoney = TargetMoney;
			stageName = StageName;
			bigStageNameID = BigStageNameID;
			UIStageNameID = UISTAGENameID;
			bgmID = BgmID;
			week = Week;
			purchaseBuyMax = PurchaseBuyMax;
			purchaseNum = PurchaseNum;
			priceUpProduct1 = PriceUpProduct1;
			priceUpPercent1 = PriceUpPercent1;
			priceUpProduct2 = PriceUpProduct2;
			priceUpPercent2 = PriceUpPercent2;
			priceUpProduct3 = PriceUpProduct3;
			priceUpPercent3 = PriceUpPercent3;
			priceDownProduct1 = PriceDownProduct1;
			priceDownPercent1 = PriceDownPercent1;
			priceDownProduct2 = PriceDownProduct2;
			priceDownPercent2 = PriceDownPercent2;
			priceDownProduct3 = PriceDownProduct3;
			priceDownPercent3 = PriceDownPercent3;
			priceDownProduct4 = PriceDownProduct4;
			priceDownPercent4 = PriceDownPercent4;
			favoriteProduct1 = FavoriteProduct1;
			favoriteProduct2 = FavoriteProduct2;
			rankC = RankC;
			rankB = RankB;
			rankA = RankA;
			rankS = RankS;
		}
	}

	public struct CHARACTER_MOVE_DATA {
		public int one;
		public int two;
		public int three;
		public int four;
		public int five;
		public int six;
		public int seven;
		public int eight;
		public int nine;
		public int ten;
		public int eleven;
		public int twelve;
		public int thirteen;
		public int forteen;
		public int fifteen;
		public int sixteen;
		public int seventeen;
		public int eighteen;
		public int nineteen;
		public int twenty;
		public CHARACTER_MOVE_DATA(int One, int Two, int Three, int Four, int Five, int Six, int Seven, int Eight, int Nine, int Ten, int Eleven, int Twelve, int Thirteen, int Forteen, int Fifteen, int Sixteen, int Seventeen, int Eighteen, int Nineteen, int Twenty) {
			one = One;
			two = Two;
			three = Three;
			four = Four;
			five = Five;
			six = Six;
			seven = Seven;
			eight = Eight;
			nine = Nine;
			ten = Ten;
			eleven = Eleven;
			twelve = Twelve;
			thirteen = Thirteen;
			forteen = Forteen;
			fifteen = Fifteen;
			sixteen = Sixteen;
			seventeen = Seventeen;
			eighteen = Eighteen;
			nineteen = Nineteen;
			twenty = Twenty;
		}
	}

	[SerializeField]
	public static int stageLevel = 1;
	//we are は1,3,8,9を起用

	public static int stageLevelMax = stageLevel;

	public Image backGroundImage;
	public Sprite springBackGroundImage;
	public Sprite summerBackGroundImage;
	public Sprite autumnBackGroundImage;
	public Sprite winterBackGroundImage;
	public AudioSource bgmAudio;
	public GameObject firstStageMap;
	public GameObject lateStageMap;

	public const int STAGE_MAX = 24;

	public List<STAGE_DATA> stageData = new List<STAGE_DATA>();
	public List<CHARACTER_MOVE_DATA> characterMoveData = new List<CHARACTER_MOVE_DATA> ();
	public int[,] characterMoveDataArray;


	//そうだ、stageMapをプレハブ化しよう！

	// Use this for initialization
	void Awake () {
		Entity_StageData entityStageData = Resources.Load ("Data/StageData") as Entity_StageData;
		Entity_CharacterMoveData entityCharacterMoveData = Resources.Load ("Data/CharacterMoveData") as Entity_CharacterMoveData;

		//stageDataにデータを入れる
		for (int i = 0; i < entityStageData.param.Count; i++) {
			stageData.Add (new STAGE_DATA (
				entityStageData.param [i].stage,
				entityStageData.param [i].mouth,
				entityStageData.param [i].season,
				entityStageData.param [i].stageScale,
				entityStageData.param [i].firstMoney,
				entityStageData.param [i].targetMoney,
				entityStageData.param [i].stageName,
				entityStageData.param [i].bigStageNameID,
				entityStageData.param [i].UIStageNameID,
				entityStageData.param [i].bgmID,
				entityStageData.param [i].week,
				entityStageData.param [i].purchaseBuyMax,
				entityStageData.param [i].purchaseNum,
				entityStageData.param [i].priceUpProduct1,
				entityStageData.param [i].priceUpPercent1,
				entityStageData.param [i].priceUpProduct2,
				entityStageData.param [i].priceUpPercent2,
				entityStageData.param [i].priceUpProduct3,
				entityStageData.param [i].priceUpPercent3,
				entityStageData.param [i].priceDownProduct1,
				entityStageData.param [i].priceDownPercent1,
				entityStageData.param [i].priceDownProduct2,
				entityStageData.param [i].priceDownPercent2,
				entityStageData.param [i].priceDownProduct3,
				entityStageData.param [i].priceDownPercent3,
				entityStageData.param [i].priceDownProduct4,
				entityStageData.param [i].priceDownPercent4,
				entityStageData.param [i].favoriteProduct1,
				entityStageData.param [i].favoriteProduct2,
				entityStageData.param [i].rankC,
				entityStageData.param [i].rankB,
				entityStageData.param [i].rankA,
				entityStageData.param [i].rankS)

			);

		}

		characterMoveDataArray = new int[4,20] {
			{ entityCharacterMoveData.param[0].one, entityCharacterMoveData.param[0].two, entityCharacterMoveData.param[0].three, entityCharacterMoveData.param[0].four, entityCharacterMoveData.param[0].five, entityCharacterMoveData.param[0].six, entityCharacterMoveData.param[0].seven, entityCharacterMoveData.param[0].eight, entityCharacterMoveData.param[0].nine, entityCharacterMoveData.param[0].ten, entityCharacterMoveData.param[0].eleven, entityCharacterMoveData.param[0].twelve, entityCharacterMoveData.param[0].thirteen, entityCharacterMoveData.param[0].forteen, entityCharacterMoveData.param[0].fifteen, entityCharacterMoveData.param[0].sixteen, entityCharacterMoveData.param[0].seventeen, entityCharacterMoveData.param[0].eighteen, entityCharacterMoveData.param[0].nineteen, entityCharacterMoveData.param[0].twenty},
			{ entityCharacterMoveData.param[1].one, entityCharacterMoveData.param[1].two, entityCharacterMoveData.param[1].three, entityCharacterMoveData.param[1].four, entityCharacterMoveData.param[1].five, entityCharacterMoveData.param[1].six, entityCharacterMoveData.param[1].seven, entityCharacterMoveData.param[1].eight, entityCharacterMoveData.param[1].nine, entityCharacterMoveData.param[1].ten, entityCharacterMoveData.param[1].eleven, entityCharacterMoveData.param[1].twelve, entityCharacterMoveData.param[1].thirteen, entityCharacterMoveData.param[1].forteen, entityCharacterMoveData.param[1].fifteen, entityCharacterMoveData.param[1].sixteen, entityCharacterMoveData.param[1].seventeen, entityCharacterMoveData.param[1].eighteen, entityCharacterMoveData.param[1].nineteen, entityCharacterMoveData.param[1].twenty},
			{ entityCharacterMoveData.param[2].one, entityCharacterMoveData.param[2].two, entityCharacterMoveData.param[2].three, entityCharacterMoveData.param[2].four, entityCharacterMoveData.param[2].five, entityCharacterMoveData.param[2].six, entityCharacterMoveData.param[2].seven, entityCharacterMoveData.param[2].eight, entityCharacterMoveData.param[2].nine, entityCharacterMoveData.param[2].ten, entityCharacterMoveData.param[2].eleven, entityCharacterMoveData.param[2].twelve, entityCharacterMoveData.param[2].thirteen, entityCharacterMoveData.param[2].forteen, entityCharacterMoveData.param[2].fifteen, entityCharacterMoveData.param[2].sixteen, entityCharacterMoveData.param[2].seventeen, entityCharacterMoveData.param[2].eighteen, entityCharacterMoveData.param[2].nineteen, entityCharacterMoveData.param[2].twenty},
			{ entityCharacterMoveData.param[3].one, entityCharacterMoveData.param[3].two, entityCharacterMoveData.param[3].three, entityCharacterMoveData.param[3].four, entityCharacterMoveData.param[3].five, entityCharacterMoveData.param[3].six, entityCharacterMoveData.param[3].seven, entityCharacterMoveData.param[3].eight, entityCharacterMoveData.param[3].nine, entityCharacterMoveData.param[3].ten, entityCharacterMoveData.param[3].eleven, entityCharacterMoveData.param[3].twelve, entityCharacterMoveData.param[3].thirteen, entityCharacterMoveData.param[3].forteen, entityCharacterMoveData.param[3].fifteen, entityCharacterMoveData.param[3].sixteen, entityCharacterMoveData.param[3].seventeen, entityCharacterMoveData.param[3].eighteen, entityCharacterMoveData.param[3].nineteen, entityCharacterMoveData.param[3].twenty},
		};



	}

	void Start() {
		SetStage ();
	}

	// Update is called once per frame
	void Update () {
		Debug.Log ("stagelevel " + stageLevel);
	}

	void SetStage() {
		//背景の画像
		switch(stageData[stageLevel].season) {
		case (int)SEASON.SPRING:
			backGroundImage.sprite = springBackGroundImage;
			break;
		case (int)SEASON.SUMMER:
			backGroundImage.sprite = summerBackGroundImage;
			break;
		case (int)SEASON.AUTUMN:
			backGroundImage.sprite = autumnBackGroundImage;
			break;
		case (int)SEASON.WINTER:
			backGroundImage.sprite = winterBackGroundImage;
			break;
		}

		//BGMの設定、再生
		bgmAudio.clip = Resources.Load <AudioClip>("Sound/" + stageData [stageLevel].bgmID);
		bgmAudio.Play ();

		//店の画像
		if (stageLevel <= 3) {
			firstStageMap.SetActive (true);
			lateStageMap.SetActive (false);
		} else {
			firstStageMap.SetActive (false);
			lateStageMap.SetActive (true);
		}
	}

}
