using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class SkillManager {
	public static SkillManager _skillManager;
	private Dictionary<int,skillData> skillDic;
	public Image skill001;
	public Image skill002;
	public Image skill003;
	public Image skill004;
	public Image skillbg;
	private Skill skillDemo;
	private ArrayList skillCacheArr;
	private ArrayList skillEffectPool;
	private ArrayList deleteSkillArr;
	private Dictionary<int,ArrayList> skillAttackedMonsterArr;//被技能攻击过的怪物列表
	private int skillId;
	private Rect screenRect;
	private skillData currentSkill;
	private Monster lockMonster;//技能类型为4的技能当前锁定的目标

	public static SkillManager getInstance(){//获取单例
		if(_skillManager == null){
			_skillManager = new SkillManager();
		}
		return _skillManager;
	}

	public SkillManager(){
		skillEffectPool = new ArrayList ();
		deleteSkillArr = new ArrayList ();
		skillAttackedMonsterArr = new Dictionary<int, ArrayList> ();
		//foreach (Monster p in list)
		//monsterData = new Dictionary<int, ArrayList>();
	}
	// Use this for initialization
	public void Start () {
		
		//_skillManager = this;
		skill001.gameObject.SetActive (false);
		skill002.gameObject.SetActive (false);
		skill003.gameObject.SetActive (false);
		skill004.gameObject.SetActive (false);
		skillbg.transform.localPosition = new Vector3 (0, 0, 0);
		//transform.SetSiblingIndex (999);
		skillbg.gameObject.SetActive(false);
		screenRect = new Rect (new Vector2(0,0),new Vector2(Screen.width,Screen.height));		//screenRect = ChapterScene._chapterScene.bg.rectTransform.rect;
		//initSkillData();

		//UGUIEventTrigger.Get (skill001).AddEventListener (EventTriggerType.Move,OnMove);
	
	}
	public void setSomeGameObject(Image img1,Image img2,Image img3,Image img4,Image img5){
		skill001 =img1;
		skill002 = img2;
		skill003 = img3;
		skill004 = img4;
		skillbg = img5;
		Start ();
		
	}
	public void setSkillDemo(Skill skill){
		if(skillDemo == null){
			skillDemo = skill;
			skillCacheArr = new ArrayList ();
			skillDemo.gameObject.SetActive (false);
			skillCacheArr.Add (skillDemo);
			for(int k =0;k < 10; k++){
				Skill skill2 = (Skill)GameObject.Instantiate (skillDemo, skillDemo.transform.position, skillDemo.transform.rotation);
				skillCacheArr.Add (skill2);
			}
		}
	}
	public void Clear(){
		skillAttackedMonsterArr.Clear ();
		currentSkill = null;
	}
	public Skill getSkillDemo(int skillId){
		//Skill skill;
		if (skillCacheArr.Count > 0) {
			bool b = false;
			for(int i=0;i < skillCacheArr.Count;i++){
				Skill skill1 = (Skill)skillCacheArr [i];
				if (skill1.skillId == skillId) {
					b = true;
					//skill = skill1;
					skillCacheArr.RemoveAt (i);
					skill1.transform.rotation = new Quaternion (0, 0, 0, 1);//纠正旋转
					return skill1;
				}
			}
			if (!b) {
				Skill skill = (Skill)skillCacheArr [0];
				skillCacheArr.RemoveAt (0);
				skill.transform.rotation = new Quaternion (0, 0, 0, 1);//纠正旋转
				return skill;
			}


		} else {
			Skill skill =  (Skill)GameObject.Instantiate (skillDemo, skillDemo.transform.position, skillDemo.transform.rotation);
			skill.transform.rotation = new Quaternion (0, 0, 0, 1);//纠正旋转
			return skill;
		}
		//skill.transform.rotation = new Quaternion (0, 0, 0, 1);//纠正旋转
		//skill.gameObject.SetActive (true);
		return null;
	}
	public void addToCache(Skill skill){
		skillCacheArr.Add (skill);
	}
	public skillData getSkillById(int skillid){
		return DataManager.getInstance ().skillDic [skillid];
	}

	public void OnMove(BaseEventData eventData){
		PointerEventData _PointerEventData = (PointerEventData)eventData;
		Debug.Log (_PointerEventData.position.x + "     " + _PointerEventData.position.y);
		//SkillManager._skillManager.PlaySkill (1001);
	}
	public void FixedUpdate(){


	}
	// Update is called once per frame
	public void Update () {
		if (Time.timeScale == 0.0f && currentSkill != null) {
            //int skillType = getSkillById ();
            Vector3 _localP = ChapterScene._chapterScene.bg.transform.InverseTransformPoint(Input.mousePosition);
            if (currentSkill.skillType == 2) {
				skill001.transform.localPosition = new Vector3 (skill001.transform.localPosition.x, _localP.y, 0);
				MonsterManager.getInstance ().getMonstersByRect (skill001);
				//MonsterManager.getInstance ().getMonstersByY (skill001.transform.position.y - 50,skill001.transform.position.y +50);
			} else if(currentSkill.skillType == 1){
				//skill001.transform.rotation
				float  radian=Mathf.Atan2((_localP.y-skill001.transform.localPosition.y),(_localP.x-skill001.transform.localPosition.x));

				float f=(float)(radian*180/3.14);
				Debug.Log(radian + "   " + f);
				skill001.transform.localRotation = Quaternion.AngleAxis (f, Vector3.forward);
				//MonsterManager.getInstance ().getMonstersByY (skill001.transform.position.y - 50,skill001.transform.position.y +50);
				MonsterManager.getInstance ().getMonstersByRect (skill001);

			}else if(currentSkill.skillType == 4){
				skill004.transform.localPosition =_localP;

				ArrayList arr = MonsterManager.getInstance ().getMonstersByRect (skill004,true,lockMonster);//只选取一个目标
				if (arr.Count > 0) {
					lockMonster= (Monster)arr[0];
				}

				
			}else if(currentSkill.skillType > 3){
				
				//MonsterManager.getInstance ().getMonstersByY (skill001.transform.position.y - 50,skill001.transform.position.y +50);
				//MonsterManager.getInstance ().getMonstersByRect (skill001);
				float  radian=Mathf.Atan2((_localP.y-skill002.transform.localPosition.y),(_localP.x-skill002.transform.localPosition.x));

				float f=(float)(radian*180/3.14);
				Debug.Log(radian + "   " + f);
				skill002.transform.localRotation = Quaternion.AngleAxis (f, Vector3.forward);
				float dis = Vector3.Distance(_localP, skill002.transform.localPosition);
				if (dis <= skill002.rectTransform.rect.width/2) {
					skill003.transform.position = Input.mousePosition;
				}
				MonsterManager.getInstance ().getMonstersByRect (skill003);

			}

		} else {

		}

		//skill001.transform.GetComponent<Rigidbody2D> ().MovePosition (Input.mousePosition);
	}
	public void PlaySkill(Tower tower,int _skillId){//释放技能
		if(skillAttackedMonsterArr.ContainsKey(_skillId)) 
			return;
		skillId = _skillId;
		currentSkill = getSkillById (skillId);
		skillbg.transform.SetSiblingIndex (1000);
		skillbg.gameObject.SetActive (true);
		Time.timeScale = 0;
        Vector3 _localP= ChapterScene._chapterScene.bg.transform.InverseTransformPoint(Input.mousePosition);
		skill003.transform.localPosition = new Vector3(0,0,0);
		if(currentSkill.skillType <= 2){
			skill001.gameObject.SetActive (true);
			skill001.transform.rotation = new Quaternion (0, 0, 0, 1);
			RectTransform rectTransform = skill001.gameObject.GetComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, currentSkill.attackRange);
		}
		if (currentSkill.skillType == 2) {
			
            float x =  - ChapterScene._chapterScene.bg.rectTransform.rect.width / 2;

            skill001.transform.localPosition = new Vector3 (x, _localP.y, 0);
		} else if(currentSkill.skillType == 1){
			skill001.transform.localPosition = tower.transform.localPosition;
		} else if(currentSkill.skillType >= 3){
			if (currentSkill.skillType == 4) {
				skill004.gameObject.SetActive (true);
			} else {
				skill002.gameObject.SetActive (true);

				RectTransform rectTransform2 = skill002.gameObject.GetComponent<RectTransform>();
				rectTransform2.sizeDelta = new Vector2(tower.attackRange*2, tower.attackRange *2);

				//skill002.transform.localScale = new Vector3 (scale,scale,scale);
				skill002.transform.localPosition = tower.transform.localPosition;
				if (currentSkill.skillType != 3) {
					skill003.gameObject.SetActive (true);
					RectTransform rectTransform3 = skill003.gameObject.GetComponent<RectTransform>();
					rectTransform3.sizeDelta = new Vector2(currentSkill.attackRange, currentSkill.attackRange);
					skill003.transform.localPosition = _localP;
				}
			}


		}
		//UGUIEventTrigger.Get (skill001.gameObject).AddEventListener (EventTriggerType.Drag,OnMove);
	}
	public void toDo(Tower tower,int _skillId){
		if(skillAttackedMonsterArr.ContainsKey(_skillId) || !skillbg.isActiveAndEnabled)
			return;
		Time.timeScale = 1;
		MonsterManager.getInstance ().SetMonsterSiblingIndex (0);
		skillId = _skillId;
		currentSkill = getSkillById (skillId);
		skillbg.gameObject.SetActive (false); 
		skill001.gameObject.SetActive (false);
		skill002.gameObject.SetActive (false);
		skill003.gameObject.SetActive (false);
		skill004.gameObject.SetActive (false);
		Skill skilleffect = getSkillDemo (skillId);
        
		skilleffect.init(currentSkill);
		skilleffect.transform.SetParent (ChapterScene._chapterScene.bg.transform);
		skilleffect.gameObject.SetActive (true);
		skilleffect.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
		if (currentSkill.skillType == 9) {
			skilleffect.transform.SetSiblingIndex (0);
		} else {
			skilleffect.transform.SetSiblingIndex (1100);
		}

		skillAttackedMonsterArr[skillId] = new ArrayList();
		skillAttackedMonsterArr [skillId].Add (tower);
		skillAttackedMonsterArr [skillId].Add (skilleffect);
		if(currentSkill.skillType >= 3){
            //skillEffect.transform.rotation = skill001.transform.rotation;
			if (currentSkill.skillType == 4) {
				skilleffect.transform.position = skill004.transform.position;
			} else {
				skilleffect.transform.position = skill003.transform.position;
			}
           
			skillAttackedMonsterArr [skillId].Add (currentSkill.attackNum);
			skillAttackedMonsterArr [skillId].Add (currentSkill.attackInterval);
			skillAttackedMonsterArr [skillId].Add (currentSkill.attackInterval + 1.0f);
		}else{
            skilleffect.transform.localRotation = skill001.transform.localRotation;
            skilleffect.transform.localPosition = skill001.transform.localPosition;
		}



		//skillEffect.gameObject.SetActive (true);
	}
	public void updateByScene(){

		foreach (KeyValuePair<int,ArrayList> kvp in skillAttackedMonsterArr) {
			Skill skillEffect = (Skill)kvp.Value [1];
			if (skillEffect != null) {
				skillData skill = getSkillById (kvp.Key);
				if (skill.skillType == 1 || skill.skillType == 2) {
					skillEffect.transform.Translate (10, 0, 0);
					//Vector3 _p = screenRect.InverseTransformPoint(skillEffect.transform.position);
					if (!screenRect.Contains(skillEffect.transform.position)) {
						deleteSkillArr.Add(kvp.Key);
						skillEffect.gameObject.SetActive (false);
						skillCacheArr.Remove (skillEffect);
						addToCache (skillEffect);
					} else {
						MonsterManager.getInstance ().getMonstersByRect2 (kvp.Value);
					}

				} else if(skill.skillType >= 3) {
					int attackNum = (int)kvp.Value [2];
					float attackInterval = (float)kvp.Value [3];
					float time = (float)kvp.Value [4];
					if (time > attackInterval && skillEffect.isCanAttack) {
						kvp.Value [4] = 0.0f;
						if (attackNum > 0) {
							ArrayList arr;
							if (skill.skillType == 4) {
								arr = MonsterManager.getInstance ().getMonstersByRect (skill004,true);
							}else if (skill.skillType == 3) {
								arr = MonsterManager.getInstance ().getMonstersBySkill (skill002);
							} else {
								arr = MonsterManager.getInstance ().getMonstersBySkill (skill003);
							}

							for (var j = 0; j < arr.Count; j++) {
								Monster enemy = (Monster)arr [j];
								if (enemy.currentHP > 0) {
									enemy.attackByTowerSkill ((Tower)kvp.Value [0]);
								}
							}
							attackNum--;
							kvp.Value [2] = attackNum;
						} else {
							if (skillEffect.isPalyOne) {
								skillEffect.gameObject.SetActive (false);
								//skillCacheArr.Remove (skillEffect);
								addToCache (skillEffect);
								deleteSkillArr.Add (kvp.Key);
							}

						}
					} else {
						time += Time.fixedDeltaTime;
						kvp.Value [4] = time;
					}




				}

			}
		}
		for (var j = 0; j < deleteSkillArr.Count; j++) {
			skillAttackedMonsterArr.Remove ((int)deleteSkillArr[j]);
		}
		deleteSkillArr.Clear ();
		//

	}
}
