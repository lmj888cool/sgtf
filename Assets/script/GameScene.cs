using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameScene : MonoBehaviour {
	public Image bg;
	public Image bottomBtnsPanel;
	public HeroScene heroPanel;
	public MainScene mainPanel;
	public SceletScene selectChapterPanel;
	public HandbookScene tupuPanel;
	public BagScene bagPanel;
	public Button mainBtn;
	public Button heroBtn;
	public Button tupuBtn;
	public Button chapterBtn;
	public Button bagBtn;
	private ArrayList panelList;
	private ArrayList brnList;
	//private SceletScene selectScene;
	//private HeroScene heroScene;
	//private MainScene mainScene;
	// Use this for initialization
	void Awake(){
		//selectScene = selectChapterPanel.gameObject.GetComponent<SceletScene> ();
		//selectScene.enabled = false;
		//heroScene = heroPanel.gameObject.GetComponent<HeroScene> ();
		//heroScene.enabled = false;
		heroPanel.gameObject.SetActive (false);
		selectChapterPanel.gameObject.SetActive (false);
		tupuPanel.gameObject.SetActive (false);
		bagPanel.gameObject.SetActive (false);
	}
	void Start () {
		//屏幕适配,按宽度缩放

		//显示主界面
		showPanel (mainPanel,mainBtn);

        BagManager.getInstance().setGameScene(this);
		//AudioManager.instance.playBg ();
	}

	// Update is called once per frame
	void Update () {
	
	}
	void showPanel(MonoBehaviour panel,Button btn){
        if (btn.interactable)
        {
			AudioManager.instance.playBtnClick ();
            if (panel != heroPanel) { heroPanel.gameObject.SetActive(false); heroBtn.interactable = true; }
            if (panel != selectChapterPanel) { selectChapterPanel.gameObject.SetActive(false); chapterBtn.interactable = true; }
            if (panel != tupuPanel) { tupuPanel.gameObject.SetActive(false); tupuBtn.interactable = true; }
            if (panel != mainPanel) { mainPanel.gameObject.SetActive(false); mainBtn.interactable = true; }
            if (panel != bagPanel) { bagPanel.gameObject.SetActive(false); bagBtn.interactable = true; }
            //MonoBehaviour scene = panel.gameObject.GetComponent<MonoBehaviour> ();
            panel.gameObject.SetActive(true);
            panel.transform.localPosition = new Vector3(0, 0, 0);
            btn.interactable = false;
        }
		
	}
	public void onclickBtn(int type){
		heroPanel.selectKind = null;
		switch (type) {
		case 1:
			mainPanel.fresh ();
			showPanel (mainPanel,mainBtn);
			break;
		case 2:
			heroPanel.fresh ();
			showPanel (heroPanel,heroBtn);
			break;
		case 3:
			showPanel (tupuPanel,tupuBtn);
			break;
		case 4:
			showPanel (selectChapterPanel,chapterBtn);
			break;
		case 5:
			showPanel (bagPanel, bagBtn);
			bagPanel.onclickBtn(1);
			//BagManager.getInstance ().showAll ();
			break;
		default:
			break;
		}
	}
}
