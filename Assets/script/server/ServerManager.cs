using System.Collections;
using System.Timers;
using System.Collections.Generic;
using Pomelo.DotNetClient;
using SimpleJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class ServerManager {
	private static ServerManager _serverManager;
	private PomeloClient pclient = null;
	public string query_host="127.0.0.1";//服务器网关查询IP
	public int query_port=-1;//服务器网关查询端口

	public string entry_host="127.0.0.1";//服务器IP
	public int entry_port=-1;//服务器端口
	public string usename;
	public string passward;
	public NetWorkState connectState;
	public bool isShowReConnectPanel = false;
	public ReConnectPanel _ReConnectPanel;
	public static ServerManager getInstance(){//获取单例
		if(_serverManager == null){
			_serverManager = new ServerManager();
		}
		return _serverManager;
	}

	public ServerManager(){
		

	}
	public void disconnectServer(){
		if (pclient != null) {
			isShowReConnectPanel = false;
			pclient.disconnect ();
			pclient = null;
		}
	}
	public void NetWorkStateChangedEvent(NetWorkState state){
		Debug.Log("NetWorkStateChangedEvent:" + state.ToString());
		Loom.QueueOnMainThread (() => {
			connectState = state;
			if (NetWorkState.CONNECTED == state) {
				TipManager.instance.hideReconnectPanel();
			} else if (NetWorkState.CONNECTING == state) {

			} else {//连接错误
				//TipManager.instance.showReconnectPanel(1);
				//Debug.Log("isShowReConnectPanel:" + isShowReConnectPanel.ToString());
				//if (isShowReConnectPanel) {
					TipManager.instance.showReconnectPanel(1);
				//}
				//isShowReConnectPanel = true;
			}
		});
	}
	public void connectServer(string host,int port,
		System.Action<JsonObject> callBack,
		int connectType = 1){
		if (pclient != null) {
			//isShowReConnectPanel = false;
			pclient.NetWorkStateChangedEvent -= NetWorkStateChangedEvent;
			pclient.disconnect ();
			pclient = null;
		}

		pclient = new PomeloClient();
		TipManager.instance.showReconnectPanel(0);
		pclient.NetWorkStateChangedEvent += NetWorkStateChangedEvent;

		pclient.initClient(host, port, () =>
			{
				//JsonObject user = new JsonObject();
				pclient.connect(null,callBack);
			});
		
	}
	public void request(string rute,JsonObject msg,System.Action<JsonObject> callBack){
		TipManager.instance.showReconnectPanel(0);
		bool isReturnData = false;
		pclient.request(rute, msg, (data)=>{
			Loom.QueueOnMainThread(()=>{
				TipManager.instance.hideReconnectPanel();
				callBack(data);
			}
			);
		});
	}
	public void onUpgrade(){
		pclient.on ("onUpgrade", (data) => {
			Debug.Log ("get update info :" + data.ToString ());
			NotificationManager.getInstance().PostNotification(null,Message.MONEY_GOLD_UPDATE);
		});
		pclient.on ("onUpgradeHero", (data) => {
			Debug.Log ("onUpgradeHero :" + data.ToString ());
			HeroManager.getInstance().updateHeroByServer(data);
		});
		pclient.on ("onUpgradeBag", (data) => {
			Debug.Log ("onUpgradeBag :" + data.ToString ());
			BagManager.getInstance().updateItemByServer(data);
			BagManager.getInstance().getGameScene().mainPanel.updateData();
		});
	}
}
