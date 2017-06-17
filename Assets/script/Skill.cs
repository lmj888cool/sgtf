using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Skill : MonoBehaviour {
	private Sprite[] sprites;
	private int spriteIndex = 0;//序列帧索引
	private int spriteIndexStart = 0;//序列帧某个动作起始帧
	private int spriteIndexEnd = 0;//序列帧索引某个动作结束帧
	private float spriteChangeSpeed = 0.1f;//序列帧切换速度
	private float spriteChangeTime = 0.0f;//序列帧切换速度
	private Image img;
	private bool isPlay = false;
	public AudioSource music;
	public bool isPalyOne = false;//技能序列帧是否播放了一轮
	public Rect attackRange;
	private skillData skilldata;
	public int skillId;
	public bool isCanAttack = false;
	// Use this for initialization
	void Start () {
		SkillManager.getInstance().setSkillDemo (this);

	}
	void FixedUpdate(){
		if (isPlay) {
			if (sprites.Length > 1) {
				if (spriteChangeTime > spriteChangeSpeed){
					spriteChangeTime = 0;
					if (spriteIndexEnd > spriteIndex) {
						img.sprite = sprites [spriteIndex];
						img.SetNativeSize ();
						spriteIndex++;
						if(skilldata.startAttackIndex <= spriteIndex){
							isCanAttack = true;
							if(skilldata.shakeScreenNum > 0)
								iTween.ShakePosition(ChapterScene._chapterScene.bg.gameObject, new Vector3(5.0f, 5.0f, 0.0f), skilldata.shakeScreenNum);
						}
					} else {
						spriteIndex = spriteIndexStart;
						isPalyOne = true;
					}
				}
			}
			spriteChangeTime += Time.fixedDeltaTime;
		}

	}
	// Update is called once per frame
	void Update () {
	}
	public void playMusic(){
		if(skilldata != null && skilldata.music != "" && isPlay){
			
			music.Play();
		}
	}
	public void init(skillData skilldata){
		isPalyOne = false;
		spriteIndex = 0;
		spriteIndexStart = 0;
		isCanAttack = false;
		if (skillId == skilldata.skillId) {
			img.sprite = sprites [spriteIndex];
			spriteIndex++;
			img.SetNativeSize ();
			gameObject.SetActive (true);
			isPlay = true;
			playMusic ();
			return;
		}
			
		this.skilldata = skilldata;
		skillId = skilldata.skillId;
		img = this.GetComponent<UnityEngine.UI.Image> ();
		// 加载此文件下的所有资源
		sprites = Resources.LoadAll<Sprite>(skilldata.skillEffectName);
		img.sprite = sprites [spriteIndex];
		img.SetNativeSize ();
		img.rectTransform.pivot = new Vector2 (0.5f,skilldata.effectPriotY);
		attackRange = img.rectTransform.rect;
		attackRange.width = skilldata.attackRange;
		attackRange.height = skilldata.attackRange;
		//RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
		//rectTransform.localPosition = new Vector3(200, 60, 0);
		//rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, skilldata.attackRange);
		//transform.localScale = new Vector3 (skilldata.effectScale,skilldata.effectScale,skilldata.effectScale);
		spriteIndex++;
		gameObject.SetActive (true);
		spriteIndexEnd = sprites.Length;
		//music.pl
		music.clip = (AudioClip)Resources.Load(skilldata.music, typeof(AudioClip));//调用Resources方法加载AudioClip资源
		music.loop = true;

		//iTween.ShakePosition(ChapterScene._chapterScene.bg.gameObject, new Vector3(5.0f, 5.0f, 0.0f), 1);
		isPlay = true;
		playMusic ();
	}
}
