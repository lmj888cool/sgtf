using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
using Spine;
using Spine.Unity;
public class handBookPanel : BagPanel {
    //public Text info;
    //public Text handBookname;
    //public Image monsterStyle;
	//public Image yinying;

    public Text key1;
    public Text key2;
    public Text key3;
    public Text key4;
    public Text value1;
    public Text value2;
    public Text value3;
    public Text value4;
	public Text id;
	public Button btn1;
	public Button btn2;
	public Button btn3;
	//public SkeletonGraphic skeletonGraphic;
    //public static handBookPanel _demoPanel;
	private Sprite[] sprites;
	private int spriteIndex = 0;//序列帧索引
	private int spriteIndexStart = 0;//序列帧某个动作起始帧
	private int spriteIndexEnd = 0;//序列帧索引某个动作结束帧
	private float spriteChangeSpeed = 0.2f;//序列帧切换速度
	private float spriteChangeTime = 0.0f;//序列帧切换速度


	void Awake () {
		
		//yinying.gameObject.SetActive (false);

		PoolManager.getInstance ().initPoolByType (PoolManager.HANDBOOK_ITEM + poolType,this,5);
	}
	
	// Update is called once per frame
	void Update () {
		if (sprites != null && sprites.Length > 1) {
			if (spriteChangeTime > spriteChangeSpeed){
				spriteChangeTime = 0;
				if (spriteIndexEnd > spriteIndex) {
					icon.sprite = sprites [spriteIndex];
					icon.SetNativeSize ();
					spriteIndex++;
				} else {
					spriteIndex = spriteIndexStart;
				}
			}
		}
		spriteChangeTime += Time.fixedDeltaTime;
		
	}
	public void playAction(string actionName){
		//skeletonGraphic.AnimationState.SetAnimation (0, actionName, false);
		//skeletonGraphic.AnimationState.AddAnimation (0, "stand", true,2.0f);
	}
    public void init(JsonObject jo,string type)
    {
		base.init(jo,0);
		spriteIndex = 0;
		sprites = null;
		key1.text = "";
		key2.text = "";
		key3.text = "";
		key4.text = "";
		value1.text = "";
		value2.text = "";
		value3.text = "";
		value4.text = "";
		//skeletonGraphic.transform.SetParent (null);
		//skeletonGraphic.gameObject.SetActive (false);
		//yinying.gameObject.SetActive (false);
		//monsterStyle.gameObject.SetActive (false);
		btn1.gameObject.SetActive (false);
		btn2.gameObject.SetActive (false);
		btn3.gameObject.SetActive (false);
		if (type == "monster") {
			//monsterStyle.sprite = (Resources.Load(jo["style"].ToString(), typeof(Sprite)) as Sprite);
			//monsterStyle.SetNativeSize();
			//info.text = jo ["desc"].ToString ();
			name.text = jo ["name"].ToString ();
			key1.text = DataManager.getInstance ().monsterDicJson [0] ["hp"].ToString ();
			key2.text = DataManager.getInstance ().monsterDicJson [0] ["pr"].ToString ();
			key3.text = DataManager.getInstance ().monsterDicJson [0] ["sr"].ToString ();
			key4.text = DataManager.getInstance ().monsterDicJson [0] ["speed"].ToString ();

			value1.text = jo ["hp"].ToString ();
			value2.text = jo ["pr"].ToString ();
			value3.text = jo ["sr"].ToString ();
			value4.text = jo ["speed"].ToString ();

			icon.gameObject.SetActive (true);
			// 加载此文件下的所有资源
			sprites = Resources.LoadAll<Sprite> (jo ["style"].ToString ());
			icon.sprite = sprites [spriteIndex];
			icon.SetNativeSize ();
			spriteIndex++;
			//gameObject.SetActive (true);
			spriteIndexEnd = 4;
		} else if ("equip" == type) {
			key1.text = DataManager.getInstance ().equipDicJson [0] ["attackValue"].ToString ();
			key2.text = DataManager.getInstance ().equipDicJson [0] ["defenceValue"].ToString ();
			key3.text = DataManager.getInstance ().equipDicJson [0] ["hpValue"].ToString ();
			key4.text = DataManager.getInstance ().equipDicJson [0] ["heroLevel"].ToString ();

			value1.text = jo ["attackValue"].ToString ();
			value2.text = jo ["defenceValue"].ToString ();
			value3.text = jo ["hpValue"].ToString ();
			value4.text = jo ["heroLevel"].ToString ();
			//info.text = jo ["desc"].ToString ();
			//name.text = jo ["name"].ToString ();
			icon.gameObject.SetActive (true);
			//handBookname.color = DataManager.getInstance ().getColor (jo ["color"].ToString ());
			icon.sprite = (Resources.Load ("icon/" + jo ["icon"].ToString (), typeof(Sprite)) as Sprite);
			icon.SetNativeSize ();
		} else {
			btn1.gameObject.SetActive (true);
			btn2.gameObject.SetActive (true);
			btn3.gameObject.SetActive (true);
			//skeletonGraphic.gameObject.SetActive (true);
			//yinying.gameObject.SetActive (true);
			//skeletonGraphic = HeroManager.getInstance().getSkeletonGraphic(jo["style"].ToString(),skeletonGraphic);
			//skeletonGraphic.gameObject.SetActive(true);
			//skeletonGraphic.transform.SetParent(this.yinying.transform);
			//skeletonGraphic.transform.localPosition = new Vector3 (0.0f,0.0f, 0.0f);
			//skeletonGraphic.AnimationState.SetAnimation (0,"stand",true);
			key1.text = DataManager.getInstance ().heroDicJson [0] ["attack"].ToString ();
			key2.text = DataManager.getInstance ().heroDicJson [0] ["skill1"].ToString ();
			key3.text = DataManager.getInstance ().heroDicJson [0] ["hp"].ToString ();
			key4.text = DataManager.getInstance ().heroDicJson [0] ["attackSpeed"].ToString ();

			value1.text = jo ["attack"].ToString ();
			value2.text = jo ["skill1"].ToString ();
			value3.text = jo ["hp"].ToString ();
			value4.text = jo ["attackSpeed"].ToString ();
		}
		info.text = jo ["desc"].ToString ();
		name.text = jo ["name"].ToString ();
		id.text = " id:" + jo ["id"].ToString ();
        
    }
}
