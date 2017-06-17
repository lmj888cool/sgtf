using UnityEngine;
using System.Collections;
using TFSG;
using System.Collections.Generic;
using System;
using SimpleJson;
using Spine;
using Spine.Unity;
public class HeroManager {
	private static HeroManager _heroManager;
    public HeroScene heroscene;
	private Dictionary<string,SkeletonGraphic> spinePool;
	private Dictionary<int,JsonObject> HeroArr;

	public static HeroManager getInstance(){//获取单例
		if(_heroManager == null){
			_heroManager = new HeroManager();
		}
		return _heroManager;
	}

	public HeroManager(){
		spinePool = new Dictionary<string, SkeletonGraphic> ();
		HeroArr = new Dictionary<int, JsonObject> ();
		if(DataManager.playerData.ContainsKey("heros")){
			List<object> joList = DataManager.playerData ["heros"] as List<object>;
			//JsonArray heroArr = (DataManager.playerData ["heros"]) as JsonArray;
			//Debug.Log (heroStr);
			//heroStr = heroStr.Substring (1,heroStr.Length -2);
			//string[] heroArr = heroStr.Split (",".ToCharArray (), StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < joList.Count; i++) {
				JsonObject hero = joList[i] as JsonObject;
				//Debug.Log (hero.ToString());
				//hero = hero.Substring (1,heroStr.Length -2);
				//string[] heroArr2 = hero.Split (":".ToCharArray (), StringSplitOptions.RemoveEmptyEntries);
				//heroData _herodata = new heroData ();
				//int heroId = int.Parse(hero["id"].ToString());
				updateData (hero);

			}

			
		}
        //初始的三位英雄，每位玩家都有

        /**addHero(DataManager.getInstance().heroDic[2]);
        addHero(DataManager.getInstance().heroDic[3]);
        addHero(DataManager.getInstance().heroDic[4]);
		addHero(DataManager.getInstance().heroDic[5]);
		addHero(DataManager.getInstance().heroDic[6]);
		addHero(DataManager.getInstance().heroDic[7]);
		addHero(DataManager.getInstance().heroDic[8]);
		addHero(DataManager.getInstance().heroDic[9]);
		addHero(DataManager.getInstance().heroDic[10]);**/
    }

	public JsonObject getHeroById(int heroid){
		//heroData hd2;
		if (HeroArr.ContainsKey (heroid)) {
			return HeroArr[heroid];
		}
		return null;
	}
	//public void addHero(JsonObject hero)
    ///{
    //    HeroArr.Add(hero);
    //}
	public Dictionary<int,JsonObject> getHeros()
    {
		return HeroArr;
	}
	public ArrayList getHerosArrayList(){
		ArrayList arr = new ArrayList ();
		foreach (KeyValuePair<int,JsonObject> kvp in HeroArr) {
			arr.Add (kvp.Value);
		}
		return arr;
	}
	public void updateData(JsonObject hero){
		int heroid = int.Parse (hero ["heroId"].ToString ());
		//JsonObject _herodata = getHeroById(heroid);
		//if (_herodata == null) {
			//_herodata = new JsonObject ();
			//_herodata["staticdata"] = DataManager.getInstance ().heroDicJson[heroid];
		//	addHero (hero);
		//}
		//_herodata ["data"] = hero;
		HeroArr[heroid] = hero;
		//return hero;
	}
	public JsonObject getHeroStaticData(JsonObject hero){
		int heroid = int.Parse (hero ["heroId"].ToString ());
		if(DataManager.getInstance ().heroDicJson.ContainsKey(heroid))
			return DataManager.getInstance ().heroDicJson[heroid];
		return null;
	}
	public void updateHeroByServer(JsonObject herodata){
		updateData (herodata);
		//heroscene.addHero (hd);
		NotificationManager.getInstance().PostNotification(null,Message.HERO_UPDATE,herodata);
		/**if (heroscene.data == null) {
			heroscene.updateHero (herodata);
		} else {
			int updateheroId = int.Parse(herodata["heroId"].ToString());
			int curheroId = int.Parse(heroscene.heroId.ToString());
			if (updateheroId == curheroId) {
				heroscene.updateHero (herodata);
			}
		}**/

	}
	/**public SkeletonAnimation getSpine(string spineName){
		if (!spinePool.ContainsKey (spineName)) {
			SkeletonDataAsset yourSkeletonDataAsset = Resources.Load<SkeletonDataAsset> ("spine/" + spineName + "/" + spineName + "_SkeletonData");
			SkeletonAnimation newSkeletonAnimation = SkeletonAnimation.NewSkeletonAnimationGameObject (yourSkeletonDataAsset);
			//heroscene.DontDestroyOnLoad(newSkeletonAnimation.transform.gameObject);
			newSkeletonAnimation.calculateNormals = true;
			newSkeletonAnimation.loop = true;
			newSkeletonAnimation.AnimationName = "stand";
			newSkeletonAnimation.transform.localPosition = new Vector3 (0.0f, -0.5f, 0.0f);

			spinePool [spineName] = newSkeletonAnimation;
		} else if (spinePool [spineName] == null) {
			spinePool.Remove (spineName);
			getSpine (spineName);
		}
		return spinePool [spineName];
	}
	**/
	public SkeletonGraphic getSkeletonGraphic(string spineName,SkeletonGraphic demo){
		if (!spinePool.ContainsKey (spineName)) {
			SkeletonDataAsset yourSkeletonDataAsset = Resources.Load<SkeletonDataAsset> ("spine/" + spineName + "/" + spineName + "_SkeletonData");
			SkeletonGraphic skeletonGraphic = (SkeletonGraphic)GameObject.Instantiate (demo,demo.transform.localPosition,demo.transform.localRotation);
			//heroscene.DontDestroyOnLoad(newSkeletonAnimation.transform.gameObject);
			skeletonGraphic.skeletonDataAsset = yourSkeletonDataAsset;
			skeletonGraphic.Initialize (true);
			skeletonGraphic.timeScale = 1;
			skeletonGraphic.unscaledTime = true;
			skeletonGraphic.AnimationState.SetAnimation (0,"stand",true);


			spinePool [spineName] = skeletonGraphic;
		} else if (spinePool [spineName] == null) {
			spinePool.Remove (spineName); 
			getSkeletonGraphic (spineName,demo);
		}
		return spinePool [spineName];
	}
}
