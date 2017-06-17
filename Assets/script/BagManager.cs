using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

public class BagManager{

	private static BagManager _bagManager;
	//private DictionaryEntry<int,Hero> _heroArr;
	private Dictionary<int,JsonObject> itemArr;
	private Dictionary<int,JsonObject> equipArr;
	private ArrayList bagItemArr;
	public BagScene _bagScene;
    private GameScene gamescene;
	public static BagManager getInstance(){//获取单例
		if(_bagManager == null){
			_bagManager = new BagManager();
		}
		return _bagManager;
	}
	public BagManager(){
		bagItemArr = new ArrayList ();
		itemArr = new Dictionary<int, JsonObject> ();
		equipArr = new Dictionary<int, JsonObject> ();
		if(DataManager.playerData.ContainsKey("bag")){
			JsonObject heroArr = (DataManager.playerData ["bag"]) as JsonObject;
			if (heroArr != null) {
				initData (heroArr);
			}


		}
	}
	public void initData(JsonObject heroArr){
		itemArr.Clear ();
		equipArr.Clear ();
		List<object> objs;
		//JsonObject objss = (heroArr ["items"]) as JsonObject;
		if(heroArr.ContainsKey ("items")){
			objs = (heroArr ["items"]) as List<object>;
			for(int i = 0; i < objs.Count; i++) {
				JsonObject jo = objs [i] as JsonObject;
				//int itemid = int.Parse(jo["itemId"].ToString());
				int id = int.Parse (jo ["id"].ToString ());
				int count = int.Parse (jo ["count"].ToString ());
				//JsonObject item = DataManager.getInstance ().itemDicJson [itemid] as JsonObject;
				//jo ["staticdata"] = item;
				if (count > 0) {
					itemArr [id] = jo;
				}

				//item ["count"] = jo ["count"];
				//item ["type"] = jo ["type"];
				//Debug.Log (item.ToString());

			}
		}
		if (heroArr.ContainsKey ("equips")) {
			objs = (heroArr ["equips"]) as List<object>;
			for (int i = 0; i < objs.Count; i++) {
				JsonObject jo = objs [i] as JsonObject;
				//int itemid = int.Parse(jo["itemId"].ToString());
				int id = int.Parse (jo ["id"].ToString ());
				int count = int.Parse (jo ["count"].ToString ());
				//JsonObject item = DataManager.getInstance ().equipDicJson [itemid] as JsonObject;
				//jo ["staticdata"] = item;
				if (count > 0) {
					equipArr [id] = jo;
				}

				//item ["count"] = jo ["count"];
				//item ["type"] = jo ["type"];
				//Debug.Log (item.ToString());

			}
		}

	}
    public void setGameScene(GameScene _gamescene)
    {
        gamescene = _gamescene;
    }
	public GameScene getGameScene()
	{
		return gamescene;
	}
    public void Clear()
    {
        for (int i = 0; i < bagItemArr.Count; i++)
        {
			BagPanel go = (BagPanel)bagItemArr[i];
			if (go != null) {
				//go.transform.SetParent(null);
				PoolManager.getInstance ().addToPool (PoolManager.BAG_ITEM + go.poolType,go);
			}
            
        }
		bagItemArr.Clear ();
    }
    public void showEquipByType(string type)
    {
        gamescene.onclickBtn(5);
        _bagScene.onclickBtn(2);
        Clear();
		foreach(KeyValuePair<int,JsonObject> kvp in equipArr){
			JsonObject jo = kvp.Value;

			if(jo["kind"].ToString() == type)
			{
				int heroId = int.Parse(jo["heroId"].ToString());
				if (heroId > 0) {//被穿戴的装备不会在背包里面显示
					_bagScene.add (jo,2);
				}
			}
		}
		foreach(KeyValuePair<int,JsonObject> kvp in equipArr){
			JsonObject jo = kvp.Value;

			if(jo["kind"].ToString() == type)
			{
				int heroId = int.Parse(jo["heroId"].ToString());
				if (heroId == 0) {//被穿戴的装备不会在背包里面显示
					_bagScene.add (jo,2);
				}
			}
		}

    }
    public void showItemByType(string type){
        Clear();
		if(type == "equip")
        {
			foreach(KeyValuePair<int,JsonObject> kvp in equipArr){
				JsonObject jo = kvp.Value;
				int heroId = int.Parse(jo["heroId"].ToString());
				if (heroId > 0) {//被穿戴的装备不会在背包里面显示
					_bagScene.add (jo);
				}
			}
			foreach(KeyValuePair<int,JsonObject> kvp in equipArr){
				JsonObject jo = kvp.Value;
				int heroId = int.Parse(jo["heroId"].ToString());
				if (heroId == 0) {//被穿戴的装备不会在背包里面显示
					_bagScene.add (jo);
				}
			}
        }
        else
        {
			foreach(KeyValuePair<int,JsonObject> kvp in itemArr){
				//JsonObject staticdata = kvp.Value ["staticdata"] as JsonObject;
				if (type == kvp.Value["itemType"].ToString()) {
					_bagScene.add (kvp.Value);
				}
			}
        }

    }
	public void showAll(){
        Clear();
        //if (bagItemArr.Count == 0){//
		foreach(KeyValuePair<int,JsonObject> kvp in itemArr){
			_bagScene.add(kvp.Value);
		}
		foreach(KeyValuePair<int,JsonObject> kvp in equipArr){
			_bagScene.add(kvp.Value);
		}
		//}
	}
	public JsonObject getItemById(int id){
		//heroData hd2;
		if (itemArr.ContainsKey (id)) {
			return itemArr [id];
		}
		return null;
	}
	public JsonObject getEquipById(int id){
		//heroData hd2;
		if (equipArr.ContainsKey (id)) {
			return equipArr [id];
		}
		return null;
	}
	public ArrayList getEquipByHeroId(int heroid){
		ArrayList arr = new ArrayList ();
		foreach(KeyValuePair<int,JsonObject> kvp in equipArr){
			if (int.Parse (kvp.Value ["heroId"].ToString ()) == heroid) {
				arr.Add (kvp.Value);
			}
		}
		return arr;
	}
	public JsonObject getItemByItemId(int itemid){
		ArrayList arr = new ArrayList ();
		foreach(KeyValuePair<int,JsonObject> kvp in itemArr){
			if (int.Parse (kvp.Value ["itemId"].ToString ()) == itemid) {
				return kvp.Value;
			}
		}
		return null;
	}
	public JsonObject getEquipByHeroIdAndKind(int heroid,string kind){
		JsonObject data = null;
		foreach(KeyValuePair<int,JsonObject> kvp in equipArr){
			if (int.Parse (kvp.Value ["heroId"].ToString ()) == heroid && kvp.Value ["kind"].ToString () == kind) {
				data = kvp.Value;
				break;
			}
		}
		return data;
	}
	public JsonObject getEquipByHeroIdAndItemId(int heroid,int itemid){
		JsonObject data = null;
		foreach(KeyValuePair<int,JsonObject> kvp in equipArr){
			if (int.Parse (kvp.Value ["heroId"].ToString ()) == heroid && int.Parse(kvp.Value ["itemId"].ToString ()) == itemid) {
				data = kvp.Value;
				break;
			}
		}
		return data;
	}
	public void addItem(BagPanel item)
	{
		bagItemArr.Add(item);
	}
	public void showItemsByItemType(int type)
	{
		Clear();
		foreach(KeyValuePair<int,JsonObject> kvp in itemArr){
			//JsonObject staticdata = kvp.Value ["staticdata"] as JsonObject;
			if (type == int.Parse(kvp.Value["itemType"].ToString())) {
				_bagScene.add (kvp.Value);
			}
		}
	}
	public void removeItemById(int id){
		if (itemArr.ContainsKey (id)){
			itemArr.Remove (id);
		}
	}
	public void removeEquipById(int id){
		if (equipArr.ContainsKey (id)){
			equipArr.Remove (id);
		}
	}
	public void updateItemByServer(JsonObject data){
		//DataManager.playerData ["bag"] = data;
		int itemId = int.Parse(data["itemId"].ToString());
		int id = int.Parse(data["id"].ToString());
		if (itemId > 8000) {//装备
			equipArr[id] = data;
		} else {
			itemArr [id] = data;
		}
		NotificationManager.getInstance ().PostNotification (null,Message.EQUIP_LEVELUP,null);
		//if (itemId == 100 || itemId == 101) {
			NotificationManager.getInstance ().PostNotification (null,Message.MONEY_GOLD_UPDATE,null);
		//}
		NotificationManager.getInstance ().PostNotification (null,Message.BAG_UPDATE,data);
		//NotificationCenter.DefaultCenter ().PostNotification (null,"initBase",data);
		//initData (data);
	}
	public JsonObject getItemStaticData(JsonObject item){
		int itemid = int.Parse (item ["itemId"].ToString ());
		if (itemid > 8000) {//装备
			if (DataManager.getInstance ().equipDicJson.ContainsKey (itemid))
				return DataManager.getInstance ().equipDicJson [itemid];
		} else {
			if (DataManager.getInstance ().itemDicJson.ContainsKey (itemid))
				return DataManager.getInstance ().itemDicJson [itemid];
		}
		return null;
	}
}
