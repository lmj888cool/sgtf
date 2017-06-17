using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PoolManager {
	//protected ArrayList poolArrayList;
	//protected MonoBehaviour gameobject;
	public static string HERO_ITEM = "hero";
	public static string CHAPTER_ITEM = "chapter";
	public static string CAMPAIGN_ITEM = "campaign";
	public static string BAG_ITEM = "bagItem_";
	public static string BAG_ITEM_GREEN = "bagItem_green";
	public static string BAG_ITEM_BLUE = "bagItem_blue";
	public static string BAG_ITEM_PURPLE = "bagItem_purple";
	public static string BAG_ITEM_YELLOW = "bagItem_yellow";

	public static string RE_CONNECT= "RE_CONNECT";

	public static string HANDBOOK_ITEM = "handbook_";
	public static string ICON1 = "green";
	public static string ICON2 = "blue";
	public static string ICON3 = "purple";
	public static string ICON4 = "yellow";
	public static string ICON5 = "goldYellow";
	public static string ICON6 = "red";

	public static string EQUIP_INFO = "equipinfo";
	public static string ITEM_INFO = "iteminfo";
	public static string SKILL_INFO = "skillinfo";
	public Dictionary<string,ArrayList> poolCacheDic;
	private static PoolManager _poolManager;
	public PoolManager(){
		//poolArrayList = new ArrayList ();
		poolCacheDic = new Dictionary<string, ArrayList>();
	}

	public static PoolManager getInstance(){//获取单例
		if (_poolManager == null) {
			_poolManager = new PoolManager ();
		}

		return _poolManager;
	}
	public void initPoolByType(string type,MonoBehaviour _gameobject,int num){
		if (!poolCacheDic.ContainsKey (type)) {
			//if (gameobject == null) {
			ArrayList poolArrayList = new ArrayList ();
			poolCacheDic [type] = poolArrayList;
			//gameobject = _gameobject;
			_gameobject.transform.SetParent(null);
			_gameobject.gameObject.SetActive (false);
			poolArrayList.Add (_gameobject);
			for (int k = 0; k < num; k++) {
				MonoBehaviour go = GameObject.Instantiate (_gameobject, _gameobject.transform.position, _gameobject.transform.rotation);
				poolArrayList.Add (go);
			}

			//}

		}

	}

	public MonoBehaviour getGameObject(string _type){
		if (poolCacheDic.ContainsKey (_type)) {
			ArrayList poolArrayList = poolCacheDic [_type];
			if (poolArrayList.Count > 0) {
				MonoBehaviour go = (MonoBehaviour)poolArrayList[0];

				poolArrayList.RemoveAt (0);
				if (go != null) {
					if (poolArrayList.Count == 0) {
						for (int k = 0; k < 4; k++) {

							MonoBehaviour goCopy = GameObject.Instantiate (go, go.transform.position, go.transform.rotation);
							poolArrayList.Add (goCopy);
						}
					}
					go.gameObject.SetActive (true);
					return go;
				} else {
					getGameObject (_type);
				}

			}
		}
		return null;


	}
	public void addToPool(string _type,MonoBehaviour go){
		if (go != null) {
			
			go.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
			go.transform.localScale = new Vector3 (1.0f,1.0f,1.0f);
			go.transform.SetParent (null);
			go.gameObject.SetActive (false);
			poolCacheDic [_type].Add (go);
		}

	}
}
