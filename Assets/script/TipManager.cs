using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TipManager {
	public static readonly TipManager instance = new TipManager();
	private GameObject objPrefab;
	private ReConnectPanel _ReConnectPanel;
	private TipManager()
	{
		init();
	}

	public void init()
	{
	}

	public void showTip(int tipId)
	{
		string tipStr;
		if(DataManager.getInstance().languageJson.ContainsKey(tipId)){
			tipStr = DataManager.getInstance().languageJson[tipId]["name"].ToString();
		}else{
			tipStr = "undefined:" + tipId.ToString();
		}
		showTip (tipStr);
	}
	public void showTip(string tipStr)
	{

		Loom.QueueOnMainThread (() => {
			//Object objPrefab = Resources.Load ("Text");

			if(objPrefab == null){
				objPrefab = GameObject.Instantiate((GameObject)Resources.Load("tip"));

			}

			objPrefab.transform.SetParent(Loom.Current.transform);
			objPrefab.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
			objPrefab.GetComponentInChildren<Text>().text = tipStr;
			Loom.QueueOnMainThread (() => {
				objPrefab.transform.SetParent(null);
			},2.0f);
		});
	}
	public void showReconnectPanel(int type){
		Loom.QueueOnMainThread (() => {
			if (_ReConnectPanel == null) {
				GameObject _ob = GameObject.Instantiate ((GameObject)Resources.Load ("reconnectPanel"));
				//_ob.SetActive(true);
				//_ob.transform.SetParent(Loom.Current.transform);
				//_ob.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
				//_rcScript = (ReConnectPanel)_ReConnectPanel.GetComponent<ReConnectPanel> ();
				_ReConnectPanel = (ReConnectPanel)_ob.GetComponent<ReConnectPanel> ();
			}
			Debug.Log("showReconnectPanel:" + Time.deltaTime.ToString() + "type:" + type.ToString());
			_ReConnectPanel.show (type);
		});
	}
	public void hideReconnectPanel(){
		Debug.Log("hideReconnectPanel:" + Time.deltaTime.ToString());
		if (_ReConnectPanel != null) {
			_ReConnectPanel.hide ();
		}

	}
}
