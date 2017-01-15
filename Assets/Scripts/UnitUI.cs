using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
	public GameObject buttonPrefab;

	public static UnitUI Instance;
	void Awake() { Instance = this; }

	void Start()
	{
		NotificationCenter.DefaultCenter.AddObserver(this, "Moving");
		NotificationCenter.DefaultCenter.AddObserver(this, "MoveDone");
	}

	public void Moving()
	{
		Debug.Log("MOVING");
		HideUI();
	}

	public void HideUI()
	{
		Debug.Log("HIDE");
		transform.GetChild(0).gameObject.SetActive(false);
	}

	public void MoveDone()
	{
		Debug.Log("MOVEDONE");
		UnhideUI();
	}

	public void UnhideUI()
	{
		Debug.Log("UNHIDE");
		transform.GetChild(0).gameObject.SetActive(true);
		UpdateUI();
	}

	public void UpdateUI()
	{
		foreach (Transform child in transform.GetChild(0))
			Destroy(child.gameObject);

		PawnAction[] allActions = Selector.Instance.selection.GetComponents<PawnAction>();
		foreach(PawnAction action in allActions)
		{
			if (action.GetComponent<Pawn>().ap >= action.cost)
			{
				GameObject currentButton = Instantiate(buttonPrefab) as GameObject;
				currentButton.transform.SetParent(transform.GetChild(0));
				action.SetButton(currentButton);
			}
		}
	}
}
