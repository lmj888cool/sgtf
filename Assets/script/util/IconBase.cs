using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class IconBase : MonoBehaviour {

	public Image icon;
	public Image sub;
	public string type;
	private JsonObject data;
	public callBackFunc<JsonObject> Func;

	void Awake () {
		
		PoolManager.getInstance ().initPoolByType (type,this,5);
	}
	// Use this for initialization
	void Start () {
		//UGUIEventTrigger.Get (icon.gameObject).AddEventListener (EventTriggerType.PointerClick,OnClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public IconBase init(JsonObject jo){
		sub.gameObject.SetActive (false);
		data = jo;
		Func = null;
		if (jo.ContainsKey ("icon")) {
			icon.sprite = (Resources.Load("icon/" + data["icon"].ToString(), typeof(Sprite)) as Sprite);
		} else {
			icon.sprite = (Resources.Load("icon/" + data["id"].ToString(), typeof(Sprite)) as Sprite);
		}
		if (jo.ContainsKey ("heroId")) {//英雄碎片
			if (int.Parse (jo ["heroId"].ToString ()) > 0) {
				sub.gameObject.SetActive (true);
			}
		}
		icon.SetNativeSize();
		return this;
	}
	public void onClick(){
		AudioManager.instance.playBtnClick ();
		if (Func != null) {
			Func (data);
		}
	}
}
