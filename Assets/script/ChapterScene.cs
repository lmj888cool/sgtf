using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TFSG;
using SimpleJson;

public class ChapterScene : MonoBehaviour {

	public UnityEngine.UI.Image bg;

	public Image skill001;
	public Image skill002;
	public Image skill003;
	public Image skill004;
	public Image skillbg;
	public string pathString = "";
	public bool isEditMode = false;
	public string[][] waveArr;
	public ArrayList monsterArr;
	public Text hp_text;
	//通关奖励
	public Text reward1;
	public Text reward2;
	public Text reward3;
	public Text reward4;
	public ArrayList rewardArr;

	private float frontTime;
	private ArrayList hpTextArr;
	public static ChapterScene _chapterScene;
	public Button startBtn;
	public Button pauseBtn;
	public Button backBtn;
	public bool isGameStart;
	public Image pausePanel;
	private float gameTimes;//游戏进行了多长时间

    public Transform content;
	public Image heroHeadDemo;
	public Image dragObject;
    public Rect rect;
	private ArrayList heroHeadList;
	private bool isInDrag;
	public ScrollRect scroll;
	public Button huishouBtn;
	/// <summary>
	/// 胜利或失败界面相关
	/// </summary>
	public Image winPanel;//胜利结算面板
	public Image failPanel;//失败面板
	public RawImage star1;
	public RawImage star2;
	public RawImage star3;
	private ArrayList starArr;

	private bool chapterDong = false;//关卡出怪的点
	public RawImage dong;
	public Monster chapterQiZhi;


	public Text skillDamagesTxt;
	public int skillDamages;
	private int skillDamagesShowTime;
	private Tower clickTower;
	private int delayFinish;
	private float scrollY = 0.0f;
	private bool mouseState = false;
	private List<object> dropitems;
    // Use this for initialization
    void Start () {
		delayFinish = 1;
        _chapterScene = this;
        isGameStart = false;
		isInDrag = false;
		pauseBtn.gameObject.SetActive (false);
		pausePanel.transform.localPosition = new Vector3 (0, 0, 0);
		pausePanel.gameObject.SetActive (false);
		huishouBtn.gameObject.SetActive (false);

		winPanel.gameObject.SetActive (false);
		failPanel.gameObject.SetActive (false);
		skillDamagesTxt.gameObject.SetActive (false);
		starArr = new ArrayList ();
		rewardArr = new ArrayList ();
		starArr.Add (star1);
		starArr.Add (star2);
		starArr.Add (star3);
		rewardArr.Add (reward1);
		rewardArr.Add (reward2);
		rewardArr.Add (reward3);
		rewardArr.Add (reward4);

		SkillManager.getInstance ().setSomeGameObject (skill001,skill002,skill003,skill004,skillbg);
		//设置关卡背景图
		hp_text.gameObject.SetActive(false);
		hpTextArr = new ArrayList();
		heroHeadList = new ArrayList ();
		//ChapterManager.chapterId = 1;

		chapterData chapterInfo = ChapterManager.getInstance ().getChapter ();

		bg.sprite = (Resources.Load (chapterInfo.chapterMap,typeof(Sprite)) as Sprite);
		//bg.SetNativeSize ();

		//屏幕适配,按宽度缩放
		float retio = (float)(Screen.width) / (float)(bg.sprite.rect.width);
		float retioBg = (float)(Screen.height) / (float)(bg.sprite.rect.height);
		if (retio < retioBg)
		{
			bg.transform.localScale = new Vector3(retio, retio, 0);
		}
		else
		{
			bg.transform.localScale = new Vector3(retioBg, retioBg, 0);
		}

		//UGUIEventTrigger.Get (bg.gameObject).AddEventListener (EventTriggerType.PointerDown,OnClick);
		//UGUIEventTrigger.Get (bg.gameObject).AddEventListener (EventTriggerType.PointerUp,OnClickUp);
		//UGUIEventTrigger.Get (gameObject).AddEventListener (EventTriggerType.PointerUp,OnClickUp);
		//UGUIEventTrigger.Get (bg.gameObject).AddEventListener (EventTriggerType.Move,OnMove);
		MonsterManager.getInstance ().initMonsterData (chapterInfo.chapterMonster);

		monsterArr = MonsterManager.getInstance ().getMonstersByBoShu (1,bg.transform);
		frontTime = Time.time;
        //maskBg.gameObject.SetActive (false);
   
        TowerManager.getInstance().initChapterTower(chapterInfo.chapterTower);

		Dictionary<int,JsonObject> heroarr = HeroManager.getInstance().getHeros();
        ArrayList towerarr = TowerManager.getInstance().getTowersList();
		foreach(KeyValuePair<int,JsonObject> kvp in heroarr)
        {
			JsonObject hd = kvp.Value;
			string heroId = hd ["heroId"].ToString ();
            bool isInFight = false;
            for (int j = 0; j < towerarr.Count; j++)
            {
                Tower tower = (Tower)towerarr[j];
				string towerHeroId = tower.hd["heroId"].ToString ();
				if (tower.hd != null && towerHeroId == heroId)
                {
                    isInFight = true;
                    break;
                }
            }
            if (!isInFight)
            {
                addHeroHead(hd);
            }
                
        }

		for (int i=0; i < monsterArr.Count; i++) {
			ArrayList arr = (ArrayList)monsterArr[i];
			if(arr.Count > 1) {
					Monster _monster = (Monster)arr [arr.Count-1];
				if (!chapterDong) {
					dong.transform.localPosition = _monster.transform.localPosition;
					dong.transform.SetSiblingIndex (0);
					chapterDong = true;

					chapterQiZhi = MonsterManager.getInstance().getMonster (true);
					//_monster.transform.SetParent(_parent);
					chapterQiZhi.transform.localScale = new Vector3(1,1,1);
					chapterQiZhi.gameObject.SetActive (true);
					string[] str = new string[5];
					str[1] = "1000";
					str [3] = "";
					chapterQiZhi.init (str, 0);
					//_monster.waveArr[_monster.waveArr.Length - 1];
					Vector3 _p= new Vector3 (0, 0, 0);
					_p = (Vector3)_monster.pathArr[_monster.pathArr.Count - 1];
					//_p.y = int.Parse(_monster.pathArr[_monster.pathArr.Count - 1][1]);

					chapterQiZhi.transform.SetParent (bg.transform);
					chapterQiZhi.transform.localPosition = _p;
					chapterQiZhi.transform.SetSiblingIndex (0);

				} else {
					GameObject obj = ChapterManager.getInstance ().clone (dong.gameObject, dong.transform.parent);
					obj.transform.localPosition = _monster.transform.localPosition;
					obj.transform.SetSiblingIndex (0);
				}
					
			}

		}

		scrollY = scroll.transform.position.y + scroll.gameObject.GetComponent<RectTransform> ().rect.height /2;
    }
	public void changeSkillDamages(int damage){
		skillDamages += damage;
		skillDamagesTxt.text = "伤害" + skillDamages.ToString ();
		if(!skillDamagesTxt.isActiveAndEnabled){
			skillDamagesTxt.gameObject.SetActive (true);
			skillDamagesTxt.transform.SetSiblingIndex (1000);
		}
		skillDamagesShowTime = 60;
	}
	public void showWinPanel(){
		Time.timeScale = 0;
		winPanel.transform.localPosition = new Vector3(0,0,0);
		winPanel.transform.SetSiblingIndex (1100);
		winPanel.gameObject.SetActive (true);
		int loveNum = ChapterManager.getInstance ().getStar ();

		for(int i = 0;i < loveNum;i++){
			RawImage star = (RawImage)starArr [i];
			Destroy (star);
		}
		ArrayList arr = TowerManager.getInstance().getTowersList();
		for (int i = 0; i < arr.Count; i++) {
			Tower tower = (Tower)arr [i];
			if (tower.isInit) {
				tower.heroStyle.AnimationState.SetAnimation (0,"celebrate",true).timeScale = 1;
				tower.isInit = false;
			}
			//iTween.MoveBy(tower.body.gameObject, iTween.Hash("y", 15, "easeType", iTween.EaseType.linear, "loopType", "pingPong", "delay", .1));
		}
		for (int i = 0; i < rewardArr.Count; i++) {
			Text txt = (Text)rewardArr[i];
			txt.text = "";
		}
		isGameStart = false;
		//iTween.Stop ();

		JsonObject userMessage = new JsonObject();
		userMessage.Add ("chapterId", ChapterManager.getInstance().getChapter().chapterId);
		userMessage.Add ("chapterStar", ChapterManager.getInstance().getStar());
		userMessage.Add ("chapterType", ChapterManager.getInstance().chapterType);
		//if (LoginScene.pclient != null) {
		ServerManager.getInstance ().request("area.playerHandler.upgradeChapter", userMessage, (data)=>{
			Debug.Log(data.ToString());
			DataManager.playerData ["chapter"] = data["chapter"];
			dropitems = data["dropItems"] as List<object>;


			
		});
		//}

	}
	public void showFailPanel(){
		Time.timeScale = 0;
		failPanel.transform.localPosition = new Vector3(0,0,0);
		failPanel.transform.SetSiblingIndex (1100);
		failPanel.gameObject.SetActive (true);
		ArrayList arr = TowerManager.getInstance().getTowersList();
		for (int i = 0; i < arr.Count; i++) {
			Tower tower = (Tower)arr [i];
			if (tower.isInit) {
				tower.heroStyle.AnimationState.SetAnimation (0,"dizzy",true).timeScale = 1;
				tower.isInit = false;
			}
			//iTween.MoveBy(tower.body.gameObject, iTween.Hash("y", 15, "easeType", iTween.EaseType.linear, "loopType", "pingPong", "delay", .1));
		}
		isGameStart = false;
		//iTween.Stop ();
	}
	public void showHuiShou(bool b){
		huishouBtn.gameObject.SetActive (b);
		backBtn.gameObject.SetActive (!b);
	}
    public void addHeroHead(JsonObject herodata)
    {
			string heroId = herodata ["heroId"].ToString();
			string heroName = HeroManager.getInstance ().getHeroStaticData (herodata)["name"].ToString ();
				
		for (int i = 0; i < heroHeadList.Count; i++)
		{
			Image btn2 = (Image)heroHeadList [i];

			if (btn2.name == heroId) {
				btn2.gameObject.SetActive (true);
				btn2.transform.SetParent (content);
				return;
			}
		}
		Image btn;
		heroHeadDemo.gameObject.SetActive (true);

        if (content.childCount == 0)
        {
			heroHeadDemo.transform.FindChild("Text").GetComponent<Text>().text = heroName;
			//heroHeadDemo.GetComponent<Image>().sprite = (Resources.Load ("heroIcon/204", typeof(Sprite)) as Sprite);
            heroHeadDemo.transform.SetParent(content);
            btn = heroHeadDemo;
            //OnChangeHero(herodata, heroHeadDemo);
        }
        else
        {
			btn = (Image)GameObject.Instantiate(heroHeadDemo, heroHeadDemo.transform.position, heroHeadDemo.transform.rotation);
            //btn.interactable = true;
			btn.transform.FindChild("Text").GetComponent<Text>().text = heroName;

            btn.transform.SetParent (content);
        }
		btn.name = heroId;
        heroHeadList.Add(btn);
        //btn.onClick.AddListener(delegate () {
            //this.OnChangeHero(herodata, btn);
			//Debug.Log ("asfdasfa");

        //});
        //UGUIEventTrigger.Get(btn.gameObject).AddEventListener(EventTriggerType.PointerDown, OnHeadClickDown);
		//UGUIEventTrigger.Get(btn.gameObject).AddEventListener(EventTriggerType.EndDrag, OnClickUp);
        //UGUIEventTrigger.Get(btn.gameObject).AddEventListener(EventTriggerType.PointerExit, OnMove);

    }
	public void OnClickDown(){
		Debug.Log ("asfdasfa");
	}
    public void OnMove(BaseEventData eventData)
    {
        if (!isGameStart)
        {
            PointerEventData _PointerEventData = (PointerEventData)eventData;
            if (dragObject != null)
            {
                dragObject.transform.position = Input.mousePosition;
            }
        }

    }
	public void OnHeadClickDown(BaseEventData eventData)
    {
        if (!isGameStart)
        {
            PointerEventData _PointerEventData = (PointerEventData)eventData;
            //dragObject = eventData.selectedObject;
            //dragObject.transform.position = Input.mousePosition;
            //dragObject.transform.SetSiblingIndex(1200);
        }

    }
    public void OnHeadClickDownUp(BaseEventData eventData)
    {
        if (!isGameStart)
        {
            PointerEventData _PointerEventData = (PointerEventData)eventData;
            if (dragObject != null)
            {
                //dragObject.transform.SetParent(content);
                dragObject = null;
            }
            
        }

    }
    public void onClickStart(){
		Time.timeScale = 1;
		isGameStart = true;
		pauseBtn.gameObject.SetActive (true);
		startBtn.gameObject.SetActive (false);
		backBtn.gameObject.SetActive (false);
        content.parent.parent.gameObject.SetActive(false);
		ArrayList arr = TowerManager.getInstance().getTowersList();
		for (int i = 0; i < arr.Count; i++) {
			Tower tower = (Tower)arr [i];
			//iTween.MoveBy(tower.body.gameObject, iTween.Hash("y", 15, "easeType", iTween.EaseType.linear, "loopType", "pingPong", "delay", .1));
		}
		showWinPanel();//测试

    }

	public void onClickPause(int type){
		if (type == 0) {
			isGameStart = false;
			Time.timeScale = 0;
			pausePanel.gameObject.SetActive (true);
			pausePanel.transform.SetSiblingIndex (1000);
		} else if (type == 1) {
			pausePanel.gameObject.SetActive (false);
			onClickStart ();
		} else if (type == 2) {//胜利后继续下一关
			//Time.timeScale = 1;
			ChapterManager.getInstance().GotoNextChapterScene();
		} else if (type == 3 || type == 4) {
			SceneManager.LoadScene ("GameScene");
		}else if(type == 5){//重来
			ChapterManager.getInstance().GotoChapterScene();
		}

		
	}
	void FixedUpdate(){
		if (isGameStart) {
			gameTimes += Time.fixedDeltaTime;
			if (Time.time - frontTime > 1) {
				bool haveMonster = false;
				frontTime = Time.time;
				for (int i=0; i < monsterArr.Count; i++) {
					ArrayList arr = (ArrayList)monsterArr[i];
					if(arr.Count > 1) {
						haveMonster = true;
						float mosterStartTime = (float)arr[0];
						if(mosterStartTime < gameTimes){
							Monster _monster = (Monster)arr [arr.Count-1];
							arr.RemoveAt (arr.Count -1);
							_monster.gameObject.SetActive (true);

							//_monster.transform.SetSiblingIndex (1);
							_monster.startWalk ();
							MonsterManager.getInstance ().addMonster (_monster);
						}

					}

				}

				if (!haveMonster && MonsterManager.getInstance ().activeMonsterArr.Count == 0) {//闯关成功!!!!
					if (delayFinish <= 0) {
						
						showWinPanel ();
					}
					delayFinish--;

				} else {
					if(ChapterManager.getInstance().loveNum <= 0){//闯关成功!!!!
						if (delayFinish <= 0) {
							showFailPanel();
						}
						delayFinish--;

					}
				}

			}
			if (SkillManager._skillManager != null) {
				SkillManager._skillManager.updateByScene();
			}

			for (int i = 0; i < hpTextArr.Count;i++) {
				Text _txt = (Text)hpTextArr [i];
				int count =(int) hpTextArr [i + 1];
				if (count > 0) {
					_txt.transform.Translate (0, 2, 0);
					hpTextArr [i + 1] = count - 1;
				} else {
					hpTextArr.RemoveAt (i);
					i--;
					Destroy (_txt);
					hpTextArr.RemoveAt (i + 1);
					i--;
				}
				i++;
				if (i < -1)
					i = -1;
			}
			//总伤害显示处理
			if (skillDamagesShowTime > 0) {
				skillDamagesShowTime--;
			} else {
				if(skillDamagesTxt.isActiveAndEnabled){
					skillDamagesTxt.gameObject.SetActive (false);
					skillDamages = 0;
				}
			}

        }
        {
			
        }


	}
	public void playJumpHp(int hp,Vector3 p){//播放掉血
		Text txt = (Text)GameObject.Instantiate(hp_text,hp_text.transform.position,hp_text.transform.rotation);

		txt.text = hp.ToString ();
		txt.transform.SetParent (bg.transform);
		txt.transform.localScale = new Vector3(1,1,1);
		txt.transform.position = p;
		txt.gameObject.SetActive(true);
		hpTextArr.Add (txt);
		hpTextArr.Add (30);
	}
	// Update is called once per frame
	void Update () {
		if (isGameStart) {
			SkillManager.getInstance ().Update ();
		}
		bool b2 = Input.GetMouseButton (0);
		//if (Time.timeScale != 0) {
			if (b2) {
				if (!isGameStart) {
					if (dragObject != null) {
						if (isInDrag) {
							dragObject.transform.position = Input.mousePosition;
						} else {
							//Vector3 _p2 = scroll.transform.InverseTransformPoint (Input.mousePosition);
							//bool b = scroll.gameObject.GetComponent<RectTransform> ().rect.Contains (_p2);
							if (Input.mousePosition.y > scrollY) {
								isInDrag = true;
								dragObject.transform.SetParent (bg.transform);
								//iTween.MoveBy(scroll.gameObject, iTween.Hash("y", 80, "easeType", "easeInOutExpo", "loopType", "none", "delay", .1));
							}

						}
					} 
					if (clickTower != null) {
						clickTower.body.transform.position = Input.mousePosition;
					} 
				}
				if (!mouseState) {
					mouseState = true;
					OnClick (null);
				}

			} else {
				if (mouseState) {
					mouseState = false;
					OnClickUp (null);
				}

			}
		//}

		if (dropitems != null) {
			for (int i = 0; i < dropitems.Count; i++) {
				JsonObject jo = dropitems[i] as JsonObject;
				JsonObject staticdata = DataManager.getInstance().dataDic[jo["type"].ToString()][int.Parse(jo["id"].ToString())];
				Text txt = (Text)rewardArr[i];
				txt.text = staticdata["name"].ToString() + " x" + jo["count"].ToString();
				//txt.transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
				//iTween.ScaleTo(txt.gameObject, iTween.Hash("y", 1.0f,"x", 1.0f,"z", 1.0f ,"delay", 0.0f,"time",0.5f));
				//iTween.MoveFrom(txt.gameObject, new Vector3(-500.0f,txt.transform.localPosition.y,txt.transform.localPosition.z),0.5f);

			}

			dropitems = null;
		}
	}
	private Vector3 _pTemp;
	public void OnClick(BaseEventData eventData){
		//PointerEventData _PointerEventData = (PointerEventData)eventData;

		if (isEditMode) {
			Vector3 _p = bg.transform.InverseTransformPoint (Input.mousePosition);
			//float x = _PointerEventData.position.x - (Screen.width) / 2;
			//float y = _PointerEventData.position.y - (Screen.height) / 2;

			int vc = 30;
			int dir = 1;
			if (_pTemp != null) {
				if (_pTemp.x - _p.x > vc) {
					dir = 2;
				}
				if (_pTemp.x - _p.x < -vc) {
					dir = 3;
				}
				if (_pTemp.y - _p.y > vc) {
					dir = 1;
				}
				if (_pTemp.y - _p.y < -vc) {
					dir = 4;
				}
			}
			pathString += (int)_p.x + "," + (int)_p.y + "," + dir.ToString () + "\r\n";
			
			_pTemp = _p;

		} else {
			if (!isGameStart) {
				for (int i = 0; i < heroHeadList.Count; i++) {
					Image btn = (Image)heroHeadList [i];
					if (btn.isActiveAndEnabled) {
						Vector3 _p2 = btn.transform.InverseTransformPoint (Input.mousePosition);
						bool b = btn.rectTransform.rect.Contains (_p2);
						if (b) {
							dragObject = btn;
							//dragObject.transform.SetParent (bg.transform);
							break;
						}
					}

				}
				//拖拽英雄进行部署

			} 
			//{
				ArrayList arr = TowerManager.getInstance().getTowersList();
				for (int i = 0; i < arr.Count; i++) {
					Tower tower = (Tower)arr [i];
					if (tower.isInit) {//初始化的塔才可以被拖拽
						Vector3 _p3 = tower.heroStyle.transform.InverseTransformPoint (Input.mousePosition);
						bool b = tower.heroStyle.rectTransform.rect.Contains (_p3);
						//Debug.Log ("OnClick:" + _p3.ToString());
						if (b) {
							clickTower = tower;
							Debug.Log ("OnClickOk:" + _p3.ToString());
							tower.OnClickDown (null);
						}
					}
					
					//iTween.MoveBy(tower.body.gameObject, iTween.Hash("y", 15, "easeType", iTween.EaseType.linear, "loopType", "pingPong", "delay", .1));
				}
			//}


		}
		Debug.Log ("OnClick");
		//ArrayList heroarr = HeroManager.getInstance().getHeros();




	}
	public void OnClickUp(BaseEventData eventData){
		Debug.Log ("OnClickUp");
		//_pTemp = ;
		//PointerEventData _PointerEventData = (PointerEventData)eventData;
		//Vector3 _p = bg.transform.InverseTransformPoint (_PointerEventData.position);
		if (clickTower != null) {
			clickTower.OnEndDrag (null);
			clickTower = null;
		}
		if (dragObject != null) {
			if (isInDrag) {
				ArrayList arr = TowerManager.getInstance().getTowersList();
				bool isChange = false;
				for(int i=0;i < arr.Count; i++)
				{
					Tower tower = (Tower)arr[i];
					//if (tower.hd != null)
					//{
						//Image img = tower.gameObject.GetComponent<Image>();
						float _w = Mathf.Abs(dragObject.transform.position.x - tower.transform.position.x);
						float _h = Mathf.Abs(dragObject.transform.position.y - tower.transform.position.y);

						if (_w < 80)
						{
							if (_h < 80)
							{
								//Vector3 _p = tower.body.transform.position;
								//transform.position = tower.transform.position;
								//tower.transform.SetParent (content);
							JsonObject hd = HeroManager.getInstance().getHeroById(int.Parse(dragObject.name));
								
							if (tower.isInit) {
								addHeroHead (tower.hd);
							}
							dragObject.transform.SetParent (null);
							dragObject.gameObject.SetActive (false);
							tower.initTower(hd);
								
							//dragObject.transform.position = _p;
							isChange = true;
								break;
							}

						}
					//}
				}
				if (!isChange)
				{
					dragObject.transform.SetParent (content);


				}

			}
			isInDrag = false;
			dragObject = null;
			//dragObject = null;

		}



	}
	public void OnPointerEnter(BaseEventData eventData){
		Debug.Log ("OnPointerEnter");
	}
	public void startEditPath(){
		pathString = "";
		isEditMode = true;
		scroll.gameObject.SetActive (false);
		startBtn.gameObject.SetActive (false);
	}
	public void endEditPath(){
		isEditMode = false;
		Debug.Log (pathString);
		scroll.gameObject.SetActive (true);
		startBtn.gameObject.SetActive (true);
	}
}
