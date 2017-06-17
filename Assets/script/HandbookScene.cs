using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public class HandbookScene : MonoBehaviour {

	public Button equipTab;
	public Button monsterTab;
	public Button heroTab;
    public Transform content;
    private string type = "monster";
	//private ArrayList cacheArray;
	//private ArrayList actvieArray;
    // Use this for initialization
    void Start () {
		//cacheArray = new ArrayList ();
		//actvieArray = new ArrayList ();
		//cacheArray.Add (handBookPanel._demoPanel);
        Debug.Log("进入图鉴界面");
		onclickBtn (1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
	void showPanel(string str,Button btn){
		if (btn.interactable)
		{
			BagPanel[] actvieArray = content.transform.GetComponentsInChildren<BagPanel> ();
			for(int i = 0; i < actvieArray.Length ; i++){
				BagPanel panel = actvieArray [i];
				//panel.transform.SetParent (null);
				PoolManager.getInstance ().addToPool (PoolManager.BAG_ITEM + panel.poolType,panel);
				//cacheArray.Add (panel);
			}
			type = str;
			if (btn == monsterTab) {
				foreach(KeyValuePair<int,JsonObject> kvp in DataManager.getInstance().monsterDicJson){
					add(DataManager.getInstance().monsterDicJson [kvp.Key]);
				}
				equipTab.interactable = true;
				heroTab.interactable = true;

			}
			if (btn == equipTab) { 
				foreach(KeyValuePair<int,JsonObject> kvp in DataManager.getInstance().equipDicJson){
					add(DataManager.getInstance().equipDicJson [kvp.Key]);
				}
				monsterTab.interactable = true;
				heroTab.interactable = true;

			}
			if (btn == heroTab) { 
				foreach(KeyValuePair<int,JsonObject> kvp in DataManager.getInstance().heroDicJson){
					add(DataManager.getInstance().heroDicJson [kvp.Key]);
				}
				monsterTab.interactable = true;
				equipTab.interactable = true;

			}
			btn.interactable = false;
		}

	}
	public void onclickBtn(int type){
		switch (type) {
		case 1:
			showPanel ("monster",monsterTab);
			break;
		case 2:
			showPanel ("equip",equipTab);
			break;
		case 3:
			showPanel ("hero",heroTab);
			break;
		default:
			break;
		}
	}
    public void add(JsonObject cd)
    {
		if(int.Parse(cd["id"].ToString()) > 0){
			BagPanel panel;
			if (cd.ContainsKey ("color")) {
				panel = (BagPanel)PoolManager.getInstance ().getGameObject (PoolManager.BAG_ITEM + cd ["color"].ToString ());
			} else {
				panel= (BagPanel)PoolManager.getInstance ().getGameObject (PoolManager.BAG_ITEM + "green");
			}

			//if (cacheArray.Count > 0) {
			//	handBookPanel panel = (handBookPanel)cacheArray [0];
				panel.transform.SetParent (content);
				panel.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
				panel.init (cd, 0);
			//	cacheArray.RemoveAt (0);
			//	actvieArray.Add (panel);
			/**
	        //Button btn;
	        if (content.childCount == 0)
	        {
	            handBookPanel._demoPanel.transform.SetParent(content);
	            handBookPanel._demoPanel.init(cd, type);
				cacheArray.Add (handBookPanel._demoPanel);
	            //OnChangeHero(cd,heroHeadDemo);
	        }
	        else
	        {
	           
				cacheArray.Add (panel);
	            //btn.transform.SetParent (content.transform);
	        }

	        //heroHeadList.Add (btn);
	        //btn.onClick.AddListener(delegate () {
	        //    this.OnChangeHero(cd, btn);

	        //});
			**/
		}
    }
}
