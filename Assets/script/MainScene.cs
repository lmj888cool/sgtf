using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Spine;
using Spine.Unity;
using SimpleJson;
public class MainScene : Observer {
    public UnityEngine.UI.Image bg;

	public HeroStyle skeletonGraphic;
	public Image heroPanel;
	public Text heroName;
	public Text heroLevel;
	public int t = 800;
	public int heroIndex = 0;
	public JsonObject herodata;
	public bool isNeedUpdate = false;
    // Use this for initialization
    void Start () {
        //屏幕适配,按宽度缩放



		skeletonGraphic.Func = new callBackFunc<JsonObject> (changeHero);
		skeletonGraphic.ClickFunc = new callBackFunc<JsonObject> (onClickButton);
		skeletonGraphic.heroStyleBg.gameObject.SetActive (false);
		updateData ();
    }
	public void updateData(){
		isNeedUpdate = true;
	}
	// Update is called once per frame
	void Update () {
		if (t > 500) {
			//t =0;
			ArrayList heroarr = HeroManager.getInstance ().getHerosArrayList ();
			//int heroid = int.Parse (herodata["heroId"].ToString());
			//herodata = HeroManager.getInstance ().getHeros () [heroid];
			int index = heroarr.LastIndexOf (herodata);
			index = index + 1 < heroarr.Count ? index + 1 : 0;
			changeHero(heroarr [index] as JsonObject);

		}
		t++;

	}
	public void fresh(){
		changeHero (herodata);
	}
	void changeHero(JsonObject _herodata){
		t =0;
		skeletonGraphic.init (_herodata);
		herodata = _herodata;
		JsonObject staticdata = HeroManager.getInstance ().getHeroStaticData (herodata);
		JsonObject data = herodata;

		heroName.text = staticdata ["name"].ToString ();
		heroLevel.text = data ["level"].ToString ();
		heroName.color = DataManager.getInstance().getColor(staticdata["color"].ToString());
	}
	public void onClickButton(JsonObject _herodata){

		if (_herodata != null && this.isActiveAndEnabled) {
			BagManager.getInstance ().getGameScene ().onclickBtn (2);
			int heroid = int.Parse (_herodata["heroId"].ToString());
			herodata = HeroManager.getInstance ().getHeros () [heroid];
			BagManager.getInstance ().getGameScene ().heroPanel.OnChangeHero (herodata);
		}


    }
}
