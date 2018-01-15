using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pawn : MapObject
{
	public float totalHP;
	public float hp;
	public float totalAP;
	public float ap;
	public Image healthBar;
	public Image actionBar;

	public override void Start()
	{
		base.Start();
		TurnController.Instance.units.Add(this);
	}

	public float GetInitiative()
	{
		return Random.Range(0f, 100f);
	}

	public void EndTurn()
	{
		TurnController.Instance.NextUnit();
	}

	public void TakeDamage(float damage)
	{
		hp -= damage;
		healthBar.fillAmount = hp / totalHP;
		if (hp < 0)
			Kill();
	}

	public void UseAP(float amt)
	{
		ap -= amt;
		actionBar.fillAmount = ap / totalAP;
	}

	public void Kill()
	{
		TurnController.Instance.units.Remove(this);
		if(TurnController.Instance.turnOrder.Contains(this))
			TurnController.Instance.turnOrder.Remove(this);
		IslandData.Instance.mapObjects.Remove(gridLoc);
		Destroy(gameObject);
	}
}
