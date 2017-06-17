using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJson;
public class SceletScene : MonoBehaviour {
    //public UnityEngine.UI.Image bg;
    // Use this for initialization
	public Transform content;
	//public Transform content_chapter;
	public ChapterItem chapterItem;
	public CampaignItem campaignItem;
	public static SceletScene _sceletScene;
	public Button frontBtn;
	public Button nextBtn;
	public int chapterId = 0;
	//private ArrayList cacheArray;
	//private ArrayList actvieArray;
	void Awake(){
		//this.enabled = false;
	}
    void Start () {
		Debug.Log("进入关卡界面");
		//cacheArray = new ArrayList ();
		//actvieArray = new ArrayList ();
		//cacheArray.Add (chapterItem);
		//onclickBtn (0);
		_sceletScene = this;
		UpdateList("1");
		//foreach(KeyValuePair<int,chapterData> kvp in DataManager.getInstance().chapterDic){
		//	addChapter (kvp.Value);
		//}
    }
	public void UpdateList (string _chapterId) {
		chapterId = int.Parse (_chapterId);
		MonoBehaviour[] actvieArray2 = content.transform.GetComponentsInChildren<CampaignItem> ();
		for(int i = 0; i < actvieArray2.Length ; i++){
			MonoBehaviour panel = actvieArray2 [i];
			//panel.transform.SetParent (null);
			PoolManager.getInstance ().addToPool (PoolManager.CAMPAIGN_ITEM,panel);
		}
		chapterItem.init(DataManager.getInstance ().chapterDicJson [chapterId]);
		float delay = 0.0f;
		foreach (KeyValuePair<int,JsonObject> kvp in DataManager.getInstance().campaignDicJson) {
			JsonObject jo = kvp.Value;
			if (int.Parse (jo ["id"].ToString ()) > 0) {
				if (jo ["chapter"].ToString () == _chapterId) {
					addChapter (kvp.Value,delay);
					delay += 0.05f;
				}
			}
		}

		/**if(DataManager.playerData.ContainsKey("chapter")){
			
			//actvieArray.Clear ();
			JsonArray PassChapterArray = (DataManager.playerData ["chapter"]) as JsonArray;
			//Debug.Log (heroStr);
			//heroStr = heroStr.Substring (1,heroStr.Length -2);
			//string[] heroArr = heroStr.Split (",".ToCharArray (), StringSplitOptions.RemoveEmptyEntries);
			JsonObject PassChapterList = PassChapterArray[chapterType] as JsonObject;
			foreach(KeyValuePair<string,object> kvp in PassChapterList){
				chapterData cd = DataManager.getInstance ().chapterDic [int.Parse (kvp.Key)];
				cd.star = int.Parse(kvp.Value.ToString());
				addChapter (cd);
			}
			int nextChapterId = PassChapterList.Count + 1;
			if (DataManager.getInstance ().chapterDic.ContainsKey (nextChapterId)) {

				chapterData cdNext = DataManager.getInstance ().chapterDic [PassChapterList.Count + 1];
				cdNext.star = 0;
				addChapter (cdNext);
			}


		}**/
	}
	// Update is called once per frame
	void Update () {
		
	}
    public void onClickBack()
    {
        SceneManager.LoadScene("MainScene");

    }
	void showPanel(int chapterType,Button btn){
		//if (btn.interactable)
		{
			//putongTab.interactable = true;
			//jingyingTab.interactable = true;
			//UpdateList ("chapter",0);

			//btn.interactable = false;
		}

	}
	public void onclickBtn(int type){
		//chapterType = type;
		//UpdateList ();
		switch (type) {
		case 1:
			//showPanel (1,putongTab);
			if (chapterId > 1) {
				chapterId = chapterId - 1 < 1 ? 1 : chapterId - 1;
				UpdateList (chapterId.ToString());
			}

			break;
		case 2:
			int count = DataManager.getInstance ().chapterDicJson.Count;
			if (chapterId + 1 < count) {
				chapterId = chapterId + 1 >= count ? chapterId:chapterId + 1;
				UpdateList (chapterId.ToString());
			}

			//showPanel (2,jingyingTab);
			break;
		default:
			break;
		}
	}
	public void onClickBaoXiang(int type){
	}
	public void getCampaignItem(){
	}
	public void addChapter(JsonObject jo,float delay){
		MonoBehaviour panel = (MonoBehaviour)PoolManager.getInstance ().getGameObject (PoolManager.CAMPAIGN_ITEM);


		//Vector3 v3 = panel.transform.localPosition;
		//panel.transform.localPosition = new Vector3 (-800,v3.y,v3.z);
		panel.transform.SetParent (content);
		((CampaignItem)panel).init (jo,chapterId);
		panel.transform.localScale = new Vector3 (1.0f, 1.0f,1.0f);
		//iTween.ScaleTo(panel.gameObject, iTween.Hash("y", 1.0f,"x", 1.0f,"z", 1.0f ,"delay", delay,"time",0.5f));

	}
	public void OnChangeHero(chapterData cd,Button btn){
		//heroStyle.sprite = Resources.Load(herodata.heroStyle,typeof(Sprite)) as Sprite;
		//heroStyle.SetNativeSize ();
		//heroBB.text = herodata.heroDescription;
		//for (int i = 0; i < heroHeadList.Count; i++) {
		//	Button btn2 = (Button)heroHeadList[i];
		//	btn2.interactable = true;
		//}

		//btn.interactable = false;

		//技能
		//skillData skilldata = DataManager.getInstance().skillDic[herodata.heroSkill];
		//skillName.text = skilldata.skillName;
		//skillIcon.sprite = Resources.Load(skilldata.skillIcon,typeof(Sprite)) as Sprite;
		//skillIcon.SetNativeSize ();
		/**JsonObject userMessage = new JsonObject();
		userMessage.Add ("chapterId", cd.chapterId);
		userMessage.Add ("chapterStar", 3);
		//if (LoginScene.pclient != null) {
		ServerManager.getInstance ().request("area.playerHandler.upgradeChapter", userMessage, (data)=>{
			Debug.Log(data.ToString());
		});**/
		//ChapterManager.getInstance ().chapterType = chapterType;
		ChapterManager.getInstance().setChapterId(cd.chapterId);
        ChapterManager.getInstance().GotoChapterScene();
        

	}
}
