using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;

public class CampaignItem : MonoBehaviour {
	public RawImage star1;
	public RawImage star2;
	public RawImage star3;
	private ArrayList starArr;
	private JsonObject cd;
	public Text chapterName;
	public Button tiaozhanBtn;
	public Image dropPanel;
	private int chapterId;
	private List<object> dropitems;
	// Use this for initialization
	void Awake(){
		PoolManager.getInstance ().initPoolByType (PoolManager.CAMPAIGN_ITEM.ToString(),this,5);
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (dropitems != null) {
			dropitems = null;
		}
		
	}
	public void init(JsonObject _cd,int _chapterId){
		chapterId = _chapterId;
		this.cd = _cd;
		//chapterType = _chapterType;
		chapterName.text = cd["name"].ToString();

		((Button)this.GetComponent<Button>()).interactable = true;
		//for(int i = 0;i < cd.star;i++){
		//	RawImage star = (RawImage)starArr [i];
		//	star.gameObject.SetActive (false);
		//	//Destroy (star);
		//}

		string dropstr = cd["drop"].ToString();
		showDropItem(dropstr.Split ('|'));
	}
	public void showDropItem(string[] dropArr){
		//Rect rect = this.GetComponent<Image> ().rectTransform.rect;
		float xoffset = 40;
		IconBase[] dropIconArray = dropPanel.transform.GetComponentsInChildren<IconBase> ();
		int num = dropIconArray.Length;
		int index = 0;
		for (int j = index; j < dropIconArray.Length; j++) { 
			IconBase icon = dropIconArray [j];
			if (icon != null) {
				PoolManager.getInstance ().addToPool (icon.type,icon);
			}
		}
		for (int i = 0; i < dropArr.Length; i++) {  
			string[] itemInfo = dropArr [i].Split ('_');
			int itemId = int.Parse (itemInfo[0]);
			JsonObject jo = DataManager.getInstance().getItemDataById(itemId);
			IconBase icon = (IconBase)PoolManager.getInstance ().getGameObject (jo["color"].ToString());
			/**if (num > i) {
				icon = dropIconArray [i];
				index++;
			} else {
				icon = (IconBase)PoolManager.getInstance ().getGameObject (jo["color"].ToString());
			}**/
			index++;
			icon.init (jo).Func = new callBackFunc<JsonObject>(onClickDropItem);
			icon.transform.SetParent (dropPanel.transform);
			icon.transform.localScale = new Vector3 (0.5f,0.5f,0.5f);
			icon.transform.localPosition = new Vector3 (xoffset,0,0);
			xoffset += 55;
		} 


		
	}
	public void onClickDropItem(JsonObject jo){
		
	}
	public void onTiaoZhan(){
		JsonObject userMessage = new JsonObject();
		userMessage.Add ("chapterId", chapterId);
		userMessage.Add ("campaignId", cd["id"]);
		//userMessage.Add ("chapterStar", 3);
		//if (LoginScene.pclient != null) {
			ServerManager.getInstance ().request ("area.playerHandler.upgradeChapter", userMessage, (data) => {
				Debug.Log (data.ToString ());
				DataManager.playerData ["chapter"] = data["chapter"];
			dropitems = data["dropItems"] as List<object>;
			});
		//}
		//ChapterManager.getInstance ().chapterType = chapterType;
		//ChapterManager.getInstance().setChapterId(cd.chapterId);
		//ChapterManager.getInstance().GotoChapterScene();
	}
}
