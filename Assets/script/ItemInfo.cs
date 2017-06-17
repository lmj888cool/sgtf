using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;

public class ItemInfo : MonoBehaviour {
	public Text panelName;
	public Text itemName;
	public Text itemShuLiang;
	public Text itemInfo;
	public JsonObject data;
	public Image icon;
	public Image zhandouliPanel;
	public Image bg;
	public string type;
	// Use this for initialization
	void Awake () {
		zhandouliPanel.gameObject.SetActive (false);
		PoolManager.getInstance ().initPoolByType (type,this,1);
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void init(JsonObject jo){
		data = jo;
		//if (jo.ContainsKey ("staticdata")) {
			jo = BagManager.getInstance ().getItemStaticData (jo);
		//}
		/// <summary>
		/// 初始化武器信息
		/// </summary>
		if (jo.ContainsKey ("icon")) {
			icon.sprite = (Resources.Load("icon/" + jo["icon"].ToString(), typeof(Sprite)) as Sprite);
		} else {
			icon.sprite = (Resources.Load("icon/" + jo["id"].ToString(), typeof(Sprite)) as Sprite);
		}
		icon.SetNativeSize();
		itemName.text = jo ["name"].ToString ();
		itemInfo.text = jo ["desc"].ToString ();

		bg.sprite = (Resources.Load("all/hero_bg_" + jo["color"].ToString(), typeof(Sprite)) as Sprite);
		itemShuLiang.text = "x" + data ["count"].ToString ();
	}
	public void onClose(){
		PoolManager.getInstance ().addToPool (type,this);
	}
}
