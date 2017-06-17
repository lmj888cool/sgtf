using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;

public class ChapterItem : MonoBehaviour {
	private JsonObject cd;
	public Text chapterName;
	private int chapterId;
	// Use this for initialization
	void Awake(){
		//PoolManager.getInstance ().initPoolByType (PoolManager.CHAPTER_ITEM.ToString(),this,5);
	}
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void init(JsonObject _cd){
		
		this.cd = _cd;
		chapterName.text = cd["name"].ToString();

	}
	public void onClick(){
		SceletScene._sceletScene.UpdateList (this.cd["id"].ToString());
		/**JsonObject userMessage = new JsonObject();
		userMessage.Add ("chapterId", cd.chapterId);
		userMessage.Add ("chapterStar", 3);
		//if (LoginScene.pclient != null) {
			ServerManager.getInstance ().request ("area.playerHandler.upgradeChapter", userMessage, (data) => {
				Debug.Log (data.ToString ());
			});
		//}**/
		//ChapterManager.getInstance ().chapterType = chapterType;
		//ChapterManager.getInstance().setChapterId(cd.chapterId);
		//ChapterManager.getInstance().GotoChapterScene();
	}
}
