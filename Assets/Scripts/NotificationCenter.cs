using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NotificationCenter : MonoBehaviour
{
	private static NotificationCenter defaultCenter;
	public static NotificationCenter DefaultCenter
	{
		get
		{
			if (!defaultCenter)
			{
				GameObject notificationObject = new GameObject("Default Notification Center");
				defaultCenter = notificationObject.AddComponent<NotificationCenter>();
			}
			return defaultCenter;
		}
	}



	Hashtable notifications = new Hashtable();

	public void AddObserver(Component observer, string name) { AddObserver(observer, name, null); }
	public void AddObserver(Component observer, string name, Component sender)
	{
		if (string.IsNullOrEmpty(name)) { Debug.Log("Null name specified for notification in AddObserver."); return; }
		if (notifications[name] == null)
			notifications[name] = new List<Component>();
		List<Component> notifyList = notifications[name] as List<Component>;
		if (!notifyList.Contains(observer)) { notifyList.Add(observer); }
	}

	public void RemoveObserver(Component observer, string name)
	{
		List<Component> notifyList = (List<Component>)notifications[name];
		if (notifyList != null)
		{
			if (notifyList.Contains(observer)) { notifyList.Remove(observer); }
			if (notifyList.Count == 0) { notifications.Remove(name); }
		}
	}

	public void PostNotification(Component aSender, string aName) { PostNotification(aSender, aName, null); }
	public void PostNotification(Component aSender, string aName, Hashtable aData) { PostNotification(new Notification(aSender, aName, aData)); }
	public void PostNotification(Notification aNotification)
	{
		if (string.IsNullOrEmpty(aNotification.name))
		{
			Debug.Log("Null name sent to PostNotification.");
			return;
		}
		List<Component> notifyList = (List<Component>)notifications[aNotification.name];
		if (notifyList == null)
		{
			Debug.Log("Notify list not found in PostNotification: " + aNotification.name);
			return;
		}

		List<Component> observersToRemove = new List<Component>();

		foreach (Component observer in notifyList)
		{
			if (!observer) { observersToRemove.Add(observer); }
			else
			{
				observer.SendMessage(aNotification.name, aNotification, SendMessageOptions.DontRequireReceiver);
			}
		}

		foreach (Component observer in observersToRemove)
		{
			notifyList.Remove(observer);
		}
	}

	public class Notification
	{
		public Component sender;
		public string name;
		public Hashtable data;
		public Notification(Component aSender, string aName) { sender = aSender; name = aName; data = null; }
		public Notification(Component aSender, string aName, Hashtable aData) { sender = aSender; name = aName; data = aData; }


	}
}