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
			CalcTurnOrder();
		}
	}

	public void CalcTurnOrder()
	{
		turnOrder.Clear();
		Dictionary<float, Pawn> turns = new Dictionary<float, Pawn>();
		foreach(Pawn unit in units)
		{
			turns.Add(unit.GetInitiative(), unit);
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
		Selector.Instance.SetSelection(turnOrder[0]);
		UnitUI.Instance.UpdateUI();
	}

	public void NextUnit()
	{
		turnOrder.RemoveAt(0);
		if (turnOrder.Count == 0)
		{
			CalcTurnOrder();
		}
		Selector.Instance.SetSelection(turnOrder[0]);
		UnitUI.Instance.UpdateUI();
	}
}
