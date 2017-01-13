using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnController : MonoBehaviour
{
	public List<Pawn> units = new List<Pawn>();
	public List<Pawn> turnOrder = new List<Pawn>();

	public static TurnController Instance;
	void Awake() { Instance = this; }

	public bool go = false;

	void Update()
	{
		if(go)
		{
			go = false;
			NextUnit();
		}
	}

	public void CalcTurnOrder()
	{
		turnOrder.Clear();
		Dictionary<float, Pawn> turns = new Dictionary<float, Pawn>();
		foreach(Pawn unit in units)
		{
			turns.Add(unit.GetInitiative(), unit);
			unit.ap = unit.totalAP;
		}

		float highestInit = 0;
		while (turns.Count != 0)
		{
			foreach (KeyValuePair<float, Pawn> kvp in turns)
			{
				if (kvp.Key > highestInit)
					highestInit = kvp.Key;
			}
			turnOrder.Add(turns[highestInit]);
			turns.Remove(highestInit);
			highestInit = 0;
		}
		//UpdateForNextUnit();
    }

	public void NextUnit()
	{
		if (turnOrder.Count == 0)
			CalcTurnOrder();
		SelectNextUnit();
		turnOrder.RemoveAt(0);
	}

	void SelectNextUnit()
	{
		Selector.Instance.SetSelection(turnOrder[0]);
		CameraControl.Instance.FocusCamera(turnOrder[0].gridLoc);
		if (turnOrder[0].GetComponent<AIControl>() != null)
			turnOrder[0].GetComponent<AIControl>().Activate();
		else
			UnitUI.Instance.UpdateUI();
	}
}
