using UnityEngine;
using System.Collections;
using TFSG;
using SimpleJson;

public class TowerManager {
	private ArrayList towerList = new ArrayList();
	private static TowerManager _towerManager;
    private Tower towerDemo;
	public static TowerManager getInstance(){//获取单例
		if(_towerManager == null){
			_towerManager = new TowerManager();
		}
		return _towerManager;
	}

	public TowerManager(){
		//foreach (Monster p in list)
	}
	public void addTower(Tower tower){
        if (towerDemo == null)
        {
            towerDemo = tower;
        }
		//towerList.Add(tower);		
	}
    public ArrayList getTowersList()
    {
        return towerList;
    }
    public void ClearTowers()
    {
        towerList = new ArrayList();
    }
    public void initChapterTower(string towerTxtPath)
    {
        string[][] arr = DataManager.getInstance().getData(towerTxtPath, "\r\n",0);
        for (int i = 0; i < arr.Length; i++)
        {
            string[] position = arr[i];
            Tower tower;
            if (towerList.Count == 0)
            {
                tower = towerDemo;
            }
            else
            {
                tower = (Tower)GameObject.Instantiate(towerDemo, towerDemo.transform.position, towerDemo.transform.rotation, towerDemo.transform.parent);
                
            }
			tower.transform.localPosition = new Vector3(float.Parse(position[0]), float.Parse(position[1]), 0);
			bool fiex = false;
			if (position.Length == 3) {
				if (float.Parse (position [2]) == 0.0f) {
					tower.spineFlipX = false;
				} else {
					tower.spineFlipX = true;
				}
			}
			towerList.Add(tower);
			/**ArrayList heros = HeroManager.getInstance ().getHeros ();
			if (heros.Count > i) {
				JsonObject hd = heros [i] as JsonObject;
				if (hd != null) {
					tower.initTower (hd);
				} else {
					tower.initTower (null);
				}

			} else {
				tower.initTower (null);
			}**/
			tower.initTower (null);

        }
    }
}
