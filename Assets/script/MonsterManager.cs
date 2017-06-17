using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager {
	private string[][] monsterList;
	private static MonsterManager _monsterManager;
	private Monster _monsterDemo;
	private ArrayList monsterCacheArr;//对象池
	public ArrayList activeMonsterArr;//激活的怪物
	public static MonsterManager getInstance(){//获取单例
		if(_monsterManager == null){
			_monsterManager = new MonsterManager();
		}
		return _monsterManager;
	}

	public MonsterManager(){
		
		activeMonsterArr = new ArrayList ();
		//foreach (Monster p in list)
		//monsterData = new Dictionary<int, ArrayList>();
	}
	public void setMonsterDemo(Monster _monster){
		if (_monsterDemo == null) {
			monsterCacheArr = new ArrayList ();
			_monsterDemo = _monster;
			monsterCacheArr.Add (_monsterDemo);
			for(int k =0;k < 10; k++){
				monsterCacheArr.Add (getMonster(false));
			}

			//monsterList.Add (_monsterDemo);
		}
	}
	public void addMonster(Monster monster){
		activeMonsterArr.Add (monster);
		//activeMonsterArr.Sort (0,activeMonsterArr.Count,new MonsterSortByY ());
		//monsterList.Add(monster);		
	}
	public void ClearMonster(){
		activeMonsterArr.Clear ();
	}
	public void removeMonster(Monster monster){
		if(activeMonsterArr.Contains(monster)){
			activeMonsterArr.Remove (monster);
		}
	}
	public void addToCachePool(Monster _monster){
		monsterCacheArr.Add(_monster);
	}
	public Monster getMonster(bool isFromCache){
		if (!isFromCache) {
			return (Monster)GameObject.Instantiate (_monsterDemo, _monsterDemo.transform.position, _monsterDemo.transform.rotation);
		} else {
			if (monsterCacheArr.Count > 0) {
				Monster _monster = (Monster)monsterCacheArr [0];
				monsterCacheArr.RemoveAt (0);
				return _monster;
			} else {
				return (Monster)GameObject.Instantiate (_monsterDemo, _monsterDemo.transform.position, _monsterDemo.transform.rotation);
			}
		}

	}
	public void initMonsterData(string monsterPath){
		monsterList = DataManager.getInstance ().getData (monsterPath,"\r\n",0);

	}
	public ArrayList getMonstersByBoShu(int boshu,Transform _parent){
		int siblingIndex = 200;
		ArrayList list = new ArrayList ();
		for(int i =1;i < monsterList.Length; i++)
		{  
			string [] oneData = monsterList[i];
			int b = int.Parse(oneData [0]);
			//if(boshu == b){
				int num = int.Parse(oneData [2]);
				ArrayList arr = new ArrayList();
				arr.Add (float.Parse(oneData [4]));
				for(int k =0;k < num; k++)
				{  
					Monster _monster = getMonster (true);
					//_monster.transform.SetParent(_parent);
					_monster.transform.SetParent(_parent);
					_monster.transform.localScale = new Vector3(1,1,1);
					_monster.gameObject.SetActive (false);
					_monster.init (oneData,siblingIndex--);
					arr.Add(_monster);
				}
				list.Add (arr);
			//}
		}
		return list;
	}

	public ArrayList getMonstersByNear(Tower tower,bool isAOE = false){//群攻或单攻根据攻击范围选取monster
		ArrayList arrs = new ArrayList();
		//Monster enemy;
		for (var j = 0; j < activeMonsterArr.Count; j++) {
			Monster enemy = (Monster)activeMonsterArr[j];
			if (enemy != null && enemy.currentHP > 0) {
				float distance = Vector3.Distance (tower.transform.localPosition,enemy.transform.localPosition);

				//Debug.Log("tt distance is:" + distance);
				if (tower.attackRange >= distance) {
					//Debug.Log("在" + towerName + "攻击范围内！！！");

					if(enemy.currentHP > 0){
						enemy.currentHP -= tower.attackDamage;
						if(enemy.currentHP <= 0)
						{
							enemy.isDead = true;
						}
						arrs.Add (enemy);
						if (!isAOE) {
							break;
						}

					}
				}
			}
		}
		return arrs;
	}

	public ArrayList getMonstersByY(float y1,float y2){//群攻或单攻根据攻击范围选取monster
		ArrayList arrs = new ArrayList();
		//Monster enemy;
		for (var j = 0; j < activeMonsterArr.Count; j++) {
			Monster enemy = (Monster)activeMonsterArr[j];
			if (enemy != null && enemy.currentHP > 0) {
				float y= enemy.transform.position.y;
				if (y1 <= y && y2 >= y) {
					enemy.transform.SetSiblingIndex(1100);
				} else {
					enemy.transform.SetSiblingIndex(3);

				}
			}
		}
		return arrs;
	}
	public ArrayList SetMonsterSiblingIndex(int index){//群攻或单攻根据攻击范围选取monster
		ArrayList arrs = new ArrayList();
		//Monster enemy;
		for (var j = 0; j < activeMonsterArr.Count; j++) {
			Monster enemy = (Monster)activeMonsterArr[j];
			if (enemy != null && enemy.currentHP > 0) {
					enemy.transform.SetSiblingIndex(index);
	
			}
		}
		return arrs;
	}
	public ArrayList getMonstersByRect(Image _image,bool isChooseOne = false,Monster lockMonster = null){//群攻或单攻根据攻击范围选取monster
		ArrayList arrs = new ArrayList();
		//Monster enemy;
		for (var j = 0; j < activeMonsterArr.Count; j++) {
			Monster enemy = (Monster)activeMonsterArr[j];
			if (enemy != null && enemy.currentHP > 0) {
				
				//float y= enemy.transform.position.y;

				Vector3 _p = _image.transform.InverseTransformPoint(enemy.transform.position);
				if (_image.rectTransform.rect.Contains(_p)){
					enemy.transform.SetSiblingIndex(1100);
					arrs.Add (enemy);
					if (isChooseOne) {
						if (lockMonster != enemy && lockMonster != null) {
							lockMonster.transform.SetSiblingIndex (3);
						}
						//lockMonster = enemy;
						break;
					}
				} else {
						enemy.transform.SetSiblingIndex (3);
				}
			}
		}
		return arrs;
	}
	public ArrayList getMonstersBySkill(Image _image){//群攻或单攻根据攻击范围选取monster
		ArrayList arrs = new ArrayList();
		//Monster enemy;
		for (var j = 0; j < activeMonsterArr.Count; j++) {
			Monster enemy = (Monster)activeMonsterArr[j];
			if (enemy != null && enemy.currentHP > 0) {

				//float y= enemy.transform.position.y;

				Vector3 _p = _image.transform.InverseTransformPoint(enemy.transform.position);
				if (_image.rectTransform.rect.Contains(_p)) {
					arrs.Add (enemy);
				}
			}
		}
		return arrs;
	}

	public ArrayList getMonstersByRect2(ArrayList list){//群攻或单攻根据攻击范围选取monster
		ArrayList arrs = new ArrayList();
		//Monster enemy;
		for (var j = 0; j < activeMonsterArr.Count; j++) {
			Monster enemy = (Monster)activeMonsterArr[j];
			if (enemy != null && enemy.currentHP > 0) {

				//float y= enemy.transform.position.y;
				Skill _image = (Skill)list [1];
				Vector3 _p = _image.transform.InverseTransformPoint(enemy.transform.position);
				if (_image.attackRange.Contains(_p) && !list.Contains(enemy)) {
					if(enemy.currentHP > 0){
						enemy.attackByTowerSkill ((Tower)list [0]);
						arrs.Add (enemy);
					}
					list.Add (enemy);
				}
			}
		}
		return arrs;
	}
	 
}
