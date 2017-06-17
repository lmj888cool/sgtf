using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Pomelo.DotNetClient;
using SimpleJson;

public class LoginScene : MonoBehaviour {
	public Image LoginPanel;
	public Image createPlayPanel;
	public static PomeloClient pclient;
	public InputField ip;
	public InputField username;
	public InputField passward;
	public InputField playerName;
	public int isCanLogin = 0;
	// Use this for initialization
	void Start () {
		//屏幕适配,按宽度缩放
		DataManager.getInstance();
		LoginPanel.transform.localPosition = new Vector3(0,0,0);
		createPlayPanel.transform.localPosition = new Vector3(0,0,0);
		createPlayPanel.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (isCanLogin > 0) {
			if (isCanLogin == 1) {//该账号下有角色
				SceneManager.LoadScene ("GameScene");
			}else if(isCanLogin == 2){//该账号下没有角色,跳到创建角色界面
				createPlayPanel.gameObject.SetActive (true);
				LoginPanel.gameObject.SetActive (false);
			}

			isCanLogin = 0;
		}
	}
	public void onClickLogin(){
		
		//AudioManager.instance.playBtnClick ();
		//SceneManager.LoadScene ("GameScene");
		ServerManager.getInstance ().usename = username.text;
		ServerManager.getInstance ().passward = passward.text;
		string host=ServerManager.getInstance ().query_host = ip.text;//"192.168.1.106";//(www.xxx.com/127.0.0.1/::1/localhost etc.)
		int port=ServerManager.getInstance ().query_port = 3014;
		//Loom.RunAsync(()=>{
		ServerManager.getInstance ().connectServer (host,port,(data) =>
			{
				//process handshake call back data
				JsonObject msg = new JsonObject();
				msg["uid"] = "1000001";
				Debug.Log("111111222");
				//pclient.request("gate.gateHandler.queryEntry", msg, OnQuery);
				ServerManager.getInstance ().request("gate.gateHandler.queryEntry",msg,(data2) => {
					Debug.Log("111111");
					OnQuery (data2);
				});

			});
		//}
		//);
	}
		
	void OnQuery(JsonObject result){
		Debug.Log(result.ToString());
		if(int.Parse(result["code"].ToString()) == 200){
			//pclient.disconnect();

			string host = ServerManager.getInstance ().entry_host = (string)result["host"];
			int port =ServerManager.getInstance ().entry_port = int.Parse(result["port"].ToString());
			ServerManager.getInstance ().connectServer (host, port, (data) => {
					Debug.Log("22222");
					Entry ();
			});
		}
	}
	
	//Entry chat application.
	void Entry(){
		JsonObject userMessage = new JsonObject();
		userMessage.Add("token", "lmj");
		userMessage.Add("rid", 2);
	
		userMessage.Add ("username", username.text);
		userMessage.Add ("passwd", passward.text);
		//if (pclient != null) {
			ServerManager.getInstance ().request("connector.entryHandler.entry", userMessage, (data)=>{
				Debug.Log(data.ToString());
				if(int.Parse(data["code"].ToString()) == 200){
					if(data.ContainsKey("player")){
						//DataManager.playerData = data["player"] as JsonObject;
						goMainScene();

					}else{
						isCanLogin = 2;
					}
					
				}else{
					TipManager.instance.showTip (int.Parse(data["code"].ToString()));
				}
				//Application.LoadLevel(Application.loadedLevel + 1);

			});
		//}
	}

	public void Create(){
		JsonObject userMessage = new JsonObject();
		//userMessage.Add("token", "lmj");
		//userMessage.Add("roleId", 2);
		userMessage.Add ("name", playerName.text);
		//if (pclient != null) {
		ServerManager.getInstance ().request("connector.roleHandler.createPlayer", userMessage, (data)=>{
				Debug.Log(data.ToString());
				if(data["code"].ToString() == "200"){
					//Entry();
					if(data.ContainsKey("player")){
						goMainScene();
						//DataManager.playerData = data["player"] as JsonObject;
						//isCanLogin = 1;
					}else{
						isCanLogin = 2;
					}

				}else{

				}
				//Application.LoadLevel(Application.loadedLevel + 1);

			});
		//}
	}
	public void goMainScene(){
		JsonObject userMessage = new JsonObject();
		//userMessage.Add ("name", playerName.text);
		//if (pclient != null) {
			ServerManager.getInstance ().request("area.playerHandler.enterScene", userMessage, (data)=>{
				Debug.Log(data.ToString());
				if(data.ContainsKey("code") && data["code"].ToString() == "500"){
						Debug.Log("角色名已经被占用!!");
				}else{
					DataManager.playerData = data;//["curPlayer"] as JsonObject;
					isCanLogin = 1;
					ServerManager.getInstance().onUpgrade();
				}
			});
		}
	//}
}
