using UnityEngine;
using SimpleJson;
using System;
using System.Collections.Generic;
public class AudioManager
{

	public static readonly AudioManager instance = new AudioManager();

	public AudioSource m_AudioMgr;
	public Dictionary<int,AudioSource> soundCache;

	private AudioClip playClip;
	private string curMusicName = "";
	private AudioManager()
	{
		init();
	}

	public void init()
	{
		soundCache = new Dictionary<int, AudioSource> ();
	}
	public void PlayBG(string fileName)
	{
		if (!fileName.Equals(curMusicName))
		{
			playClip = Resources.Load(fileName) as AudioClip;
                m_AudioMgr.clip = playClip;
                m_AudioMgr.Play();
                curMusicName = fileName;
		}
		//Debug.Log("背景音乐: "+fileName);
	}






	public void PlayBG(AudioClip m_PlayClip)
	{
		m_AudioMgr.clip = m_PlayClip;
		m_AudioMgr.Play();
	}

	public void StopBG()
	{
		m_AudioMgr.Stop();
		curMusicName = "";
	}

	public AudioSource Play(AudioClip clip, Transform emitter, bool loop)
	{
		return Play(clip, emitter, 1f, 1f, loop);
	}

	public AudioSource Play(AudioClip clip, Transform emitter, float volume, bool loop)
	{
		return Play(clip, emitter, volume, 1f, loop);
	}


	public AudioSource Play(AudioClip clip, Transform emitter, float volume, float pitch, bool loop)
	{
		GameObject go = new GameObject("Audio:" + clip.name);
		go.transform.position = emitter.position;
		go.transform.parent = emitter;

		// create the source
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.pitch = pitch;
		source.loop = loop;

		if (!loop)
		{
			//GameObject.Destroy(go, clip.length);
		}

		return source;
	}

	public AudioSource Play(AudioClip clip, bool loop)
	{
		if(clip)
			return Play(clip, Vector3.zero, 1f, 1f, loop);
		else
		{
			return null;
		}
	}

	public AudioSource Play(AudioClip clip, Vector3 point, float volume, bool loop)
	{
		return Play(clip, point, volume, 1f, loop);
	}


	public AudioSource Play(AudioClip clip, Vector3 point, float volume, float pitch, bool loop)
	{
		GameObject go = new GameObject("Audio:" + clip.name);
		go.transform.position = point;
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.pitch = pitch;
		source.loop = loop;
		source.Play();
		if (!loop)
		{
			//GameObject.DestroyObject(go, clip.length);
		}
		return source;
	}

	public void PlayBackage()
	{
		AudioClip objPrefab = (AudioClip)Resources.Load("music/BG");
		Play(objPrefab, true);
	}


	public void PlayWin()
	{
		AudioClip objPrefab = (AudioClip)Resources.Load("music/win");
		Play(objPrefab, false);
	}
	public void playBg()
	{
		System.Random rd = new System.Random(); 　　　　
		int i = rd.Next(1,100); //1到100之间的数，可任意更改
		Play(i % 3 + 1);
	}
	public void playBtnClick()
	{
		Play(4);
	}
	public void playCloseClick()
	{
		Play(5);
	}
	public void playEquip()
	{
		Play(6);
	}
	public void playUnEquip()
	{
		Play(7);
	}
	public void playMenuClick(){
		Play(8);
	}
	public void Play(int id)
	{
		Loom.QueueOnMainThread (() => {
			
			if(!soundCache.ContainsKey(id)){
				JsonObject jo = DataManager.getInstance ().soundJson [id];
				AudioClip clip = (AudioClip)Resources.Load (jo ["path"].ToString ());
				bool loop = Convert.ToBoolean (jo ["loop"].ToString ());
				float volume = float.Parse(jo ["volume"].ToString ());
				soundCache[id] = Play(clip,  Vector3.zero, volume, 1f, loop);
			}
			AudioSource aso = soundCache[id];
			if(aso != null){
				soundCache[id].Play();
			}else{
				soundCache.Remove(id);
				Play(id);
			}

			//Play (clip, loop);
		});
	}
}