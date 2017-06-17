using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Observer:MonoBehaviour {
	public ArrayList messageArr;
	public List<Notification> notificationQueue;
	public Observer(){
		messageArr = new ArrayList ();
		notificationQueue = new List<Notification> ();
	}
	//public virtual void LocalMessage (Notification nt){
		
	//}
	void OnEnable(){
		for (int i = 0; i < messageArr.Count; i++) {
			NotificationManager.getInstance ().AddObserver (this,(int)messageArr[i]);
		}
		//Debug.Log("Observer OnEnable");
	}
	void OnDisable(){
		for (int i = 0; i < messageArr.Count; i++) {
			NotificationManager.getInstance ().RemoveObserver (this,(int)messageArr[i]);
		}
		//Debug.Log("Observer OnDisable");
	}
}
