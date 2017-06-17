using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class HeadPanel : Observer {
	public Text playerName;//角色名称
	public Text playerShiLi;//角色等级
	public Text gold;
	public Text zuanshi;
	public bool isNeedUpdate = false;
	void Awake () {
		messageArr.Add (Message.MONEY_GOLD_UPDATE);
	}
	// Use this for initialization
	void Start () {
		UpdateData ();
		playerName.text = DataManager.playerData["name"].ToString();
	}
	
	// Update is called once per frame
	void UpdateData () {
		JsonObject jo1 = BagManager.getInstance ().getItemByItemId (101);
		if (jo1 == null) {
			gold.text = "0";
		} else {
			gold.text = jo1 ["count"].ToString ();
		}

		JsonObject jo2 = BagManager.getInstance ().getItemByItemId (100);
		if (jo2 == null) {
			zuanshi.text = "0";
		} else {
			zuanshi.text = jo2 ["count"].ToString ();
		}
	}
	void Update () {
		if (notificationQueue.Count > 0) {
			Notification nt = notificationQueue [0];
			notificationQueue.RemoveAt (0);
			switch (nt.name) {
			case Message.MONEY_GOLD_UPDATE:
				{
					UpdateData ();
				}
				break;
			}
		}
	}
}
