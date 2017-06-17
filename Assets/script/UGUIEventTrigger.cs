
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
	public class UGUIEventTrigger : EventTrigger
	{
		public static UGUIEventTrigger Get(GameObject go)
		{
			UGUIEventTrigger trigger = go.GetComponent<UGUIEventTrigger>();
			if (null == trigger)
			{
				trigger = go.AddComponent<UGUIEventTrigger>();
			}
			return trigger;
		}

		public void AddEventListener(EventTriggerType eventTriggerType, UnityAction<BaseEventData> action)
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = eventTriggerType;
			entry.callback.AddListener(action);
			if (null == triggers)
			{
			triggers = new List<Entry>();
			}
			triggers.Add(entry);
		}
	}
