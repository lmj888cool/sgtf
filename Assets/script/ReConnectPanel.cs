using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJson;
public class ReConnectPanel : MonoBehaviour {

	public Button exitBtn;
	public Button reconnectBtn;
	public Image panel;
	public Image connectingPanel;
	public Image bg;
	public bool isReconnect;
	public string poolType;
	void Awake () {
		poolType = "RE_CONNECT";
		//PoolManager.getInstance ().initPoolByType (PoolManager.RE_CONNECT,this,1);
		//DontDestroyOnLoad (this.gameObject);
		//this.gameObject.SetActive (false);
		ServerManager.getInstance()._ReConnectPanel = this;

	}
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
	
	}
	public void show(int type){
		Loom.QueueOnMainThread(()=>{
			
			this.transform.SetParent(Loom.Current.transform);
			this.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
			this.gameObject.SetActive (true);
			if(type == 1){
				panel.gameObject.SetActive (true);
				connectingPanel.gameObject.SetActive (false);
			}else{
				panel.gameObject.SetActive (false);
				connectingPanel.gameObject.SetActive (true);
					Loom.QueueOnMainThread (() => {
						if(ServerManager.getInstance().connectState == Pomelo.DotNetClient.NetWorkState.CONNECTING){
							ServerManager.getInstance().disconnectServer();
							show(1);
						}
					},5.0f);
			}


		});

	}
	public void hide(){
		Loom.QueueOnMainThread(()=>{
			this.gameObject.SetActive (false);
		});
	}
	public void reConnectServer(){
		//Debug.Log("start reConnectServer"+ entry_host +  entry_port);
		if (ServerManager.getInstance ().entry_port == -1) {
			show (1);
			return;

		}
		ServerManager.getInstance ().connectServer (
			ServerManager.getInstance ().entry_host, 
			ServerManager.getInstance ().entry_port, 
			(data) => {
			Debug.Log("reConnectServer ok");
			//Entry ();
			JsonObject userMessage = new JsonObject();
			userMessage.Add("token", "lmj");
			userMessage.Add("rid", 2);
				userMessage.Add ("username", ServerManager.getInstance ().usename);
				userMessage.Add ("passwd", ServerManager.getInstance ().passward);
			//if (pclient != null) {
				ServerManager.getInstance ().request("connector.entryHandler.entry", userMessage, (data2)=>{
				Debug.Log(data2.ToString());
				if(int.Parse(data2["code"].ToString()) == 200){
					JsonObject userMessage3 = new JsonObject();
					//userMessage.Add ("name", playerName.text);
					//if (pclient != null) {
						ServerManager.getInstance ().request("area.playerHandler.enterScene", userMessage3, (data3)=>{
						Debug.Log(data3.ToString());
						if(data.ContainsKey("code") && data["code"].ToString() == "500"){
							Debug.Log("角色名已经被占用!!");
						}else{
							DataManager.playerData = data3;//["curPlayer"] as JsonObject;
								hide();
							Loom.QueueOnMainThread(()=>{
									
									ServerManager.getInstance ().onUpgrade();
							});
						}
					});

				}else{
					//Loom.QueueOnMainThread(()=>{
						show(1);
					//});
					
				}

			});
		});


	}
	public void onclickBtn(int type){
		if (type == 2) {//重连
			Loom.QueueOnMainThread(()=>{
				panel.gameObject.SetActive (false);
				connectingPanel.gameObject.SetActive (true);
			});
			Loom.RunAsync(()=>{
				reConnectServer();
			});


		}else if(type == 1){//退出
			Application.Quit();
		}
	}
}
