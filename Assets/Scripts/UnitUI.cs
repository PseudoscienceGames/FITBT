﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
	public GameObject buttonPrefab;

	public static UnitUI Instance;
	void Awake() { Instance = this; }

	public void UpdateUI()
	{
		foreach (Transform child in transform)
			GameObject.Destroy(child.gameObject);

		PawnAction[] allActions = Selector.Instance.selection.GetComponents<PawnAction>();
		foreach(PawnAction action in allActions)
		{
			GameObject currentButton = Instantiate(buttonPrefab) as GameObject;
			currentButton.transform.SetParent(transform);
			currentButton.transform.GetChild(0).GetComponent<Text>().text = action.actionName;
			currentButton.GetComponent<Button>().onClick.AddListener(delegate { Cursor.Instance.SelectAction(action); });
		}
	}
}