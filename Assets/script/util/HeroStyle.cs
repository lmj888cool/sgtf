using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;
using SimpleJson;

public class HeroStyle : MonoBehaviour {//英雄形象类，通用性，实现基本功能：1.滑动切换英雄 2.点击进入英雄详情


	public SkeletonGraphic skeletonGraphic;
	public Image heroStyleBg; 
	private JsonObject data;
	private bool mouseState = false;
	private Vector3 mouseDownPosition;
	public callBackFunc<JsonObject> Func;//滑动切换英雄
	public callBackFunc<JsonObject> ClickFunc;//点击
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		bool b2 = Input.GetMouseButton (0);
		if (b2) {
			
			if (!mouseState) {
				mouseDownPosition = Input.mousePosition;
				Vector3 p = this.GetComponent<Image> ().rectTransform.InverseTransformPoint (Input.mousePosition);
				mouseState = this.GetComponent<Image> ().rectTransform.rect.Contains (p);
				//mouseState = true;
			}
			if (mouseState) {
				mouseDown ();
			}

		} else {
			if (mouseState) {
				mouseState = false;
				mouseUp ();
			}
		}

	}
	void mouseDown(){
		float xoffset = Input.mousePosition.x - mouseDownPosition.x;
		skeletonGraphic.transform.localPosition = new Vector3 (skeletonGraphic.transform.localPosition.x + xoffset,skeletonGraphic.transform.localPosition.y, 0);
		mouseDownPosition = Input.mousePosition;
	}
	public void mouseUp(){

		if (Func != null) {
			ArrayList heroarr = HeroManager.getInstance ().getHerosArrayList ();
			int index = heroarr.LastIndexOf (data);
			if (skeletonGraphic.transform.localPosition.x > 100) {
				index = index - 1 >= 0 ? index - 1 : heroarr.Count - 1;
				Func (heroarr [index] as JsonObject);
			
			} else if( skeletonGraphic.transform.localPosition.x < -100)  {
				index = index + 1 < heroarr.Count ? index + 1 : 0;
				Func (heroarr [index] as JsonObject);
			}else if( skeletonGraphic.transform.localPosition.x < 5 &&  skeletonGraphic.transform.localPosition.x > -5){
				onClick ();
			}
		}
		skeletonGraphic.transform.localPosition = new Vector3 (0,0, 0);
		
	}
	public void onClick(){
		if (ClickFunc != null) {
			ClickFunc (data);
		}
	}
	public void init(JsonObject jo){
		data = jo;
		if (skeletonGraphic != null) {
			skeletonGraphic.gameObject.SetActive (false);
			skeletonGraphic.transform.parent = null;
		}
		//skeletonGraphic.Clear ();
		JsonObject staticdata = HeroManager.getInstance().getHeroStaticData(jo);
		skeletonGraphic = HeroManager.getInstance ().getSkeletonGraphic (staticdata["style"].ToString(),skeletonGraphic);
		skeletonGraphic.gameObject.SetActive (true);
		skeletonGraphic.transform.SetParent(this.transform);
		skeletonGraphic.transform.localPosition = new Vector3 (0.0f,0.0f,0.0f);
		skeletonGraphic.AnimationState.SetAnimation (0, "attack", false);
		skeletonGraphic.AnimationState.AddAnimation (0, "stand", true,0.0f);

		//if (heroStyleBg.isActiveAndEnabled) {
			heroStyleBg.sprite = (Resources.Load("all/hero_bg_" + staticdata["color"].ToString(), typeof(Sprite)) as Sprite);
		//}

		
	}
}
