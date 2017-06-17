using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
using System.Text.RegularExpressions;
public class EquipInfo :Observer {
	public Text panelName;
	public Text itemName;
	public Text itemShuXing;
	public Text equipFightPointer;
	public Text itemInfo;
	public JsonObject data;
	public Image icon;
	public Image bg;
	public Image doPanel;//操作界面
	public Image FightPointerPanel;
	public string type;
	public string kind;
	public int equipId;
	public Button changeBtn;
	public Button fumoBtn;
	public Button levelupBtn;
	public Text levelUpNeedInfo;
	public Text fumoNeedInfo;

	public Text weapon;
	public Text armor;
	public Text shoes;
	public Text amulet;
	public Text suitName;
	public Text suit2;
	public Text suit3;
	public Text suit4;
	public Dictionary<string,Text> suitArr;
	public Image suitPanel;
	public bool isFlesh = false;
	// Use this for initialization
	void Awake () {
		suitArr = new Dictionary<string, Text> ();
		suitArr ["weapon"] = weapon;
		suitArr ["armor"] = armor;
		suitArr ["shoes"] = shoes;
		suitArr ["amulet"] = amulet;
		suitArr ["name"] = suitName;
		suitArr ["suit2"] = suit2;
		suitArr ["suit3"] = suit3;
		suitArr ["suit4"] = suit4;
		messageArr.Add (Message.EQUIP_LEVELUP);
		PoolManager.getInstance ().initPoolByType (type,this,1);
	}
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
		if (notificationQueue.Count > 0) {
			Notification nt = notificationQueue [0];
			notificationQueue.RemoveAt (0);
			switch (nt.name) {
			case Message.EQUIP_LEVELUP:
				{
					if (data != null) {
						int id = int.Parse (data ["id"].ToString ());
						data = BagManager.getInstance ().getEquipById (id);
						initBase (BagManager.getInstance ().getItemStaticData (data));
					}
				}
				break;
			}
		}
	}
	public void initBase(JsonObject jo){
		/// <summary>
		/// 初始化武器信息
		/// </summary>
		kind = jo["kind"].ToString();
		equipId = int.Parse(jo["id"].ToString());
		if (jo.ContainsKey ("icon")) {
			icon.sprite = (Resources.Load("icon/" + jo["icon"].ToString(), typeof(Sprite)) as Sprite);
		} else {
			icon.sprite = (Resources.Load("icon/" + jo["id"].ToString(), typeof(Sprite)) as Sprite);
		}
		icon.SetNativeSize();
		itemName.text = "Lv."+data ["level"].ToString () + " " + jo ["name"].ToString ();
		itemName.color = DataManager.getInstance ().getColor (jo ["color"].ToString ());
		itemInfo.text = jo ["desc"].ToString ();

		bg.sprite = (Resources.Load("all/hero_bg_" + jo["color"].ToString(), typeof(Sprite)) as Sprite);
		string shuxing = "";
		int attack = int.Parse (data ["attackValue"].ToString ());
		int hp = int.Parse (data ["hpValue"].ToString ());
		int defence = int.Parse (data ["defenceValue"].ToString ());
		if( attack > 0){
			shuxing += DataManager.getInstance ().equipDicJson [0] ["attackValue"].ToString () + "+" + data ["attackValue"].ToString ();
		}
		if( hp > 0){
			shuxing += "  " + DataManager.getInstance ().equipDicJson [0] ["hpValue"].ToString () + "+" + data ["hpValue"].ToString ();
		}
		if( defence > 0){
			shuxing += "  " + DataManager.getInstance ().equipDicJson [0] ["defenceValue"].ToString () + "+" + data ["defenceValue"].ToString ();
		}
		itemShuXing.text = shuxing;

		equipFightPointer.text = ((defence + hp + attack) * 5).ToString();



		fumoBtn.gameObject.SetActive (true);
		levelupBtn.gameObject.SetActive (true);

		//升星更新
		updateBtn(levelupBtn,levelUpNeedInfo,101,"level","equipLevelUpNeed","levelUp");	

		//附魔更新
		updateBtn(fumoBtn,fumoNeedInfo,100,"level","equipFuMoNeed","levelUp");	
	}
	public void init(JsonObject jo,int openType){
		//NotificationManager.getInstance ().AddObserver (this,"equip_levelup");
		data = jo;
		//if (jo.ContainsKey ("staticdata")) {
		jo = BagManager.getInstance ().getItemStaticData (jo);
		//}
		initBase(jo);
		changeBtn.gameObject.SetActive (true);
		int heroId = int.Parse(data["heroId"].ToString());
		if (heroId == 0 || openType == 0) {
			changeBtn.gameObject.SetActive (false);
		}

		initSuit (jo);
	}
	public void initSuit(JsonObject jo){//套装系统
		JsonObject suit = DataManager.getInstance ().getSuitByEquip (jo);
		int heroId = int.Parse(data["heroId"].ToString());
		int suitNum = 0;
		if (suit != null) {
			suitNum += 1;
			suitPanel.gameObject.SetActive (true);
			foreach (KeyValuePair<string,object> kvp in suit) {
				if (suitArr.ContainsKey(kvp.Key)) {
					string value = kvp.Value.ToString ();
					bool isInt = Regex.IsMatch (value, @"^[+-]?\d*$");
					if (isInt && heroId >0) {
						
						JsonObject equip = DataManager.getInstance ().equipDicJson [int.Parse(value)];
						suitArr [kvp.Key].text = equip ["name"].ToString ();
						if (kvp.Key == kind) {
							suitArr [kvp.Key].color = DataManager.getInstance ().getColor (equip ["color"].ToString ());
						} else {
							JsonObject otherEquip = BagManager.getInstance ().getEquipByHeroIdAndItemId (heroId,int.Parse(value));

							if (otherEquip != null) {
								suitNum += 1;
								JsonObject otherEquipStaticData = BagManager.getInstance ().getItemStaticData (otherEquip);
								suitArr [kvp.Key].color = DataManager.getInstance ().getColor (otherEquipStaticData ["color"].ToString ());
							} else {
								suitArr [kvp.Key].color = Color.gray;
							}

						}
					} else {
						if (kvp.Key.IndexOf ("suit") >= 0) {
							string format = DataManager.getInstance ().suitJson [0] [kvp.Key].ToString ();
							string[] shuxingArr = value.Split('_');
							if (shuxingArr.Length == 2) {
								suitArr [kvp.Key].text = string.Format (format, shuxingArr [0], shuxingArr [1]);
							} else if (shuxingArr.Length == 3) {
								suitArr [kvp.Key].text = string.Format (format, shuxingArr [0], shuxingArr [1],shuxingArr [2]);
							} else {
								suitArr [kvp.Key].text = value;
							}

		
						} else {
							suitArr [kvp.Key].text = value;
						}

						suitArr [kvp.Key].color = Color.gray;
					}
				}
			}
			suitArr["name"].color =suitArr [kind].color;
			if (suitNum == 2) {
				suitArr ["suit2"].color = Color.green;
			} else if (suitNum == 3) {
				suitArr ["suit2"].color = Color.green;
				suitArr ["suit3"].color = Color.green;
			} else if (suitNum == 4) {
				suitArr ["suit2"].color = Color.green;
				suitArr ["suit3"].color = Color.green;
				suitArr ["suit4"].color = Color.green;
			}
		} else {
			suitPanel.gameObject.SetActive (false);
		}
	}
	public void onClose(){
		AudioManager.instance.playCloseClick ();
		//NotificationManager.getInstance ().RemoveObserver (this,"equip_levelup");
		PoolManager.getInstance ().addToPool (type,this);
	}
	public void OnChange(){//更换武器
		PoolManager.getInstance ().addToPool (type,this);
		BagManager.getInstance ().showEquipByType (kind);
	}
	public void OnFuMo(){//武器附魔
	}
	public void OnLevelUp(){//武器升级
		JsonObject userMessage = new JsonObject();
		userMessage.Add ("equipId", data["id"]);
		//userMessage.Add ("heroId", heroId);

		ServerManager.getInstance ().request("area.equipHandler.equipLevelUp", userMessage, (data)=>{
			Debug.Log(data.ToString());
			//isFlesh = true;

		});
	}

	public void updateBtn(Button btn,Text txt,int itemid,string level,string need,string dataDicName){
		JsonObject item = BagManager.getInstance ().getItemByItemId (itemid);
		int nextLevel = int.Parse(data[level].ToString()) + 1;
		if (DataManager.getInstance ().dataDic[dataDicName].ContainsKey (nextLevel)) {
			btn.gameObject.SetActive (true);
			JsonObject jo8 = DataManager.getInstance ().dataDic[dataDicName][nextLevel];
			int haveNum = 0;
			int needNum = int.Parse (jo8 [need].ToString ());
			if (item != null) {
				haveNum = int.Parse (item ["count"].ToString ());
				txt.text = item["count"].ToString() + "/" + jo8[need].ToString();
			} else {
				haveNum = 0;
				txt.text =  "0/" + jo8[need].ToString();
			}
			if (haveNum < needNum) {
				//btn.interactable = false;
				txt.color = Color.red;
			} else {
				//btn.interactable = true;
				txt.color = Color.white;
			}

		} else {
			btn.gameObject.SetActive (false);
		}
	}
}
