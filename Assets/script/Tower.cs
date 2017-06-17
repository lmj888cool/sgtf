using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TFSG;
using SimpleJson;
using Spine;
using Spine.Unity;
public class Tower : MonoBehaviour {

	public RawImage MP;
	public RawImage MPbg;
	public Text skill;
	public float currentMP = 0.0f;
	public int attackDamage = 0;//塔的攻击力
	//private string towerName = "";
	public int attackRange = 200;//攻击范围
	private float attackPingLv = 1.0f;//攻击平率
	private float frontTime = 0.0f;
	public int skillId;
	private ArrayList canAttackMonsterArr;
    private Vector3 sourceP;
    private Vector3 sourceP2;
    public Text _txt;
	public SkeletonGraphic heroStyle;
    public Image body;
    public Image range;
    public JsonObject hd;
	public bool isInit = false;
	public AudioSource attackMusic;
	public int moveYsetoff = 0;
	public bool spineFlipX = false;
	public bool isCanPlayMP = false;
    // Use this for initialization
    void Awake()
    {
		body.gameObject.SetActive (false);
        heroStyle.gameObject.SetActive(false);
        range.gameObject.SetActive(false);
        TowerManager.getInstance().addTower(this);


    }
	void Start () {
		//UGUIEventTrigger.Get(heroStyle.gameObject).AddEventListener(EventTriggerType.PointerDown, OnClickDown);
		//UGUIEventTrigger.Get(heroStyle.gameObject).AddEventListener(EventTriggerType.PointerUp, OnClickDownUp);
		//UGUIEventTrigger.Get(heroStyle.gameObject).AddEventListener(EventTriggerType.Drag, OnDrag);
		//UGUIEventTrigger.Get(heroStyle.gameObject).AddEventListener(EventTriggerType.EndDrag, OnEndDrag);

        //this.GetComponent<UnityEngine.UI.Image>().sprite = (Resources.Load ("hero/08",typeof(Sprite)) as Sprite);
        //this.GetComponent<UnityEngine.UI.Image> ().SetNativeSize ();
        
		//frontTime = Time.time;
		//attackRange = 200;
		//attackPingLv = 1.0f;
		//attackDamage = 10;
        //skillId = 101;
        //_txt.text = transform.position.ToString();
        
    }
	public void playScale(){
		if (isInit) {
			heroStyle.Skeleton.flipX = spineFlipX;
			heroStyle.transform.localScale = new Vector3 (0.0f, 0.0f, 0.0f);
			iTween.ScaleTo(heroStyle.gameObject, iTween.Hash("y", 0.4f,"x", 0.4f,"z", 0.4f ,"delay", 0.0f,"time",0.5f));
		}

	}
    public void initTower(JsonObject _hd)
    {
		if (heroStyle != null) {
			heroStyle.gameObject.SetActive(false);
			heroStyle.transform.SetParent (null);
		}
		if(_hd == null){
			if (hd != null)
				//hd.isInFight = false;
			hd = null;
			body.gameObject.SetActive (false);
			range.gameObject.SetActive(false);
			isInit = false;
			return;
		}
		isInit = true; 
		skill.gameObject.SetActive (false);
		MP.transform.localScale = new Vector3 (0,1,1);
		body.gameObject.SetActive (true);
        hd = _hd;
		//hd.isInFight = true;
		JsonObject staticdata = HeroManager.getInstance ().getHeroStaticData (hd);
		JsonObject data = hd;

        
		heroStyle = HeroManager.getInstance().getSkeletonGraphic(staticdata["style"].ToString(),heroStyle);
		heroStyle.gameObject.SetActive(true);
		heroStyle.transform.SetParent(this.body.transform);
		heroStyle.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
		heroStyle.AnimationState.SetAnimation (0,"stand",true);
		heroStyle.Skeleton.flipX = spineFlipX;
		playScale ();
		//heroStyle.timeScale = 10;
        //heroStyle.SetNativeSize ();
		attackRange = int.Parse(data["attackRange"].ToString());
		attackPingLv = 1.0f/float.Parse(data["attackSpeed"].ToString());
		attackDamage = int.Parse(data["attack"].ToString());
		skillId = int.Parse(data["skillId"].ToString());
        float retio = attackRange * 2 / range.rectTransform.rect.width;
        range.transform.localScale = new Vector3(retio, retio, 0);




    }
	public void changMp(float mp){//魔法
		//music.Play ();
		currentMP += mp;
		//int damage = beforChangeHP- currentHP;//用于显示伤害数字
		//ChapterScene._chapterScene.playJumpHp(damage,new Vector3(transform.position.x,transform.position.y + 40,transform.position.z));

		//beforChangeHP = currentHP;
		currentMP = currentMP > 100.0f ? 100.0f : currentMP;
		float xscale = (float)currentMP / 100.0f;
		MP.transform.localScale = new Vector3 (xscale,1,1);
		if (currentMP >= 100.0F) {
			//onDead ();
			isCanPlayMP = true;
			skill.gameObject.SetActive (!skill.isActiveAndEnabled);
		}

	}
    public void OnDrag(BaseEventData eventData)
    {
        if (!ChapterScene._chapterScene.isGameStart)
        {
            //PointerEventData _PointerEventData = (PointerEventData)eventData;
            body.transform.position = Input.mousePosition;
            //_txt.text = transform.position.ToString();
        }

    }
    public void OnEndDrag(BaseEventData eventData)
    {
        if (!ChapterScene._chapterScene.isGameStart)
        {
            
            //PointerEventData _PointerEventData = (PointerEventData)eventData;
            
            ArrayList arr = TowerManager.getInstance().getTowersList();
            bool isChange = false;
            for(int i=0;i < arr.Count; i++)
            {
                Tower tower = (Tower)arr[i];
				if (tower != this)
                {
                    //Image img = tower.gameObject.GetComponent<Image>();
					float _w = Mathf.Abs(body.transform.position.x - tower.body.transform.position.x);
					float _h = Mathf.Abs(body.transform.position.y - tower.body.transform.position.y);

                    if (_w < 60)
                    {
                        if (_h < 80)
                        {
							//heroData hd_temp = hd;
							//if (tower.hd != null) {
							//	initTower (tower.hd);
							//} else {
								//initTower (null);
							//}
							//tower.initTower (hd_temp);

							Vector3 _p = transform.position;
                            transform.position = tower.transform.position;
                            tower.transform.position = _p;
							bool flipX = this.spineFlipX;
							//this.heroStyle.Skeleton.flipX = tower.spineFlipX;
							spineFlipX = tower.spineFlipX;
							//tower.heroStyle.Skeleton.flipX = flipX;
							tower.spineFlipX = flipX;
							tower.playScale ();
							playScale ();
                            //body.transform.position = _p;
							//body.transform.localPosition = sourceP2;
                            isChange = true;
                            break;
                        }
                        
                    }
                }
            }
            if (!isChange)
            {
				//判断是否回收
				Button btn = ChapterScene._chapterScene.huishouBtn;
				float _w = Mathf.Abs(body.transform.position.x - btn.transform.position.x);
				float _h = Mathf.Abs(body.transform.position.y - btn.transform.position.y);

				if (_w < 80)
				{
					if (_h < 80)
					{
						
						ChapterScene._chapterScene.addHeroHead (hd);
						initTower (null);
					}

				}
                
            }
			body.transform.localPosition = new Vector3(0,0,0);
        }
		OnClickDownUp (null);

    }
    public void OnClickDown(BaseEventData eventData){
        //sourceP = transform.position;
		//sourceP2 = body.transform.localPosition;
        
        
        if (ChapterScene._chapterScene.isGameStart)
        {
            //PointerEventData _PointerEventData = (PointerEventData)eventData;
			if (isCanPlayMP) {
				SkillManager._skillManager.PlaySkill(this, skillId);
				skill.gameObject.SetActive (false);
				isCanPlayMP = false;
				currentMP = 0.0f;
				changMp (0.0f);
			}
            
            
        }else
        {
            //transform.SetSiblingIndex(1100);
			range.gameObject.SetActive(true);
            ChapterScene._chapterScene.content.parent.parent.gameObject.SetActive(false);
			ChapterScene._chapterScene.showHuiShou (true);
        }
		transform.SetSiblingIndex(1100);
		
	}
	public void OnClickDownUp(BaseEventData eventData){
       
        
        if (ChapterScene._chapterScene.isGameStart)
        {

            SkillManager._skillManager.toDo(this, skillId);
        }
        else
        {
			range.gameObject.SetActive(false);
            ChapterScene._chapterScene.content.parent.parent.gameObject.SetActive(true);
			ChapterScene._chapterScene.showHuiShou (false);
        }
		transform.SetSiblingIndex(10);

    }
	void FixedUpdate(){
		if(isInit && ChapterScene._chapterScene.isGameStart){
			if (Time.time - frontTime > attackPingLv) {
				frontTime = Time.time;
				canAttackMonsterArr = MonsterManager.getInstance().getMonstersByNear (this,false);
				if (canAttackMonsterArr.Count > 0) {
					Attack ();
				}
				changMp (5.0f);
			}
		}

	}
	// Update is called once per frame
	void Update () {
		/**if (!ChapterScene._chapterScene.isGameStart) {
			if (range.isActiveAndEnabled) {
				body.transform.position = Input.mousePosition;
			}
		}**/
	}
	void Attack(){//普攻
		attackMusic.Play();
		//heroStyle.timeScale = 3;
		heroStyle.AnimationState.SetAnimation (0,"attack",false).timeScale = 5;
		heroStyle.AnimationState.AddAnimation (0,"stand",true,0.0f).timeScale = 1;
		AttackFinished ();


		
	}
	void AttackFinished(){//普攻动作结束
		for(int i = 0;i < canAttackMonsterArr.Count; i++){
			Monster monster = (Monster)canAttackMonsterArr [i];
			//if (!monster.isDead) {
				monster.changHp ();
				//if (monster.isDead) {
				//	changMp (5.0f);
				//}
			//}

		}
	}
	public int getSkillDamage(){//获取技能伤害
		return attackDamage;
	}
	void AddBuff(){//增益效果
		
	}
		
}
