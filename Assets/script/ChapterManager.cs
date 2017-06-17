using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using SimpleJson;
using System.Collections.Generic;
public class ChapterManager {
	//public static int chapterId;
	private static ChapterManager _instance;
	private string[][] chapterList;
	private string chapterBg;
	public int loveNum;
	private chapterData cd;
	public int chapterType = 0;
	public static ChapterManager getInstance(){//获取单例
		if(_instance == null){
			_instance = new ChapterManager();
		}
		return _instance;
	}

	public ChapterManager(){
		
		//chapterList = DataManager.getInstance ().getData ("data/chapter","\r\n");
	}
	public void setChapterId(int chapterId){
		if (DataManager.getInstance ().chapterDic.ContainsKey (chapterId)) {
			cd = DataManager.getInstance().chapterDic [chapterId];
			loveNum = cd.chapterLoveNum;
			//chapterType = type;
		}

	}
	public chapterData getChapter(){
		if (cd == null)
			setChapterId (1);
		return cd;
	}
    public void GotoChapterScene()
    {
		Time.timeScale = 1;
		SkillManager.getInstance ().Clear();
        TowerManager.getInstance().ClearTowers();
		MonsterManager.getInstance ().ClearMonster ();
        SceneManager.LoadScene("ChapterScene");
		loveNum = cd.chapterLoveNum;
    }
	public void GotoNextChapterScene()
	{
		Time.timeScale = 1;
		SkillManager.getInstance ().Clear();
		TowerManager.getInstance().ClearTowers();
		MonsterManager.getInstance ().ClearMonster ();
		if (cd != null) {
			setChapterId (cd.chapterId + 1);
		} else {
			setChapterId (1);
		}

		SceneManager.LoadScene("ChapterScene");
		loveNum = cd.chapterLoveNum;
	}
	public void changeLoveNum(int num){
		loveNum -= num;
		ChapterScene._chapterScene.chapterQiZhi.currentHP -= num;
		ChapterScene._chapterScene.chapterQiZhi.changHp ();
	}
	public int getStar(){//星数评分
		int star = 0;
		if (loveNum == cd.chapterLoveNum) {
			star =  3;
		} else if (loveNum > (cd.chapterLoveNum / 2)) {
			star = 2;
		} else {
			star = 1;
		}
		cd.star = star;
		return star;
	}
	public GameObject clone(GameObject obj,Transform _parent){
		return GameObject.Instantiate (obj,obj.transform.localPosition,obj.transform.localRotation,_parent);
	}
}
