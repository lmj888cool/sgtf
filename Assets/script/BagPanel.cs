using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
using TFSG;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class BagPanel : Observer {
    public Text info;
    public Text count;
    public Text speed;
    public Text name;
    public Text defence;
    public Text magic_defence;
    public Image icon;
    public Button useBtn;
	public Button hechengBtn;
	public JsonObject data;
	private IconBase ico;
	public string poolType;
	public static BagPanel _demoPanel;
	public bool isUpdate = false;
	void Awake(){
		messageArr.Add (Message.BAG_UPDATE);
		PoolManager.getInstance ().initPoolByType (PoolManager.BAG_ITEM + poolType,this,3);
	}
    // Use this for initialization
    void Start () {
		UGUIEventTrigger.Get (icon.gameObject).AddEventListener (EventTriggerType.PointerClick,OnClick);
		//BagManager.getInstance ().showAll ();
	}
	
	// Update is called once per frame
	void Update () {
		if (notificationQueue.Count > 0) {
			Notification nt = notificationQueue [0];
			notificationQueue.RemoveAt (0);
			switch (nt.name) {
			case Message.BAG_UPDATE:
				{
					JsonObject _data = (JsonObject)nt.data;
					int updateheroId = int.Parse (_data ["id"].ToString ());
					int curheroId = int.Parse (data ["id"].ToString ());
					if (updateheroId == curheroId) {
						data = _data;
						init (data,0);
					}
				}
				break;
			}
		}
	}
	public void init(JsonObject item,int openType)
    {
		useBtn.gameObject.SetActive (false);
		hechengBtn.gameObject.SetActive (false);
		data = item;
		//if (ico != null) {
		//	PoolManager.getInstance ().addToPool (ico.type,ico);
		//}
		JsonObject staticData;// = BagManager.getInstance().getItemStaticData(item);
		if (item.ContainsKey ("itemId")) {
			staticData = BagManager.getInstance().getItemStaticData(item);
		} else {
			staticData = item;
		}

		//ico = (IconBase)PoolManager.getInstance ().getGameObject (staticdata["color"].ToString());
		//ico.init (staticdata);
		//ico.transform.SetParent (this.transform);
		//ico.transform.localScale = new Vector3 (0.6f,0.6f,0.6f);
		//ico.transform.localPosition = new Vector3 (100,-this.GetComponent<Image>().rectTransform.rect.height/2,0);
		if (staticData.ContainsKey ("icon")) {
			icon.sprite = (Resources.Load("icon/" + staticData["icon"].ToString(), typeof(Sprite)) as Sprite);
		} else if(data.ContainsKey ("id")){
			icon.sprite = (Resources.Load("icon/" + staticData["id"].ToString(), typeof(Sprite)) as Sprite);
		}
		//icon.sprite = ("icon/" + staticdata["icon"].ToString(), typeof(Sprite)) as Sprite);
		icon.SetNativeSize();
		name.text = staticData["name"].ToString();
		if (data.ContainsKey ("heroId")) {
			int heroId = int.Parse (data ["heroId"].ToString ());
			//if (heroId == 0) {//被穿戴的装备不会在背包里面显示
			if (heroId > 0) {
				JsonObject herodata = DataManager.getInstance ().heroDicJson [heroId];
				name.text = staticData ["name"].ToString () + "(" + herodata ["name"].ToString () + ")";
			}
			string shuxing = "";
			int attack = int.Parse (data ["attackValue"].ToString ());
			int hp = int.Parse (data ["hpValue"].ToString ());
			int defence = int.Parse (data ["defenceValue"].ToString ());
			if (attack > 0) {
				shuxing += DataManager.getInstance ().equipDicJson [0] ["attackValue"].ToString () + "+" + data ["attackValue"].ToString ();
			}
			if (hp > 0) {
				shuxing += "  " + DataManager.getInstance ().equipDicJson [0] ["hpValue"].ToString () + "+" + data ["hpValue"].ToString ();
			}
			if (defence > 0) {
				shuxing += "  " + DataManager.getInstance ().equipDicJson [0] ["defenceValue"].ToString () + "+" + data ["defenceValue"].ToString ();
			}
			info.text = shuxing;
			//}
		} else {
			info.text = staticData["desc"].ToString();
		}
		if (data.ContainsKey ("level")) {
			name.text = "Lv." + data ["level"].ToString () + " " + name.text;

		}
		//if (item.ContainsKey("color")) {
		//name.color = DataManager.getInstance().getColor(staticdata["color"].ToString());
		//} else {
		//	name.color = DataManager.getInstance().getColor("");
		//}
		if (item.ContainsKey ("count")) {
			count.text = item["count"].ToString();
		} else {
			count.text = "1";
		}

		//info.text = staticData["desc"].ToString();
		if (staticData.ContainsKey ("itemType")) {
			


			if (staticData ["itemType"].ToString () == "8") {
				int heroid = int.Parse(staticData ["heroId"].ToString ());
				JsonObject hd = HeroManager.getInstance().getHeroById (heroid);
				if (hd == null) {//如果没有这个英雄
					int count = int.Parse (item ["count"].ToString ());
					int needcount = int.Parse (staticData ["addExp"].ToString ());
					if (count >= needcount) {
						hechengBtn.gameObject.SetActive (true);
					}
				}

			}
			if (staticData ["itemType"].ToString () == "9") {
				int heroid = int.Parse(staticData ["heroId"].ToString ());
				//JsonObject hd = HeroManager.getInstance().getHeroById (heroid);
				//if (hd == null) {//如果没有这个英雄
					int count = int.Parse (item ["count"].ToString ());
					int needcount = int.Parse (staticData ["addExp"].ToString ());
					if (count >= needcount) {
						hechengBtn.gameObject.SetActive (true);
					}
				//}

			}
		}


		if (openType == 2) {//穿戴显示穿戴按钮
			useBtn.gameObject.SetActive (true);
		}
		BagManager.getInstance ().addItem (this);
    }
	public void OnClick(BaseEventData eventData){
		JsonObject staticData = BagManager.getInstance().getItemStaticData(data);
		if (staticData.ContainsKey ("itemType")) {
			ItemInfo _equipInfo = (ItemInfo)PoolManager.getInstance().getGameObject(PoolManager.ITEM_INFO);
			_equipInfo.transform.SetParent (BagManager.getInstance().getGameScene().transform);
			_equipInfo.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
			_equipInfo.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
			_equipInfo.init (data);
		} else {
			EquipInfo _equipInfo = (EquipInfo)PoolManager.getInstance().getGameObject(PoolManager.EQUIP_INFO);
			_equipInfo.transform.SetParent (BagManager.getInstance().getGameScene().transform);
			_equipInfo.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
			_equipInfo.init (data,0);
			_equipInfo.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
		}


	}
    public void onUse()
    {
        //HeroManager.getInstance().heroscene.selectKind.image.sprite = icon.sprite;
		HeroManager.getInstance ().heroscene.onEquip (this);

        //this.transform.SetParent(HeroManager.getInstance().heroscene.selectKind.transform);
    }
	public void onHeroShardHeCheng(){
		JsonObject userMessage = new JsonObject();
		userMessage.Add ("id",data["id"]);
		//userMessage.Add ("heroId", data.heroId);
		ServerManager.getInstance ().request("area.playerHandler.useItem", userMessage, (data)=>{
			Debug.Log(data.ToString());


		});
	}
}
